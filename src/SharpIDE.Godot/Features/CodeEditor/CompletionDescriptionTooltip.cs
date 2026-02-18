using Godot;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.CodeAnalysis.QuickInfo.Presentation;

namespace SharpIDE.Godot.Features.CodeEditor;

public static class CompletionDescriptionTooltip
{
	private static readonly FontVariation MonospaceFont = ResourceLoader.Load<FontVariation>("uid://cctwlwcoycek7");
	public static RichTextLabel WriteToCompletionDescriptionLabel(RichTextLabel label, CompletionDescription completionDescription, EditorThemeColorSet editorThemeColorSet)
	{
		var quickInfoElements = completionDescription.TaggedParts.ToInteractiveTextElements(null);
		label.PushColor(TextEditorDotnetColoursDark.White);
		label.PushFont(MonospaceFont);
		foreach (var quickInfoElement in quickInfoElements)
		{
			WriteQuickInfoElement(label, quickInfoElement, editorThemeColorSet);
		}
		//label.AddNamespace(symbol);
		label.Pop(); // font
		label.Pop(); // color
		return label;
	}

	private static void WriteQuickInfoElement(RichTextLabel label, QuickInfoElement quickInfoElement, EditorThemeColorSet editorThemeColorSet)
	{
		switch (quickInfoElement)
		{
			case QuickInfoClassifiedTextElement classifiedTextElement:
				foreach (var quickInfoClassifiedTextRun in classifiedTextElement.Runs)
				{
					var colour = ClassificationToColorMapper.GetColorForClassification(editorThemeColorSet, quickInfoClassifiedTextRun.ClassificationTypeName);
					label.PushColor(colour);
					label.AddText(quickInfoClassifiedTextRun.Text);
					label.Pop();
				}
				break;
			case QuickInfoContainerElement containerElement:
				foreach (var element in containerElement.Elements)
				{
					WriteQuickInfoElement(label, element, editorThemeColorSet);
					label.Newline();
				}
				break;
			case QuickInfoGlyphElement glyphElement:
				break;
			case QuickInfoOnTheFlyDocsElement onTheFlyDocsElement:
				break;
			default: throw new NotImplementedException();
		}
	}
}