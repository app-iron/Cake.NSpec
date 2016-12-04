using System;
using _ = NSpec;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;
using NSpec.Domain;
using System.Linq;
using NSpec.Domain.Formatters;
using System.Collections.Generic;
using System.Reflection;
using Cake.Common.Diagnostics;

namespace Cake.NSpec
{
	[CakeAliasCategory ("Testing")]
	[CakeAliasCategory ("BDD")]
	[CakeAliasCategory ("NSpec")]
	public static class NSpecBinder
	{

		[CakeMethodAlias]
		public static void NSpec (this ICakeContext context, string pattern)
		{
			NSpec (context, pattern, -1);
		}

		[CakeMethodAlias]
		public static void NSpec (this ICakeContext context, string pattern, int timeout)
		{
			if (context == null) {
				throw new ArgumentNullException (nameof (context));
			}

			var assemblies = context.Globber.GetFiles (pattern).ToArray ();
			var isOnMono = Type.GetType ("Mono.Runtime") != null;

			var cakeNSpec = System.IO.Path.GetDirectoryName (
				new System.Uri (typeof (NSpecBinder).Assembly.CodeBase).LocalPath
			);
			context.Debug ($"nspec: setting working directory {cakeNSpec}");
			if (assemblies.Length == 0) {
				context.Log.Warning ($"nspec: The provided pattern did not match any files. ({pattern})");
				return;
			} else {
				var proc = typeof (Runner).Assembly.GetName ().Name + ".exe";

				var procArgs = "";

				if (isOnMono) {
					procArgs = proc + " ";
					proc = "mono";
				}

				foreach (var asm in assemblies) {
					var fileName = asm.GetFilename ();
					var localProcArgs = procArgs + System.IO.Path.Combine (asm.GetDirectory ().FullPath, fileName.FullPath);
					localProcArgs += " -f console";

					context.Log.Information ($"Testing Spec {fileName.FullPath}");
					context.Log.Debug ($"nspec: Found Assembly {asm.GetDirectory ().FullPath}/{fileName.FullPath}");
					context.Log.Debug ($"nspec: Starting {proc} {localProcArgs}");

					var proccess = context.ProcessRunner.Start (proc, new ProcessSettings {
						Arguments = ProcessArgumentBuilder.FromString (localProcArgs),
						WorkingDirectory = cakeNSpec
					});

					if (timeout > 0) {
						proccess.WaitForExit ((int)timeout);
					} else {
						proccess.WaitForExit ();
					}

					var errorCode = proccess.GetExitCode ();
					if (errorCode != 0) {
						throw new CakeException ($"Runner {fileName} exited with {errorCode}");
					} else {
						context.Log.Debug ($"{fileName} passed.");
					}
				}
			}
		}
	}
}
