using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FindLongFilePaths
{
	public class Program
	{
		/// <summary>
		/// Regex for only including paths that don't have specific strings in them
		/// </summary>
		private const string FILE_PATH_REGEX = @"^((?!\\[Oo]bj\\|\\[Bb]in\\|\\[Dd]ebug\\|\\[Bb]uild\\|\\[Tt]humbs.db\\|\.vs\\).)*$";

		private const int DEFAULT_FILE_LENGTH_TO_OUTPUT = 200;

		private static void Main(string[] args)
		{
			string pathToSearch = null;
			int fileLengthToOutput = -1;
			if (args.Length > 0)
			{
				if (Directory.Exists(args[0]))
					pathToSearch = args[0];
				else
					Write($"*** Passed in path ({args[0]}) does not exist (or you don't have access). Using default instead.");

				if (args.Length > 1)
					int.TryParse(args[1], out fileLengthToOutput);
			}
			else
			{
				Write("Usage: FindLongFilePaths.exe [<root-file-path>] [<max-file-path-length>]");
			}

			// If no path to search was passed-in, use the default (current directory or base directory outside of the FindLongFilePaths application)
			if (string.IsNullOrEmpty(pathToSearch))
			{
				pathToSearch = Directory.GetCurrentDirectory().Split(new[] { "FindLongFilePaths" }, StringSplitOptions.RemoveEmptyEntries)[0];
			}

			if (fileLengthToOutput < 0)
			{
				string maxFilePathLengthFromConfig = ConfigurationManager.AppSettings["MaxFilePathLength"];
				if (string.IsNullOrEmpty(maxFilePathLengthFromConfig) || !int.TryParse(maxFilePathLengthFromConfig, out fileLengthToOutput))
					fileLengthToOutput = DEFAULT_FILE_LENGTH_TO_OUTPUT;

				Write($"*** No max file length was passed in (or the value passed in was invalid). Using default instead ({fileLengthToOutput}).");
			}

			Write($"*** Searching in path {pathToSearch}");
			IEnumerable<string> filePaths = Directory.EnumerateFiles(pathToSearch, "*.*", SearchOption.AllDirectories);
			foreach (string filePath in filePaths.Where(fp => Regex.IsMatch(fp, FILE_PATH_REGEX) && fp.Length >= fileLengthToOutput))
			{
				Write($"{filePath.Length}: {filePath}");
			}
		}

		private static void Write(string message)
		{
			Console.WriteLine(message);
			Debug.WriteLine(message);
		}
	}
}
