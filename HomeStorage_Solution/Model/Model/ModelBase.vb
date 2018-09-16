Imports System.ComponentModel.DataAnnotations

Public MustInherit Class ModelBase

    ''' <summary>
    ''' Setzt von wem der Datensatz erstellt wurde (z.b. Username) oder gibt diesen zurück
    ''' </summary>
    Public Overridable Property CreatedBy As String = Environment.UserName

    ''' <summary>
    ''' Setzt auf welcher Maschine der Datensatz erstellt wurde oder gibt diesen zurück
    ''' </summary>
    Public Overridable Property CreatedOn As String = Environment.MachineName

    ''' <summary>
    ''' Setzt durch wem (z.b. Username) der Datensatz zuletzt geändert wurde oder gibt ihn zurück
    ''' </summary>
    Public Overridable Property LastUpdateBy As String = Environment.UserName

    ''' <summary>
    ''' Setzt auf welcher Maschine der Datensatz zuletzt geändert wurde oder gibt diese zurück
    ''' </summary>
    Public Overridable Property LastUpdateOn As String = Environment.MachineName


    ''' <summary>
    ''' Gibt alle Validierungsfehler der aktuellen Entität zurück.
    ''' </summary>
    Public Overridable Function Validate() As IEnumerable(Of ValidationResult)
        Return ModelValidator.ValidateEntity(Me)
    End Function


    ''' <summary>
    ''' Gibt den Validierungsfehler einer bestimmten Eigenschaft zurück.
    ''' </summary>
    ''' <param name="propertyName">Die Eigenschaft für welche der Validierungsfehler zurückgegeben werden soll</param>
    ''' <returns>Gibt die Errormessage zurück wenn die Eigenschaft einen Validierungsfehler aufweist oder Nothing sollte die Eigenschaft keinen Validierungsfehler aufweisen</returns>
    Public Function GetPropertyValidationError(propertyName As String) As String
        If Validate.Where(Function(v) v.MemberNames.Contains(propertyName)).Any Then
            Return Validate.Where(Function(v) v.MemberNames.Contains(propertyName)).First.ErrorMessage
        Else
            Return Nothing
        End If
    End Function


    ''' <summary>
    ''' Schnellabfrage ob die Entität keine Validierungsfehler aufweist
    ''' </summary>
    Public Function IsValid() As Boolean
        Return Validate.Count = 0
    End Function

    ''' <summary>
    ''' Schnellabfrage für eine einzelne Eigenschaft ob diese keine Validierungsfehler aufweist
    ''' </summary>
    ''' <param name="propertyName">Eigenschaftenname welcher abgefragt werden soll</param>
    Public Function IsValid(propertyName As String) As Boolean
        If propertyName Is Nothing Then Throw New ArgumentNullException(NameOf(propertyName))
        If IsValid() Then Return True
        Return Not Validate.Where(Function(v) v.MemberNames.Contains(propertyName)).Any
    End Function




End Class
