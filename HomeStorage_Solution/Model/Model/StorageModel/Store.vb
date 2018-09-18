Imports System.ComponentModel.DataAnnotations
Imports Model.Interfaces

Namespace StorageModel
    Public Class Store
        Inherits ModelBase
        Implements ILogicalDelete

        <Key>
        Public Overridable Property StoreId As Integer
        <Required>
        Public Overridable Property Title As String

        Public Overridable Property Location As String
        Public Overridable Property Description As String


        Public Overridable Property StoreInfos As ICollection(Of StoreInfo)


        Public Property DeletedFlag As Boolean Implements ILogicalDelete.DeletedFlag
        Public Property DeletedTimestamp As Date? Implements ILogicalDelete.DeletedTimestamp

    End Class
End Namespace