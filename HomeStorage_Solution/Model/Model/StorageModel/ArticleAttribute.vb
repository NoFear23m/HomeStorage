Imports System.ComponentModel.DataAnnotations
Imports Model.Interfaces

Namespace StorageModel
    Public Class ArticleAttribute
        Inherits ModelBase
        Implements ILogicalDelete

        <Key>
        Public Overridable Property ArticleAttributeId As Integer
        <Required>
        Public Overridable Property Attribute As Attribute
        <Required>
        <MinLength(1)>
        <MaxLength(50)>
        Public Overridable Property Value As String

        Public Property DeletedFlag As Boolean Implements ILogicalDelete.DeletedFlag

        Public Property DeletedTimestamp As Date? Implements ILogicalDelete.DeletedTimestamp

    End Class
End Namespace
