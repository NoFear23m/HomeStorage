Imports System.ComponentModel.DataAnnotations
Imports Model.Interfaces

Namespace StorageModel
    Public Class SubStore
        Inherits ModelBase
        Implements ILogicalDelete

        <Key>
        Public Overridable Property SubStoreID As Integer
        <Required>
        Public Overridable Property Title As String
        Public Overridable Property Location As String
        Public Overridable Property Description As String
        Public Overridable Property Articles As ICollection(Of Article)
        Public Overridable Property StoreID As Integer
        Public Overridable Property Store As Store
        Public Property DeletedFlag As Boolean Implements ILogicalDelete.DeletedFlag
        Public Property DeletedTimestamp As Date? Implements ILogicalDelete.DeletedTimestamp
    End Class

End Namespace