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

			var formatter = new ConsoleFormatter ();
			var assemblies = context.Globber.GetFiles (pattern).ToArray ();

			if (assemblies.Length == 0) {
				context.Log.Warning ($"nspec: The provided pattern did not match any files. ({pattern})");
				return;
			} else {
				foreach (var asm in assemblies) {
					context.Log.Information ($"nspec: {asm.GetFilename ()}");
					context.Log.Verbose ($"nspec: {asm}");
				}
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


	}
}