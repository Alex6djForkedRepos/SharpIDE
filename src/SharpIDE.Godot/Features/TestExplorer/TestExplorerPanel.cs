using Godot;
using SharpIDE.Application.Features.Testing;

namespace SharpIDE.Godot.Features.TestExplorer;

public partial class TestExplorerPanel : Control
{
    [Inject] private readonly SharpIdeSolutionAccessor _solutionAccessor = null!;
    [Inject] private readonly TestRunnerService _testRunnerService = null!;
    
    private readonly PackedScene _testNodeEntryScene = ResourceLoader.Load<PackedScene>("uid://dt50f2of66dlt");

    private VBoxContainer _testNodesVBoxContainer = null!;

    public override void _Ready()
    {
        _testNodesVBoxContainer = GetNode<VBoxContainer>("%TestNodesVBoxContainer");
        _ = Task.GodotRun(AsyncReady);
    }

    private async Task AsyncReady()
    {
        await _solutionAccessor.SolutionReadyTcs.Task;
        var solution = _solutionAccessor.SolutionModel!;
        var testNodes = await _testRunnerService.DiscoverTests(solution);
        testNodes.ForEach(s => GD.Print(s.DisplayName));
        var scenes = testNodes.Select(s =>
        {
            var entry = _testNodeEntryScene.Instantiate<TestNodeEntry>();
            entry.TestNode = s;
            return entry;
        });
        await this.InvokeAsync(() =>
        {
            _testNodesVBoxContainer.QueueFreeChildren();
            foreach (var scene in scenes)
            {
                _testNodesVBoxContainer.AddChild(scene);
            }
        });
    }
}