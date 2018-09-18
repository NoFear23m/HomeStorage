Imports System.ComponentModel.DataAnnotations
Imports Model.Interfaces

Namespace StorageModel
    Public Class StoreInfo
        Inherits ModelBase
        Implements ILogicalTimestamp

        <Key>
        Public Overridable Property StoreInfoId As Integer


        Public Overridable Property StoreId As Integer
        <Required>
        Public Overridable Property Store As Store

        <Required>
        <MinLength(1)>
        <MaxLength(50)>
        Public Overridable Property StorageLocation As String


        Public Overridable Property ArticleId As Integer
        <Required>
        Public Overridable Property Article As Article


        Public Property LastUpdateTimestamp As Date? Implements ILogicalTimestamp.LastUpdateTimestamp

        Public Property CreationTimestamp As Date? Implements ILogicalTimestamp.CreationTimestamp

    End Class
End Namespace
