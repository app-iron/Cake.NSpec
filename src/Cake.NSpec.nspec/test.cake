#r Cake.NSpec.dll

Task("Default").Does(() => {
	NSpec("./*.example.dll");
});

RunTarget("Default");