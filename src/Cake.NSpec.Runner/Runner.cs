using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mono.Options;
using NSpec.Domain;
using NSpec.Domain.Formatters;

namespace Cake.NSpec
{
	public class Runner
	{
		static void Main(string[] args)
		{

			var shouldShowHelp = false;

			var useXUnitFormatter = false;

			var options = new OptionSet
			{
				{"h|help", "show this message and exit", h => shouldShowHelp = h != null},
				{"f|formatter", "console (default) | xunit",
						f => useXUnitFormatter = string.Compare(f, "xunit", StringComparison.InvariantCultureIgnoreCase) == 0
				}
			};

			if (shouldShowHelp)
			{
				Console.WriteLine("Usage: [OPTIONS] path/to/test.assembly.dll path/to/test.2.assembly.dll");
				Console.WriteLine();
				Console.WriteLine("Options:");
				options.WriteOptionDescriptions(Console.Out);
			}

			IFormatter formatter = null;
			if (useXUnitFormatter)
			{
				formatter = new XUnitFormatter();
			}
			else
			{
				formatter = new ConsoleFormatter();
			}

			IEnumerable<FileInfo> extra;
			try
			{
				extra = options.Parse(args).Select(n => new FileInfo(n)).Where(n => n.Exists);

				foreach (var asm in extra)
				{
					var invocation = new RunnerInvocation(asm.FullName, null, formatter, false);

					var result = invocation.Run();
					var failes = result.Failures().ToArray();

					if (failes.Any())
					{
						throw new Exception($"NSpec run of {asm} reported Failures");
					}
				}

			}
			catch (OptionException e)
			{
				Console.WriteLine(e.Message);
				Console.WriteLine("Try `--help' for more information.");
				return;
			}

		}
	}
}