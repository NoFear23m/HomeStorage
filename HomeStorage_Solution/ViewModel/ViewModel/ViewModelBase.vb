Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Runtime.CompilerServices


''' <summary>
''' Basisklasse für alle ViewModel-Klassen, jede ViewModel-Klasse sollte von dieser Basisklasse erben.
''' Implementiert INotifyPropertyChanged, INotifyPropertyChanging, IDataErrorInfo und IValidationInfo
''' </summary>
Public MustInherit Class ViewModelBase
    Implements INotifyPropertyChanged, IDataErrorInfo, IDisposable

    Private Shared ReadOnly HostProcesses As New List(Of String)({"XDesProc", "devenv", "WDExpress"})
    Private _isBusy As Boolean


    ''' <summary>
    ''' Setzt den zustand des ViewModel ob sich dieses in einem Ladezustand befindet oder nicht.
    ''' Für eine Logausgabe muss das LogAction Propertie genutzt werden.
    ''' </summary>
    ''' <returns></returns>
    Public Overridable Property VmIsBusy As Boolean
        Get
            Return _isBusy
        End Get
        Set
            _isBusy = Value
            RaisePropertyChanged()
        End Set
    End Property



    ''' <summary>
    ''' Gibt zurück ob sich die ausführung des Codes aktuell in der Entwicklungszeit oder in der Laufzeit befindet.
    ''' Für eine Logausgabe muss das LogAction Propertie genutzt werden.
    ''' </summary>
    ''' <returns>Wird Code des ViewModels vom XAML Designer ausgeführt wird True zurückgegeben.</returns>
    Public ReadOnly Property IsInDesignMode As Boolean
        Get
            Return HostProcesses.Contains(Process.GetCurrentProcess().ProcessName)
        End Get
    End Property



#Region "INotifyPropertyChanged"

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged


    ''' <summary>
    ''' Prozedur wirft den INotifyPropertyChanged Event welcher in der WPF benötigt wird um die UI zu verständingen
    ''' das eine Änderung an einem Property stattgefunden hat.
    ''' </summary>
    ''' <param name="prop">Das Propertie welches sich geändert hat. Ist Optional da als Parameter "CallerMemberName" verwendet wird. Wird Nothing übergeben werden alle PRoperties des Views aktualisiert!!</param>
    Protected Overridable Sub RaisePropertyChanged(<CallerMemberName> Optional ByVal prop As String = "")
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(prop))
    End Sub


#End Region


    Public Overridable Function ValidationErrors() As List(Of ValidationResult)
        Return Nothing
    End Function

    Public ReadOnly Property IsValid As Boolean
        Get
            Return ValidationErrors() IsNot Nothing AndAlso ValidationErrors.Count = 0
        End Get
    End Property


#Region "IDataErrorInfo Implementation"

    Default Public ReadOnly Property Item(columnName As String) As String Implements IDataErrorInfo.Item
        Get
            Dim valRes As ValidationResult = ValidationErrors.Where(Function(v) v.MemberNames.Contains(columnName) = True).FirstOrDefault
            If valRes Is Nothing Then
                Return Nothing
            End If
            'CommandManager.InvalidateRequerySuggested()
            Return valRes.ErrorMessage
        End Get
    End Property

    Public Overridable ReadOnly Property [Error] As String Implements IDataErrorInfo.Error
        Get
            Return Nothing
        End Get
    End Property

#End Region



#Region "IDisposable Support"
    Private _disposedValue As Boolean

    ' Dient zur Erkennung redundanter Aufrufe.

    ' IDisposable
    Public Overridable Sub Dispose(disposing As Boolean)
        If Not _disposedValue Then
            If disposing Then

            End If
        End If
        _disposedValue = True
    End Sub


    ' Dieser Code wird von Visual Basic hinzugefügt, um das Dispose-Muster richtig zu implementieren.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in Dispose(disposing As Boolean) weiter oben ein.

        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


#End Region

End Class