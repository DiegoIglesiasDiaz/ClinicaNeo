Requirements:
1. Docker

Installation Steps:
1. Clone project.
2. Run just WebApi project
3. Enjoy :)

For migrations:

1. dotnet ef migrations add InitialCreate --project Infrastructure --startup-project WebAPI
2. dotnet ef database update  InitialCreate --project Infrastructure --startup-project WebAPI