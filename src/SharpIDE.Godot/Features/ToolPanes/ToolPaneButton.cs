using Godot;
using SharpIDE.Godot.Features.BottomPanel;

namespace SharpIDE.Godot.Features.ToolPanes;

public partial class ToolPaneButton : Button
{
	public ToolPaneType ToolPaneType { get; set; }
}
