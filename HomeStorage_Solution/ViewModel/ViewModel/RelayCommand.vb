Imports System.Linq.Expressions
Imports System.Threading
Imports System.Windows.Input

Public Class RelayCommand : Implements ICommand


#Region "Fields"

    Private ReadOnly _execute As Action(Of Object)
    Private _canExecute As Func(Of Boolean)
    Private ReadOnly _observedPropertiesExpressions As HashSet(Of String) = New HashSet(Of String)
    Private ReadOnly _synchronizationContext As SynchronizationContext

#End Region

#Region "Constructor"

    Public Sub New(execute As Action(Of Object))
        Me.New(execute, Function() True)
    End Sub


    Public Sub New(execute As Action(Of Object), canExecute As Func(Of Boolean))
        If execute Is Nothing Then
            Throw New ArgumentNullException("execute")
        End If
        _synchronizationContext = SynchronizationContext.Current

        _execute = execute
        _canExecute = canExecute
    End Sub

#End Region

    Protected Overridable Sub OnCanExecuteChanged()
        If ((Not (_synchronizationContext) Is Nothing) _
                AndAlso (_synchronizationContext IsNot SynchronizationContext.Current)) Then
            _synchronizationContext.Post(Sub() RaiseEvent CanExecuteChanged(Me, EventArgs.Empty), Nothing)
        Else
            RaiseEvent CanExecuteChanged(Me, EventArgs.Empty)
        End If
    End Sub

    ''' <summary>
    ''' Observes a property that implements INotifyPropertyChanged, and automatically calls DelegateCommandBase.RaiseCanExecuteChanged on property changed notifications.
    ''' </summary>
    ''' <typeparam name="T">The object type containing the property specified in the expression.</typeparam>
    ''' <param name="propertyExpression">The property expression. Example: ObservesProperty(() => PropertyName).</param>
    Protected Sub ObservesPropertyInternal(Of T)(ByVal propertyExpression As Expression(Of Func(Of T)))
        If _observedPropertiesExpressions.Contains(propertyExpression.ToString) Then
            Throw New ArgumentException("{propertyExpression.ToString()} is already being observed.", NameOf(propertyExpression))
        Else
            _observedPropertiesExpressions.Add(propertyExpression.ToString)
            PropertyObserver.Observes(propertyExpression, Sub() RaiseCanExecuteChanged())
        End If

    End Sub

    ''' <summary>
    ''' Observes a property that implements INotifyPropertyChanged, and automatically calls DelegateCommandBase.RaiseCanExecuteChanged on property changed notifications.
    ''' </summary>
    ''' <typeparam name="T">The object type containing the property specified in the expression.</typeparam>
    ''' <param name="propertyExpression">The property expression. Example: ObservesProperty(() => PropertyName).</param>
    ''' <returns>The current instance of DelegateCommand</returns>
    Public Function ObservesProperty(Of T)(ByVal propertyExpression As Expression(Of Func(Of T))) As RelayCommand
        ObservesPropertyInternal(propertyExpression)
        Return Me
    End Function

    ''' <summary>
    ''' Observes a property that is used to determine if this command can execute, and if it implements INotifyPropertyChanged it will automatically call DelegateCommandBase.RaiseCanExecuteChanged on property changed notifications.
    ''' </summary>
    ''' <param name="canExecuteExpression">The property expression. Example: ObservesCanExecute(() => PropertyName).</param>
    ''' <returns>The current instance of DelegateCommand</returns>
    Public Function ObservesCanExecute(ByVal canExecuteExpression As Expression(Of Func(Of Boolean))) As RelayCommand
        _canExecute =  canExecuteExpression.Compile
        ObservesPropertyInternal(canExecuteExpression)
        Return Me
    End Function



#Region "ICommand Members"


    <DebuggerStepThrough>
    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return _canExecute()
    End Function


    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged



    Public Sub RaiseCanExecuteChanged()
        RaiseEvent CanExecuteChanged(Me, New EventArgs())
    End Sub


    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        _execute(parameter)
    End Sub

#End Region


End Class