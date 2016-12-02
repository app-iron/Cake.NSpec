using System;
using NSpec;

namespace iron.apps.Cake
{
	class my_first_spec : nspec
	{
		void before_each () { name = "NSpec"; }

		void it_works ()
		{
			name.should_be ("NSpec");
		}

		void describe_nesting ()
		{
			before = () => name += " BDD";

			it ["works here"] = () => name.should_be ("NSpec BDD");

			it ["and here"] = () => name.should_be ("NSpec BDD");
		}
		string name;
	}
}
