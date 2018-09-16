Imports System.ComponentModel.DataAnnotations

Public Class ModelValidator

    Public Shared Function ValidateEntity(Of T As ModelBase)(entity As T) As IEnumerable(Of ValidationResult)
        Return New ModelValidation(Of T)().Validate(entity)
    End Function


End Class