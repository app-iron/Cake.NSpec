#r bin/Debug/iron.app.Cake.NSpec.dll

Task("Default").Does(() => {
	NSpec("./bin/Debug/iron.apps.Cake.NSpec.nspec.dll");
});

RunTarget("Default");
