Imports System.ComponentModel.DataAnnotations
Imports Model.Interfaces

Namespace StorageModel

    Public Class Setting
        Inherits ModelBase
        Implements ILogicalTimestamp

        <Key>
        Public Overridable Property SettingId As Integer
        <Required>
        <MaxLength(250)>
        Public Overridable Property Key As String
        <Required>
        Public Overridable Property Value As String
        <Required>
        Public Overridable Property DefaultValue As String
        <Required>
        Public Overridable Property Category As SettingCategory
        Public Overridable Property MinValue As String
        Public Overridable Property MaxValue As String

        ''' <summary>
        ''' Bestimmt ob der Wert automatisch durch das Programm oder deren Ablauf geändert wird.
        ''' </summary>
        ''' <returns></returns>
        <Required>
        Public Overridable Property IsAutomated As Boolean
        Public Overridable Property IsHidden As Boolean
        <Required>
        Public Overridable Property DataType As EnumDataType

        Public Overridable Property LastUpdateTimestamp As Date? Implements ILogicalTimestamp.LastUpdateTimestamp
        Public Overridable Property CreationTimestamp As Date? Implements ILogicalTimestamp.CreationTimestamp
    End Class

    Public Enum EnumDataType
        TypeString = 0
        TypeBoolean = 1
        TypeInteger = 2
        TypeDouble = 3
        TypeStringFromList = 4
    End Enum


    Public Enum SettingCategory
        SettCatUi = 0
        SettCatLogic = 1
        SettCatSecurity = 2
        SettCatFocus = 3
        SettCatDefaultValues = 4


        SettCatPlugins = 99
    End Enum
End Namespace