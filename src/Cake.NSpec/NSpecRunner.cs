using System;
using NSpec;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;
using NSpec.Domain;
using System.Linq;
using NSpec.Domain.Formatters;
using System.Collections.Generic;
using System.Reflection;

namespace iron.apps.Cake.NSpec
{
	[CakeAliasCategory ("Testing")]
	[CakeAliasCategory ("BDD")]
	[CakeAliasCategory ("NSpec")]
	public static class NSpecBinder
	{

		[CakeMethodAlias]
		public static void NSpec (this ICakeContext context, string pattern)
		{

			if (context == null) {
				throw new ArgumentNullException (nameof (context));
			}

			var formatter = FindFormatter (null, null);

			var assemblies = context.Globber.GetFiles (pattern).ToArray ();

			if (assemblies.Length == 0) {
				context.Log.Verbose ("The provided pattern did not match any files.");
				return;
			}

			foreach (var asm in assemblies) {
				var invocation = new RunnerInvocation (asm.FullPath, null, formatter, false);

				var result = invocation.Run ();
				var failes = result.Failures ();

				if (failes.Any ()) {
					throw new CakeException ($"NSpec run of {asm} reported {failes.Count ()} Failures");
				}
			}

		}

		private static IFormatter FindFormatter (string formatterClassName, IDictionary<string, string> formatterOptions)
		{
			// Default formatter is the standard console formatter
			if (string.IsNullOrEmpty (formatterClassName)) {
				var consoleFormatter = new ConsoleFormatter ();
				consoleFormatter.Options = formatterOptions;
				return consoleFormatter;
			}

			var nspecAssembly = typeof (IFormatter).GetTypeInfo ().Assembly;

			// Look for a class that implements IFormatter with the provided name
			var formatterType = nspecAssembly.GetTypes ().FirstOrDefault (type =>
				  (type.Name.ToLowerInvariant () == formatterClassName)
				  && typeof (IFormatter).IsAssignableFrom (type));

			if (formatterType != null) {
				var formatter = (IFormatter)Activator.CreateInstance (formatterType);
				formatter.Options = formatterOptions;
				return formatter;
			} else {
				throw new TypeLoadException ("Could not find formatter type " + formatterClassName);

			}
		}
	}
}
