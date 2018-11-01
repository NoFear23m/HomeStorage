Imports System.Windows.Input

Public Class RelayCommand : Implements ICommand


#Region "Fields"

    ReadOnly _execute As Action(Of Object)

#End Region

#Region "Constructor"

    Public Sub New(execute As Action(Of Object))
        If execute Is Nothing Then
            Throw New ArgumentNullException("execute")
        End If

        _execute = execute
    End Sub

#End Region

    Private _isEnabled As Boolean
    Public Property IsEnabled As Boolean
        Get
            Return _isEnabled
        End Get
        Set(value As Boolean)
            If (value <> _isEnabled) Then
                _isEnabled = value
                RaiseEvent CanExecuteChanged(Me, New EventArgs())
            End If
        End Set
    End Property


#Region "ICommand Members"


    <DebuggerStepThrough>
    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return IsEnabled
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