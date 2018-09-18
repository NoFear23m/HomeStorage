Imports System.ComponentModel.DataAnnotations

Public Class ModelValidation(Of T As ModelBase)


    Public Function Validate(entity As T) As IEnumerable(Of ValidationResult)
        Dim validationResults As List(Of ValidationResult) = New List(Of ValidationResult)()
        Dim validationContext As ValidationContext = New ValidationContext(entity, Nothing, Nothing)
        Try
            Validator.TryValidateObject(entity, validationContext, validationResults, True)
        Catch ex As Exception
            Debug.WriteLine(String.Format("FEHLER Validate: {0}", ex.ToString))
        End Try

        Return validationResults
    End Function
End Class