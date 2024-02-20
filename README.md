1 - make sure that .net SDK 8.0.101 is installed and if not install it from (https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-8.0.101-windows-x64-installer)
2 - make sure that ASP.NET Core 8.0 Runtime 8.0.2 is installed and if not install it from (https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-aspnetcore-8.0.2-windows-hosting-bundle-installer)
3 - run (dotnet tool update --global dotnet-ef) in the terminal to make sure that the Global Tool is updated
4 - Ensure that the required runtime version '8.0.1' is present in the installed runtimes by Running the following command to list installed runtimes (dotnet --list-runtimes) and Verify that '8.0.1' is listed
5 - run (dotnet ef database update --context AppDbContext) in the terminal
6 - the migration adds two users 
  user: admin@admin.com  pass: 123
  user: user@user.com    pass: 123
