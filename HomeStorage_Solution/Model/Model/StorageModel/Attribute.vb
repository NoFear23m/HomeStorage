Imports System.ComponentModel.DataAnnotations
Imports Model.Interfaces

Namespace StorageModel
    Public Class Attribute
        Inherits ModelBase
        Implements ILogicalDelete

        <Key>
        Public Overridable Property AttributeId As Integer
        <Required>
        <MinLength(2)>
        <MaxLength(50)>
        Public Overridable Property Title As String
        Public Overridable Property Description As String
        <Required>
        <MaxLength(50)>
        Public Overridable Property Unit As String


        Public Overridable Property ArticleAttributes As ICollection(Of ArticleAttribute)

        Public Property DeletedFlag As Boolean Implements ILogicalDelete.DeletedFlag
        Public Property DeletedTimestamp As Date? Implements ILogicalDelete.DeletedTimestamp

    End Class
End Namespace