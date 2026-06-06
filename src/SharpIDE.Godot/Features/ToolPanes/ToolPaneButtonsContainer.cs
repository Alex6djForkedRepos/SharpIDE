using Godot;

namespace SharpIDE.Godot.Features.ToolPanes;

public partial class ToolPaneButtonsContainer : VBoxContainer
{
	[Export]
	public ToolPaneLocation Location { get; set; }

	[Export]
	public ButtonGroup ButtonGroup { get; set; } = null!;
}
