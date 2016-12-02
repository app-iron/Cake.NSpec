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

namespace Cake.NSpec
{
	[CakeAliasCategory("Testing")]
	[CakeAliasCategory("BDD")]
	[CakeAliasCategory("NSpec")]
	public static class NSpecBinder
	{
		[CakeMethodAlias]
		public static void NSpec(this ICakeContext context, string pattern)
		{
			if (context == null)
			{
				throw new ArgumentNullException(nameof(context));
			}

			var assemblies = context.Globber.GetFiles(pattern).ToArray();
			var isOnMono = Type.GetType("Mono.Runtime") != null;

			var cakeNSpec = System.IO.Path.GetDirectoryName(
				new System.Uri(typeof(NSpecBinder).Assembly.CodeBase).LocalPath
			);

			if (assemblies.Length == 0)
			{
				context.Log.Warning($"nspec: The provided pattern did not match any files. ({pattern})");
				return;
			}
			else
			{
				var proc = typeof(Runner).Assembly.GetName().Name + ".exe";

				var procArgs = "";

				if (isOnMono)
				{
					procArgs = proc + " ";
					proc = "mono";
				}

				foreach (var asm in assemblies)
				{
					var fileName = asm.GetFilename();

					context.Log.Information($"nspec: Found Assembly {fileName}");
					context.Log.Information(proc);

					var localProcArgs = procArgs + fileName.FullPath + " -f console";

					context.Log.Verbose($"nspec: Starting {proc} {localProcArgs}");

					var proccess = context.ProcessRunner.Start(proc, new ProcessSettings
					{
						Arguments = ProcessArgumentBuilder.FromString(localProcArgs),
						WorkingDirectory = cakeNSpec
					});

					proccess.WaitForExit(30000);
					var errorCode = proccess.GetExitCode();
					if (errorCode != 0)
					{
						throw new CakeException($"Runner {fileName} exited with {errorCode}");
					}
					else
					{
						context.Log.Information($"{fileName} passed.");
					}

				}
			}
		}
	}
}