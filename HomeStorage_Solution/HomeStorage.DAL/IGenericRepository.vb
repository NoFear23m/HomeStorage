Imports System.Linq.Expressions

Public Interface IGenericRepository(Of T As Class)


    Property Context As Object
    Function GetAll(Optional tracking As Boolean = False) As IQueryable(Of T)

    Function GetAsync(query As IQueryable(Of T)) As Task(Of List(Of T))


    Function Find(id As Integer) As T

    Function FindAsync(id As Integer) As Task(Of T)

    Function FindBy(predicate As Expression(Of Func(Of T, Boolean)), Optional tracking As Boolean = False) As IQueryable(Of T)


    Sub Add(entity As T)

    Sub Delete(entity As T)

    Sub Edit(entity As T)

    Sub Attach(entity As T)

    Function ReferenceLoaded(entity As T, prop As String) As Boolean
    Function CollectionLoaded(entity As T, prop As String) As Boolean

    Sub LoadReference(entity As T, prop As String)

    Sub LoadCollection(entity As T, prop As String)

    Function SetTracking(tracking As Boolean, query As IQueryable(Of T)) As IQueryable(Of T)


    Function CountAll(Optional includeDeleted As Boolean = False) As Integer
    Function CountAllAsync(Optional includeDeleted As Boolean = False) As Task(Of Integer)

    Function Any(Optional includeDeleted As Boolean = False) As Boolean
    Function AnyAsync(Optional includeDeleted As Boolean = False) As Task(Of Boolean)

    Function Any(query As IQueryable(Of T)) As Boolean
    Function AnyAsync(query As IQueryable(Of T)) As Task(Of Boolean)

    Function Save() As Integer
    Function SaveAsync() As Task(Of Integer)

End Interface