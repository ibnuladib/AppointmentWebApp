# AppointmentWebApp
In appsettings.JSON
 "ApplicationDbContextConnection": "Server=(localdb)\\express;Database=AppointmentWebApp;Trusted_Connection=True;MultipleActiveResultSets=true"
 change sql string manually, add trustedservercertificatin if needed.

Use Command in npm console:
  Update-Database (to get the sql server up to date with the migration files)
  Add-Migration "<Comment>" (to add new migration)
