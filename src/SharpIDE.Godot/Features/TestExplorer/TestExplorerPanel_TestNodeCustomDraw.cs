using Godot;

namespace SharpIDE.Godot.Features.TestExplorer;

public partial class TestExplorerPanel
{
	private Callable? _testNodeCustomDrawCallable;
	private TextLine _testNodeTextLine = new TextLine(); // Reusing this is based on the assumption that it is called by godot in a single-threaded fashion
	private void TestNodeCustomDraw(TreeItem treeItem, Rect2 rect)
	{
		var hovered = _testNodesTree.GetItemAtPosition(_testNodesTree.GetLocalMousePosition()) == treeItem;
		var isSelected = treeItem.IsSelected(0);

		var testNode = treeItem.SharpIdeTestNode;
		if (testNode is null) return;

		var displayName = testNode.DisplayName;
		var executionState = testNode.ExecutionState;

		// Define padding and spacing
		const float padding = 4.0f;
		const float spacing = 6.0f;

		var currentX = rect.Position.X + padding;
		var currentY = rect.Position.Y;

		// Get font and prepare text
		var font = _testNodesTree.GetThemeFont(ThemeStringNames.Font);
		var fontSize = _testNodesTree.GetThemeFontSize(ThemeStringNames.FontSize);
		var textColor = (isSelected, hovered) switch
		{
			(true, true) => _testNodesTree.GetThemeColor(ThemeStringNames.FontHoveredSelectedColor),
			(true, false) => _testNodesTree.GetThemeColor(ThemeStringNames.FontSelectedColor),
			(false, true) => _testNodesTree.GetThemeColor(ThemeStringNames.FontHoveredColor),
			(false, false) => _testNodesTree.GetThemeColor(ThemeStringNames.FontColor)
		};
		var textYPos = currentY + (rect.Size.Y + fontSize) / 2 - 2;

		// Calculate right-hand status width first
		var statusWidth = font.GetStringSize(executionState, HorizontalAlignment.Left, -1, fontSize).X;
		var rightSideWidth = statusWidth + padding;

		// Draw test name on the left with ellipsis truncation to avoid overlap
		var textLine = _testNodeTextLine;
		textLine.TextOverrunBehavior = TextServer.OverrunBehavior.TrimEllipsis;
		textLine.SetHorizontalAlignment(HorizontalAlignment.Left);
		textLine.AddString(displayName, font, fontSize);

		var maxNameWidth = rect.Size.X - currentX - rightSideWidth - spacing;
		if (maxNameWidth > 0)
		{
			textLine.Width = maxNameWidth;
			textLine.Draw(_testNodesTree.GetCanvasItem(), new Vector2(currentX, textYPos - textLine.GetLineAscent()), textColor);
			textLine.Clear();
		}

		// Draw execution state on the right side with status color
		var statusX = rect.Position.X + rect.Size.X - rightSideWidth;
		var statusColor = GetTextColour(executionState);
		_testNodesTree.DrawString(font, new Vector2(statusX, textYPos), executionState, HorizontalAlignment.Left, -1, fontSize, statusColor);
	}
}
