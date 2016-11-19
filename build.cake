
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var sln = "./src/iron.apps.Cake.NSpec.sln";

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

Task("Restore-NuGet-Packages").Does(() => NuGetRestore(sln));
Task("Clean").Does(() => CleanDirectories("./src/**/bin/" + configuration));
Task("Default").IsDependentOn("Build");

RunTarget(target);