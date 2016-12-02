using System;
using System.Linq;
using NSpec;

namespace ExampleTwo
{
	public class describe_TestAssemblyTwo : nspec
	{


		void it_should_not_have_The_Example_Assembly_loaded()
		{
			var exampleAssembliesCount = AppDomain.CurrentDomain.GetAssemblies()
					.Count(n => n.GetName().Name.Contains("Example"));

			exampleAssembliesCount.should_be(1);
		}

	}
}