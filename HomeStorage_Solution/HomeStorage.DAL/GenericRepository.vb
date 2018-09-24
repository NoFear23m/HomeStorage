

Imports System.Linq.Expressions
Imports Microsoft.EntityFrameworkCore
Imports System.Runtime.CompilerServices
Imports HomeStorage.DbContext

Public MustInherit Class GenericRepository(Of T As Class)
    Implements IGenericRepository(Of T)
    Implements IDisposable

    Private ReadOnly _disposeContext As Boolean = True

    Private _entities As New Object
    Public Property Context() As Object Implements IGenericRepository(Of T).Context

        Get
            Return _entities
        End Get
        Set
            _entities = Value
        End Set
    End Property

    Friend ReadOnly Property ContextInternal As StorageContext
        Get
            Return CType(Context, StorageContext)
        End Get
    End Property



    Protected Sub New()
        Me.New(Nothing, False)
    End Sub


    Protected Friend Sub New(Optional kontext As StorageContext = Nothing, Optional tracking As Boolean = False)
        Debug.WriteLine("Create new instance of GenericRepository...")

        ' Falls ein Kontext hineingereicht wurde, nehme diesen!
        If kontext IsNot Nothing Then
            Context = kontext
            _disposeContext = False
        Else
            Context = New StorageContext()
        End If


        ContextInternal.ChangeTracker.AutoDetectChangesEnabled = tracking
        If tracking Then
            ContextInternal.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll
        Else
            ContextInternal.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking
        End If


    End Sub


    Public Property ChangeTrackerTrackAll() As Boolean
        Get
            If ContextInternal.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll Then
                Return True
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            If value = True Then
                ContextInternal.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll
                ContextInternal.ChangeTracker.AutoDetectChangesEnabled = True
            Else
                ContextInternal.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking
                ContextInternal.ChangeTracker.AutoDetectChangesEnabled = False
            End If
        End Set
    End Property


#Region "IGenericRepository Implementierung"

    Public Overridable Function GetAll(Optional tracking As Boolean = False) As IQueryable(Of T) Implements IGenericRepository(Of T).GetAll
        Debug.WriteLine($"Calling GetAll. Tracking: {tracking.ToString}")
        Dim query As IQueryable(Of T) = CType(_entities, StorageContext).[Set](Of T)()
        If tracking Then query = query.AsTracking
        Debug.WriteLine("GetAll - Return Query")
        Return query
    End Function

    Public Overridable Async Function GetAsync(query As IQueryable(Of T)) As Task(Of List(Of T)) Implements IGenericRepository(Of T).GetAsync
        Debug.WriteLine($"GetAllAsync (Direct return) for T of Type {GetType(T).ToString}")
#If DEBUG Then
        Threading.Thread.Sleep(500)
#End If
        Return Await query.ToListAsync
    End Function


    Public Overridable Function Find(id As Integer) As T Implements IGenericRepository(Of T).Find
        Debug.WriteLine($"Find (Direct return) for T of Type {GetType(T).ToString}")
        Return CType(_entities, StorageContext).[Set](Of T).Find(id)
    End Function

    Public Overridable Async Function FindAsync(id As Integer) As Task(Of T) Implements IGenericRepository(Of T).FindAsync
        Debug.WriteLine($"FindAsync (Direct return) for T of Type {GetType(T).ToString}")
        Return Await CType(_entities, StorageContext).[Set](Of T).FindAsync(id)
    End Function


    Public Overridable Function FindBy(predicate As Expression(Of Func(Of T, Boolean)), Optional tracking As Boolean = False) As IQueryable(Of T) Implements IGenericRepository(Of T).FindBy
        Debug.WriteLine($"FindBy for T of Type {GetType(T).ToString} - Tracking: {tracking.ToString}")
        Dim query As IQueryable(Of T) = CType(_entities, StorageContext).[Set](Of T)().Where(predicate)
        If tracking Then query = query.AsTracking
        Debug.WriteLine("FindBy - Return Query")
        Return query
    End Function



    Public Overridable Sub Add(entity As T) Implements IGenericRepository(Of T).Add
        Debug.WriteLine($"Add (entity) of Type {GetType(T).ToString}")
        CType(_entities, StorageContext).[Set](Of T)().Add(entity)
    End Sub

    Public Overridable Function AddAsync(entity As T) As Task
        Debug.WriteLine($"Add (entity) of Type {GetType(T).ToString} async")
        Return CType(_entities, StorageContext).[Set](Of T)().AddAsync(entity)
    End Function


    Public Overridable Sub Delete(entity As T) Implements IGenericRepository(Of T).Delete
        Debug.WriteLine($"Delete (entity) of Type {GetType(T).ToString}")
        CType(_entities, StorageContext).[Set](Of T)().Remove(entity)
    End Sub


    ''' <summary>
    ''' Setze die komplette Entität (nur auf erster Ebene) auf Modified
    ''' </summary>
    ''' <param name="entity"></param>
    Public Overridable Sub Edit(entity As T) Implements IGenericRepository(Of T).Edit
        Debug.WriteLine($"Edit (entity) of Type {GetType(T).ToString}")
        CType(_entities, StorageContext).Entry(entity).State = EntityState.Modified
        Debug.WriteLine($"Successfully set EntryState to 'Modified'. Entity of Type: {entity.GetType.ToString}")
    End Sub

    Public Overridable Sub Attach(entity As T) Implements IGenericRepository(Of T).Attach
        Debug.WriteLine($"Attach (entity) of Type {GetType(T).ToString}")
        CType(_entities, StorageContext).Attach(entity)
        Debug.WriteLine($"Successfully attached Entity of Type: {entity.GetType.ToString}")
    End Sub



    Public Overridable Sub LoadReference(entity As T, prop As String) Implements IGenericRepository(Of T).LoadReference
        Debug.WriteLine($"Try to load Reference '{entity.GetType.ToString}' for Entity of Type {prop.ToString}")
        If CType(_entities, StorageContext).Entry(entity).Reference(prop).IsLoaded Then
            Debug.WriteLine($"Reference '{prop.ToString}' is allready loaded. Abort loading reference")
        Else
            CType(_entities, StorageContext).Entry(entity).Reference(prop).Load()
            Debug.WriteLine($"Reference '{prop.ToString}' was loaded.")
        End If
    End Sub

    Public Overridable Sub LoadCollection(entity As T, prop As String) Implements IGenericRepository(Of T).LoadCollection
        Debug.WriteLine($"Try to load Collection '{entity.GetType.ToString}' for Entity of Type {prop.ToString}")
        If CType(_entities, StorageContext).Entry(entity).Collection(prop).IsLoaded Then
            Debug.WriteLine($"Collection '{prop.ToString}' is allready loaded. Abort loading reference")
        Else
            CType(_entities, StorageContext).Entry(entity).Collection(prop).Load()
            Debug.WriteLine($"Collection '{prop.ToString}' was loaded.")
        End If

    End Sub

    Public Overridable Function ReferenceLoaded(entity As T, prop As String) As Boolean Implements IGenericRepository(Of T).ReferenceLoaded
        Return CType(_entities, StorageContext).Entry(entity).Reference(prop).IsLoaded
    End Function

    Public Overridable Function CollectionLoaded(entity As T, prop As String) As Boolean Implements IGenericRepository(Of T).CollectionLoaded
        Return CType(_entities, StorageContext).Entry(entity).Collection(prop).IsLoaded
    End Function




    Public Overridable Function SetTracking(tracking As Boolean, query As IQueryable(Of T)) As IQueryable(Of T) Implements IGenericRepository(Of T).SetTracking
        Debug.WriteLine($"SetTracking for Query of Type '{GetType(T).ToString}' to '{tracking.ToString}' and Return modified Query")
        If tracking Then
            Return query.AsTracking
        Else
            Return query.AsNoTracking
        End If
    End Function


    Public Overridable Function CountAll(Optional includeDeleted As Boolean = False) As Integer Implements IGenericRepository(Of T).CountAll
        If Not includeDeleted Then
            Return CType(_entities, StorageContext).[Set](Of T).Where(Function(e) DirectCast(e, Model.Interfaces.ILogicalDelete).DeletedFlag = False).Count()
        Else
            Return CType(_entities, StorageContext).[Set](Of T)().Count
        End If
    End Function

    Public Overridable Async Function CountAsync(Optional includeDeleted As Boolean = False) As Task(Of Integer) Implements IGenericRepository(Of T).CountAllAsync
        If Not includeDeleted Then
            Return Await CType(_entities, StorageContext).[Set](Of T).Where(Function(e) DirectCast(e, Model.Interfaces.ILogicalDelete).DeletedFlag = False).CountAsync()
        Else
            Return Await CType(_entities, StorageContext).[Set](Of T)().CountAsync()
        End If
    End Function


    Public Overridable Function Any(Optional includeDeleted As Boolean = False) As Boolean Implements IGenericRepository(Of T).Any
        If Not includeDeleted Then
            Return CType(_entities, StorageContext).[Set](Of T).Where(Function(e) DirectCast(e, Model.Interfaces.ILogicalDelete).DeletedFlag = False).Any()
        Else
            Return CType(_entities, StorageContext).[Set](Of T)().Any()
        End If
    End Function

    Public Overridable Async Function AnyAsync(Optional includeDeleted As Boolean = False) As Task(Of Boolean) Implements IGenericRepository(Of T).AnyAsync
        If Not includeDeleted Then
            Return Await CType(_entities, StorageContext).[Set](Of T).Where(Function(e) DirectCast(e, Model.Interfaces.ILogicalDelete).DeletedFlag = False).AnyAsync()
        Else
            Return Await CType(_entities, StorageContext).[Set](Of T)().AnyAsync()
        End If
    End Function

    Public Overridable Function Any(query As IQueryable(Of T)) As Boolean Implements IGenericRepository(Of T).Any
        Return query.Any
    End Function

    Public Overridable Async Function AnyAsync(query As IQueryable(Of T)) As Task(Of Boolean) Implements IGenericRepository(Of T).AnyAsync
        Return Await query.AnyAsync
    End Function





    Public Overridable Function Save() As Integer Implements IGenericRepository(Of T).Save
        Dim ergebnis As String = GetStatistik()
        Debug.WriteLine(ergebnis)

        Dim anz As Integer = 0
        ContextInternal.ChangeTracker.DetectChanges()
        anz = ContextInternal.SaveChanges()
        Debug.WriteLine($"GenericRepository:Save returned {anz.ToString} changes saved successfully")
        Return anz
    End Function

    Public Overridable Async Function SaveAsync() As Task(Of Integer) Implements IGenericRepository(Of T).SaveAsync
        Dim ergebnis As String = GetStatistik()
        Debug.WriteLine(ergebnis)

        ContextInternal.ChangeTracker.DetectChanges()
        Dim anz = Await ContextInternal.SaveChangesAsync
        Debug.WriteLine($"GenericRepository:Save returned {anz.ToString} changes saved successfully")
        Return anz
    End Function


#End Region

    Public Overridable Function HasContextChanges() As Boolean
        Debug.WriteLine("GenericRepository:HasContextChanges...")
        Debug.WriteLine("Detecting changes")
        If ContextInternal Is Nothing Then Return False
        ContextInternal.ChangeTracker.DetectChanges()
        If ContextInternal.ChangeTracker.HasChanges Then
            Debug.WriteLine("Context:ChangeTracker has changes, returning true")
            Return True
        Else
            Debug.WriteLine("Context:ChangeTracker has NO changes, returning false")
            Return False
        End If
    End Function


    ''' <summary>
    ''' Liefert Informationen über ChangeTracker-Status als Zeichenkette
    ''' </summary>
    Protected Overridable Function GetStatistik(Of TEntity As Class)() As String
        Dim statistik As String = ""
        statistik += "Changed: " + ContextInternal.ChangeTracker.Entries(Of TEntity)().Where(Function(x) x.State = EntityState.Modified).Count.ToString
        statistik += " New: " + ContextInternal.ChangeTracker.Entries(Of TEntity)().Where(Function(x) x.State = EntityState.Added).Count.ToString
        statistik += " Deleted: " + ContextInternal.ChangeTracker.Entries(Of TEntity)().Where(Function(x) x.State = EntityState.Deleted).Count.ToString
        Debug.WriteLine(statistik)
        Return statistik
    End Function

    ''' <summary>
    ''' Liefert Informationen über ChangeTracker-Status als Zeichenkette
    ''' </summary>
    Protected Overridable Function GetStatistik() As String
        Dim statistik As String = ""
        statistik += "Changed: " + ContextInternal.ChangeTracker.Entries().Where(Function(x) x.State = EntityState.Modified).Count.ToString
        statistik += " New: " + ContextInternal.ChangeTracker.Entries().Where(Function(x) x.State = EntityState.Added).Count.ToString
        statistik += " Deleted: " + ContextInternal.ChangeTracker.Entries().Where(Function(x) x.State = EntityState.Deleted).Count.ToString
        Debug.WriteLine(statistik)
        Return statistik
    End Function

    Public Overridable Function GetEntityOriginalValue(Of TEntity As Class)(entity As TEntity) As TEntity
        Return CType(ContextInternal.Entry(Of TEntity)(entity).OriginalValues.ToObject, TEntity)
    End Function


    Public Overridable Function GetEntityDbValue(Of TEntity As Class)(entity As TEntity) As TEntity
        Return CType(ContextInternal.Entry(Of TEntity)(entity).GetDatabaseValues.ToObject, TEntity)
    End Function

    Public Overridable Function CreateDbIfNotExist() As Boolean
        Return ContextInternal.Database.EnsureCreated()
    End Function


    Public Overridable Sub DetachAll()
        Debug.WriteLine("Detach all Entities!!")
        ContextInternal.DetachAll()
    End Sub


    ''' <summary>
    ''' DataManager vernichten (vernichtet auch den EF-Kontext)
    ''' </summary>
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Falls der Kontext von außen hineingereicht wurde, darf man nicht Dispose() aufrufen.
        ' Das ist dann Sache des Aufrufers
        If _disposeContext Then
            Debug.WriteLine("Disposing GenericRepository...")
            ContextInternal.Dispose()
            Debug.WriteLine("GenericRepository Disposed")
        Else
            Debug.WriteLine("Do not Dispose Context of GenericRepository because its created from another place!!")
        End If
    End Sub


End Class
