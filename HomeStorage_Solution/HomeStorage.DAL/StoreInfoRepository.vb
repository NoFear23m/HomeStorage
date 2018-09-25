Imports HomeStorage.DbContext

Public Class StoreInfoRepository
    Inherits GenericRepository(Of Model.StorageModel.StoreInfo)

    Public Sub New()
        MyBase.New
    End Sub

    Public Sub New(context As Object)
        MyBase.New(kontext:=CType(context, StorageContext))
    End Sub

    Friend Sub New(ctx As StorageContext)
        MyBase.New(kontext:=ctx)
    End Sub

End Class
