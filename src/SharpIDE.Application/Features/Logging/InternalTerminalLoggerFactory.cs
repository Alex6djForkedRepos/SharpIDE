using System.Reflection;
using Microsoft.Build.Framework;

namespace SharpIDE.Application.Features.Logging;

public class InternalTerminalLoggerFactory
{
	public static ILogger CreateLogger()
	{
		var type = Type.GetType("Microsoft.Build.Logging.TerminalLogger, Microsoft.Build");

		if (type == null) throw new Exception("TerminalLogger type not found");

		var method = type.GetMethod(
			"CreateTerminalOrConsoleLogger",
			BindingFlags.NonPublic | BindingFlags.Static);

		if (method == null) throw new Exception("CreateTerminalOrConsoleLogger method not found");

		string[]? args = [];
		bool supportsAnsi = true;
		bool outputIsScreen = true;
		uint? originalConsoleMode = 0x0007;

		object? logger = method.Invoke(
			obj: null,
			parameters: [args, supportsAnsi, outputIsScreen, originalConsoleMode]);

		return (ILogger)logger!; // This will be an ILogger (or INodeLogger) instance
	}
}
