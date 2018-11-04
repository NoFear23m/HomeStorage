Imports System.ComponentModel
Imports System.Linq.Expressions

Friend Class PropertyObserver

    Private ReadOnly _action As Action

    Private Sub New(ByVal propertyExpression As Expression, ByVal action As Action)
        MyBase.New
        _action = action
        SubscribeListeners(propertyExpression)
    End Sub

    Private Sub SubscribeListeners(ByVal propertyExpression As Expression)
        Dim propNameStack = New Stack(Of String)

        While TryCast(propertyExpression, MemberExpression) IsNot Nothing
            propNameStack.Push(DirectCast(propertyExpression, MemberExpression).Member.Name) ' Records the name of each property.
            propertyExpression = DirectCast(propertyExpression, MemberExpression).Expression
        End While


        If Not (TypeOf propertyExpression Is ConstantExpression) Then
            Throw New NotSupportedException("Operation not supported for the given expression type. " + "Only MemberExpression and ConstantExpression are currently supported.")
        End If


        Dim propObserverNodeRoot = New PropertyObserverNode(CType(propNameStack.Pop(), String), _action)
        Dim previousNode As PropertyObserverNode = CType(propObserverNodeRoot, PropertyObserverNode)
        For Each propName As String In propNameStack
            Dim currentNode As PropertyObserverNode = New PropertyObserverNode(propName, _action)
            previousNode.Next = currentNode
            previousNode = currentNode
        Next
        Dim propOwnerObject As Object = DirectCast(propertyExpression, ConstantExpression).Value
        If Not (TypeOf propOwnerObject Is INotifyPropertyChanged) Then
            Throw New InvalidOperationException("Trying to subscribe PropertyChanged listener in object that " +
                                                $"owns '{propObserverNodeRoot.PropertyName}' property, but the object does not implements INotifyPropertyChanged.")
        End If
        propObserverNodeRoot.SubscribeListenerFor(DirectCast(propOwnerObject, INotifyPropertyChanged))
    End Sub

    ''' <summary>
    ''' Observes a property that implements INotifyPropertyChanged, and automatically calls a custom action on 
    ''' property changed notifications. The given expression must be in this form: "() => Prop.NestedProp.PropToObserve".
    ''' </summary>
    ''' <param name="propertyExpression">Expression representing property to be observed. Ex.: "() => Prop.NestedProp.PropToObserve".</param>
    ''' <param name="action">Action to be invoked when PropertyChanged event occours.</param>
    Friend Shared Function Observes(Of T)(ByVal propertyExpression As Expression(Of Func(Of T)), ByVal action As Action) As PropertyObserver
        Return New PropertyObserver(propertyExpression.Body, action)
    End Function
End Class