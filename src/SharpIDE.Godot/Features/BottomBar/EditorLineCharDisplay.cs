using Godot;
using SharpIDE.Application.Features.Editor;

namespace SharpIDE.Godot.Features.BottomBar;

public partial class EditorLineCharDisplay : HBoxContainer
{
    private Label _label = null!;
    [Inject] private readonly EditorCaretPositionService _editorCaretPositionService = null!;
    
    private (int, int) _currentPositionRendered = (1, 1);

    public override void _Ready()
    {
        _label = GetNode<Label>("Label");
    }

    // Not sure if we should check this every frame, or an event with debouncing?
    public override void _Process(double delta)
    {
        var caretPosition = _editorCaretPositionService.CaretPosition;
        if (caretPosition != _currentPositionRendered)
        {
            _currentPositionRendered = caretPosition;
            _label.Text = $"{_currentPositionRendered.Item1}:{_currentPositionRendered.Item2}";
        }
    }
}