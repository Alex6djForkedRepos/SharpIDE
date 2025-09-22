using Microsoft.CodeAnalysis.Razor.SemanticTokens;

namespace SharpIDE.Application.Features.Analysis;

public class TranslatedSemanticRange
{
	public required SemanticRange Range { get; set; }
	public required string Kind { get; set; }
}
