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

        ''' <summary>
        ''' Für mich wäre die Location ja die Ansammlung von den Lagerplätzen, daher hier geändert
        ''' Title wäre der Name des Lagers e.g. Abstellraum, Location wäre dann der Lagerort e.g. Regal links
        ''' </summary>
        Public Overridable Property Location As ICollection(Of StoreLocation)
        Public Overridable Property Description As String
        Public Overridable Property Articles As ICollection(Of Article)
        Public Property DeletedFlag As Boolean Implements ILogicalDelete.DeletedFlag
        Public Property DeletedTimestamp As Date? Implements ILogicalDelete.DeletedTimestamp

    End Class
End Namespace