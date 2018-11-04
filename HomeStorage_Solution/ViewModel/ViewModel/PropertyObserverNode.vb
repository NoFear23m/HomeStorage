Imports System.ComponentModel
Imports System.Reflection

''' <summary>
''' Represents each node of nested properties expression and takes care of 
''' subscribing/unsubscribing INotifyPropertyChanged.PropertyChanged listeners on it.
''' </summary>
Class PropertyObserverNode

    Private ReadOnly _action As Action
    Private _inpcObject As INotifyPropertyChanged

    Public ReadOnly Property PropertyName As String

    Public Property [Next] As PropertyObserverNode

    Public Sub New(ByVal propertyName As String, ByVal action As Action)
        MyBase.New
        Me.PropertyName = propertyName

        _action = New Action(Sub() DoAction(action))

    End Sub
    Private Sub DoAction(action As Action)
        action?.Invoke()
        If ([Next] Is Nothing) Then
            Return
        End If

        [Next].UnsubscribeListener()
        GenerateNextNode()
    End Sub

    Public Sub SubscribeListenerFor(ByVal inpcObject As INotifyPropertyChanged)
        _inpcObject = inpcObject
        AddHandler _inpcObject.PropertyChanged, AddressOf OnPropertyChanged
        If (Not ([Next]) Is Nothing) Then
            GenerateNextNode()
        End If

    End Sub

    Private Sub GenerateNextNode()
        Dim propertyInfo = _inpcObject.GetType.GetRuntimeProperty(PropertyName) ' TODO: To cache, if the step consume significant performance. Note: The type of _inpcObject may become its base type or derived type.
        Dim nextProperty = propertyInfo.GetValue(_inpcObject)
        If (nextProperty Is Nothing) Then
            Return
        End If

        If Not (TypeOf nextProperty Is INotifyPropertyChanged) Then
            Throw New InvalidOperationException("Trying to subscribe PropertyChanged listener in object that " +
                                                $"owns '{[Next].PropertyName}' property, but the object does not implements INotifyPropertyChanged.")

        End If

        [Next].SubscribeListenerFor(DirectCast(nextProperty, INotifyPropertyChanged))
    End Sub

    Private Sub UnsubscribeListener()
        If (Not (_inpcObject) Is Nothing) Then
            AddHandler _inpcObject.PropertyChanged, AddressOf OnPropertyChanged
        End If

        [Next]?.UnsubscribeListener()
    End Sub

    Private Sub OnPropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs)
        ' Invoke action when e.PropertyName == null in order to satisfy:
        '  - DelegateCommandFixture.GenericDelegateCommandObservingPropertyShouldRaiseOnEmptyPropertyName
        '  - DelegateCommandFixture.NonGenericDelegateCommandObservingPropertyShouldRaiseOnEmptyPropertyName
        If ((e?.PropertyName = PropertyName) _
                    OrElse (e?.PropertyName Is Nothing)) Then
            _action?.Invoke
        End If

    End Sub
End Class
