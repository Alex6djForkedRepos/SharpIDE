using SharpIDE.Application.Features.Build;
using SharpIDE.Application.Features.FilePersistence;
using SharpIDE.Application.Features.FileWatching;
using SharpIDE.Application.Features.Run;
using SharpIDE.Godot.Features.IdeSettings;

namespace SharpIDE.Godot;

public static class Singletons
{
    public static AppState AppState { get; set; } = null!;
}