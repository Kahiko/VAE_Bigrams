"# VAE_Bigrams" 

Created					First working build
10/30/2024  12:54 PM	10/30/2024  01:53 PM

1.) Create a dotnet console project
	dotnet new sln -o bigram-parsing
	cd bigram-parsing
	dotnet new console -o bigram-parsing
	ren .\bigram-parsing\Class1.cs PrimeService.cs
	dotnet sln add ./bigram-parsing/bigram-parsing.csproj
2.) Create a test project
	dotnet new xunit -o bigram-parsing.Tests
	dotnet add ./bigram-parsing.Tests/bigram-parsing.Tests.csproj reference ./bigram-parsing/bigram-parsing.csproj
	dotnet sln add ./bigram-parsing.Tests/bigram-parsing.Tests.csproj
