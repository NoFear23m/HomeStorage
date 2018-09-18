Imports System.ComponentModel.DataAnnotations
Imports Model.Interfaces

Namespace StorageModel

    Public Class Attachment
        Inherits ModelBase
        Implements ILogicalTimestamp

        <Key>
        Public Overridable Property AttachmentId As Integer

        Public Overridable Property File As Byte()

        <Required()>
        <MaxLength(150)>
        Public Overridable Property Title As String

        <Required>
        <MinLength(2)>
        <MaxLength(5)>
        Public Overridable Property FileExtension As String

        Public Overridable Property Notes As String


        Public Property LastUpdateTimestamp As Date? Implements ILogicalTimestamp.LastUpdateTimestamp
        Public Property CreationTimestamp As Date? Implements ILogicalTimestamp.CreationTimestamp
    End Class
End Namespace