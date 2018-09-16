Imports System.ComponentModel.DataAnnotations
Imports Model.Interfaces

Namespace StorageModel

    Public Class Article
        Inherits ConcurrencyModelBase
        Implements ILogicalDelete
        Implements ILogicalTimestamp
        Implements IProtocolable

        <Key>
        Public Overridable Property ArticleId As Integer

        <Required>
        <MinLength(10)>
        <MaxLength(50)>
        Public Overridable Property Title As String
        Public Overridable Property Description As String

        Public Overridable Property Attributes As ICollection(Of ArticleAttribute)
        Public Overridable Property Stock As Integer
        Public Overridable Property MinimumStock As Integer

        Public Overridable Property StoreId As Integer
        Public Overridable Property Store As Store



        Public Property DeletedFlag As Boolean Implements ILogicalDelete.DeletedFlag
        Public Property DeletedTimestamp As Date? Implements ILogicalDelete.DeletedTimestamp
        Public Property LastUpdateTimestamp As Date? Implements ILogicalTimestamp.LastUpdateTimestamp
        Public Property CreationTimestamp As Date? Implements ILogicalTimestamp.CreationTimestamp
        Public Property Protocol As ICollection(Of Protocol) Implements IProtocolable.Protocol

    End Class
End Namespace