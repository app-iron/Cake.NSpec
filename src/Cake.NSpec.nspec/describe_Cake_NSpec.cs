using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace Cake.NSpec
{
	[TestFixture]
	public class describe_Cake
	{
		[Test]
		public void it_should_build_successfully()
		{
			var isOnMono = Type.GetType("Mono.Runtime") != null;

			var asmDir = System.IO.Path.GetDirectoryName(
				new System.Uri(typeof(describe_Cake).Assembly.CodeBase).LocalPath
			);

			var binCake = Path.Combine(asmDir, "Cake.exe");
			var fileName = isOnMono ? "mono" : binCake;
			var args = Path.Combine(asmDir, "test.cake");

			if (isOnMono)
			{
				args = binCake + " " + args;
			}

			var process = new Process()
			{
				StartInfo = new ProcessStartInfo(fileName, args)
				{
					UseShellExecute = true
					,
					CreateNoWindow = true
					,
					WorkingDirectory = Directory.GetCurrentDirectory()
				}
			};

			process.Start();
			if (process.WaitForExit(30000))
			{
				Assert.AreEqual(0, process.ExitCode);
			}

			Assert.AreEqual(0, process.ExitCode);
		}
	}
}