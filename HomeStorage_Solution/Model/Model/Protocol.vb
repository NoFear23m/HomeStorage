Imports System.ComponentModel.DataAnnotations
Imports Model.Interfaces
Imports Model.StorageModel

Public Class Protocol
    Inherits ModelBase
    Implements ILogicalTimestamp

    Public Sub New()

    End Sub

    Public Sub New(protocoltext As String, createdByUserId As Integer)
        Me.ProtocolText = protocoltext
        Me.CreatedByUserId = createdByUserId
    End Sub


    <Key>
    Public Overridable Property ProtocolId As Integer

    <Required>
    Public Overridable Property ProtocolText As String

    Public Overridable Property CreatedByUserId As Integer?


#Region "LinkedTo"

    Public Overridable Property LinkedToArticleId As Integer?
    Public Overridable Property LinkedToArticle As Article

#End Region

    Public Overridable Property LastUpdateTimestamp As DateTime? Implements ILogicalTimestamp.LastUpdateTimestamp

    Public Overridable Property CreationTimestamp As DateTime? Implements ILogicalTimestamp.CreationTimestamp



    Public Overrides Function ToString() As String
        Return $"Text:{ProtocolText}"
    End Function

End Class
