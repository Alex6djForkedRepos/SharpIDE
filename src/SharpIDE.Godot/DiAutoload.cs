using Godot;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SharpIDE.Application.Features.Build;
using SharpIDE.Application.Features.FilePersistence;
using SharpIDE.Application.Features.FileWatching;
using SharpIDE.Application.Features.Run;

namespace SharpIDE.Godot;

[AttributeUsage(AttributeTargets.Field)]
public class InjectAttribute : Attribute;

public partial class DiAutoload : Node
{
    private ServiceProvider? _serviceProvider;

    public override void _EnterTree()
    {
        GD.Print("[Injector] _EnterTree called");
        var services = new ServiceCollection();
        // Register services here
        services.AddSingleton<BuildService>();
        services.AddSingleton<RunService>();
        services.AddSingleton<IdeFileExternalChangeHandler>();
        services.AddSingleton<IdeFileSavedToDiskHandler>();
        services.AddSingleton<IdeFileWatcher>();
        services.AddSingleton<IdeOpenTabsFileManager>();

        _serviceProvider = services.BuildServiceProvider();
        GetTree().NodeAdded += OnNodeAdded;
        GD.Print("[Injector] Service provider built and NodeAdded event subscribed");
    }

    public override void _Ready()
    {
        
    }

    private void OnNodeAdded(Node node)
    {
        // Inject dependencies into every new node
        InjectDependencies(node);
    }

    private void InjectDependencies(object target)
    {
        var type = target.GetType();
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        foreach (var field in type.GetFields(flags))
        {
            if (Attribute.IsDefined(field, typeof(InjectAttribute)))
            {
                var service = _serviceProvider!.GetService(field.FieldType);
                if (service is null)
                {
                    GD.PrintErr($"[Injector] No service registered for {field.FieldType}");
                    GetTree().Quit();
                }

                field.SetValue(target, service);
            }
        }
    }
}