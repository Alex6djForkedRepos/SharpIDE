using Godot;

namespace SharpIDE.Godot.Features.ToolPanes;

public partial class ToolPaneContainer : PanelContainer
{
	[Export]
	public ToolPaneLocation Location { get; set; }
}
