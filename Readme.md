To install HolidayProject
1. Set up database
	- dotnet ef database update 0
	- dotnet ef migrations add InitialCreate
	- dotnet ef database update
2. Run project
	- dotnet build
	- dotnet run
	- after that should see the url like this https://localhost:7225 so to access the api, should add /swagger