using NuGet.Configuration;
using SharpIDE.Application.Features.SolutionDiscovery.VsPersistence;

namespace SharpIDE.Application.Features.Nuget;

public class NugetClientService
{
	public async Task Test(string directoryPath)
	{
		var settings = Settings.LoadDefaultSettings(root: directoryPath);
		var packageSourceProvider = new PackageSourceProvider(settings);
		var packageSources = packageSourceProvider.LoadPackageSources().Where(p => p.IsEnabled).ToList();

		// Get top 100 packages across all sources, ordered by download count

	}
}
