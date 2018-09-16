Imports System.ComponentModel.DataAnnotations

Public MustInherit Class ConcurrencyModelBase
    Inherits ModelBase

    <Timestamp>
    Public Property RowVersion As Byte()
End Class