Class Application
    Private Sub Application_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup
        Using context As New DbContext.StorageContext()
            context.Database.EnsureCreated()
        End Using
    End Sub

    ' Ereignisse auf Anwendungsebene wie Startup, Exit und DispatcherUnhandledException
    ' können in dieser Datei verarbeitet werden.

End Class
