# Cake.NSpec
This is a Cakebuild addin for nspec
See [nspec](http://nspec.org/) and [Cake](http://cakebuild.net).

---
## usage Example

```csharp
#addin nuget:?package=Cake.NSpec
var configuration = Argument("configuration", "Release");
...
Task("NSpec").Does(() => {
  NSpec("./src/test/**/bin/" + configuration + "/*.dll");
 });

```

## Notice
As of now this is short Weekend Project, therefore the PreRelease Version.
I will continue to release new Features and better tested Versions using [SemVer](http://semver.org/) as i please.


*Feel free to open Issues or PR :)*

