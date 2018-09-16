

Namespace Interfaces
    Public Interface ILogicalDelete

        ''' <summary>
        ''' Gibt zurück ob die Entität als gelöscht markiert ist oder setzt den Wert!!
        ''' </summary>
        Property DeletedFlag As Boolean

        Property DeletedTimestamp As DateTime?

    End Interface
End Namespace