#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var sln = "./src/Cake.NSpec.sln";

Setup((ctx) => Information("Running tasks..."));
Teardown( (ctx) => Information("Finished running tasks."));

Task("Build")
	.IsDependentOn("Restore-NuGet-Packages")
	.IsDependentOn("Clean")
	.Does(() =>
{

	DotNetBuild(sln, settings =>
		settings.SetConfiguration(configuration)
		.SetVerbosity(Cake.Core.Diagnostics.Verbosity.Minimal)
		.WithTarget("Build")
		.WithProperty("TreatWarningsAsErrors","true")
	);

});


Task("NUnit3").IsDependentOn("Build").Does(() =>
	NUnit3("./src/Cake.NSpec.nspec/bin/" + configuration + "/Cake.NSpec.nspec.dll", new NUnit3Settings {
		NoResults = true
}));

Task("Restore-NuGet-Packages").Does(() => NuGetRestore(sln));

Task("Clean").Does(() => CleanDirectories("./src/**/bin"));

Task("Default").IsDependentOn("NUnit3");

RunTarget(target);
