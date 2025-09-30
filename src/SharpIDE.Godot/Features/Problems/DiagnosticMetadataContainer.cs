using Godot;
using Microsoft.CodeAnalysis;
using SharpIDE.Application.Features.SolutionDiscovery.VsPersistence;

namespace SharpIDE.Godot.Features.Problems;

// public partial class GenericContainer<T>(T item) : RefCounted
// {
//     public T Item { get; } = item;
// }

public partial class DiagnosticMetadataContainer(Diagnostic diagnostic) : RefCounted
{
    public Diagnostic Diagnostic { get; } = diagnostic;
}

public partial class ProjectContainer(SharpIdeProjectModel project) : RefCounted
{
    public SharpIdeProjectModel Project { get; } = project;
}