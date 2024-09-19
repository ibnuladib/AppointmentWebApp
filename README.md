# AppointmentWebApp

Use Command in npm console:
  Update-Database (to get the sql server up to date with the migration files)
  Add-Migration "<Comment>" (to add new migration)

  If needed create a file appsettings.Development.json <br/>
  {
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "ApplicationDbContextConnection": "Server=<ServerName>;Database=<DatabaseName>;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
