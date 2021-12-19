#How to add a solution file and later add new project and project references to it.
Source: https://stackoverflow.com/questions/36343223/create-c-sharp-sln-file-with-visual-studio-code


#Open VS Code terminal and navigate to the directory where you want to create solution folder. Use following commands

dotnet new sln -o MyApiApp

#The -o parameter lets you specify the output directory

#Navigate to solution direction

Cd .\MyApiApp\ 

#Create new projects under root solution folder

dotnet new console -o MyApiApp.ConsoleApp
dotnet new webapi -o MyApiApp.WebApi 
dotnet new classlib -o MyApiApp.Repository 
dotnet new xunit -o MyApiApp.Tests

#Add projects to solution (use tab to navigate path).

dotnet sln MyApiApp.sln add .\MyApiApp.ConsoleApp\MyApiApp.ConsoleApp.csproj .\MyApiApp.WebApi\MyApiApp.WebApi.csproj .\MyApiApp.Repository\MyApiApp.Repository.csproj .\MyApiApp.Tests\MyApiApp.Tests.csproj 

#Add project references

dotnet add .\MyApiApp.WebApi\MyApiApp.WebApi.csproj reference .\MyApiApp.Repository\MyApiApp.Repository.csproj 
dotnet add .\MyApiApp.ConsoleApp\MyApiApp.ConsoleApp.csproj reference .\MyApiApp.Repository\MyApiApp.Repository.csproj 
dotnet add .\MyApiApp.Tests\MyApiApp.Tests.csproj reference .\MyApiApp.WebApi\MyApiApp.WebApi.csproj .\MyApiApp.Repository\MyApiApp.Repository.csproj