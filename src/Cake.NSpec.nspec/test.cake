#r Cake.NSpec.exe

Task("Default").Does(() => {
	NSpec("./*.example.dll");
});

RunTarget("Default");