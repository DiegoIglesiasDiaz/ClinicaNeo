Application to manage appointments.

Requirements:
  Docker
  .Net 8

Installation Steps:
1. Clone project.
2. Run Apphost project
3. Enjoy :)

For migrations in root folder:

1. dotnet ef migrations add InitialCreate --project Infrastructure --startup-project WebAPI
2. dotnet ef database update  InitialCreate --project Infrastructure --startup-project WebAPI
