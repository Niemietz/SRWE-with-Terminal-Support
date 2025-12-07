using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace SRWE
{
	static class Program
	{
		[DllImport( "kernel32.dll" )]
		private static extern bool AttachConsole(int dwProcessId);
		private const int ATTACH_PARENT_PROCESS = -1;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			string processName = null;
			string profileName = null;

			if (args.Length > 0)
			{
				AttachConsole(ATTACH_PARENT_PROCESS);

				if (args.Length == 1 && (args[0] == "-h" || args[0] == "--help"))
				{
					Console.WriteLine("\n\nUsage: SRWE.exe [arguments]");
					Console.WriteLine("\nArguments:");
					Console.WriteLine("\n-e or --exec\t|\tProccess name. Something like \"Notepad\"");
					Console.WriteLine("-p or --profile\t|\tFile name of a profile configuration XML. Something like \"C:\\my_favorite_game.xml\"");
					return;
				}

				StringBuilder argumentsLog = new StringBuilder();
				for (int i = 0; i <= args.Length - 1; i++)
				{
					string arg = args[i];
					if (i > 0)
					{
						argumentsLog.Append(", ");
					}
					argumentsLog.Append($"{arg}");

					if (arg.StartsWith("-"))
					{
						switch (arg.Substring(1).ToLower())
						{
							case "e":
							case "-exec":
								processName = GetNextArg(args, i, arg);
								break;

							case "p":
							case "-profile":
								profileName = GetNextArg(args, i, arg);
								break;
						}
					}
				}
				Debug.WriteLine($"Arguments: {argumentsLog}");
			}

			Debug.WriteLine($"Process Name: {processName}, Profile Name: {profileName}");
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm(processName, profileName));
		}

		private static string GetNextArg(string[] args, int currentIndex, string currentArgument)
		{
			if (args.Length == 0) return null;

			if (currentIndex < args.Length - 1)
			{
				var nextArg = args[currentIndex + 1];
				if (nextArg.StartsWith("-"))
				{
					Console.WriteLine($"\nThe value of argument {currentArgument} is invalid");
					return null;
				}

				if (!nextArg.StartsWith("-"))
				{
					return nextArg;
				}
			}
			
			return null;
		}
	}
}
