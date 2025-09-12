using System.Collections.Specialized;
using Godot;
using ObservableCollections;
using R3;
using SharpIDE.Application.Features.SolutionDiscovery.VsPersistence;

namespace SharpIDE.Godot.Features.Problems;

public partial class ProblemsPanel : Control
{
    public SharpIdeSolutionModel? Solution { get; set; }
    
	private Tree _tree = null!;
    private TreeItem _rootItem = null!;
    // TODO: Use observable collections in the solution model and downwards
    private readonly ObservableHashSet<SharpIdeProjectModel> _projects = [];

    public override void _Ready()
    {
        _tree = GetNode<Tree>("ScrollContainer/Tree");
        _rootItem = _tree.CreateItem();
        _rootItem.SetText(0, "Problems");
        Observable.EveryValueChanged(this, manager => manager.Solution)
            .Where(s => s is not null)
            .Subscribe(s =>
            {
                GD.Print($"ProblemsPanel: Solution changed to {s?.Name ?? "null"}");
                _projects.Clear();
                _projects.AddRange(s!.AllProjects);
            });
        BindToTree(_projects);
    }
    
    public void BindToTree(ObservableHashSet<SharpIdeProjectModel> list)
    {
        var view = list.CreateView(x =>
        {
            var treeItem = _tree.CreateItem(_rootItem);
            treeItem.SetText(0, x.Name);
            Observable.EveryValueChanged(x, s => s.Diagnostics.Count)
                .Subscribe(s => treeItem.Visible = s is not 0);
            return treeItem;
        });
        view.ViewChanged += OnViewChanged;
    }
    private static void OnViewChanged(in SynchronizedViewChangedEventArgs<SharpIdeProjectModel, TreeItem> eventArgs)
    {
        GD.Print("View changed: " + eventArgs.Action);
        if (eventArgs.Action == NotifyCollectionChangedAction.Remove)
        {
            eventArgs.OldItem.View.Free();
        }
    }
}