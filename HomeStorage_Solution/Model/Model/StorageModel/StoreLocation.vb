Imports System.ComponentModel.DataAnnotations
Imports Model.Interfaces

Namespace StorageModel
    Public Class StoreLocation
        Inherits ModelBase
        Implements ILogicalDelete

        <Key>
        Public Overridable Property StoreLocationId As Integer
        <Required>
        Public Overridable Property Title As String

        Public Overridable Property StoreId As Integer
        Public Overridable Property Store As Store
        Public Property DeletedFlag As Boolean Implements ILogicalDelete.DeletedFlag
        Public Property DeletedTimestamp As Date? Implements ILogicalDelete.DeletedTimestamp
    End Class
End Namespace

