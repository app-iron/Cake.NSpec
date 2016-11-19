# iron.apps.Cake.NSpec
This is a Cakebuild addin for nspec
See [nspec](http://nspec.org/) and [Cake](http://cakebuild.net).

---
## usage Example

```csharp
#addin nuget:?package=iron.apps.Cake.NSpec
...
Task("NSpec").Does(() => {
  NSpec("./src/**/bin/" + configuration + "/*.nspec.dll");
 });

```

## Notice
As of now this is short Weekend Project, therefore the PreRelease Version.
I will continue to release better integration and Feature using [SemVer](http://semver.org/) as i please.


*Feel free to open Issues or PR :)*

