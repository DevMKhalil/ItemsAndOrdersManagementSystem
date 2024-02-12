1 - make sure that .net SDK 8.0.101 is instaled and if not instal it from (https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-8.0.101-windows-x64-installer)
2 - run (dotnet tool update --global dotnet-ef) in terminal to make sure that the Global Tool is updated
3 - Ensure that the required runtime version '8.0.1' is present in the installed runtimes by Run the following command to list installed runtimes (dotnet --list-runtimes) and Verify that '8.0.1' is listed
4 - run (dotnet ef migrations add InitialCreate)
5 - run (dotnet ef database update)