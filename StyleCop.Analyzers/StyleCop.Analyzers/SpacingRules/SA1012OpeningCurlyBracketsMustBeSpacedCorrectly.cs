﻿namespace StyleCop.Analyzers.SpacingRules
{
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
    /// An opening curly bracket within a C# element is not spaced correctly.
    /// </summary>
    /// <remarks>
    /// <para>A violation of this rule occurs when the spacing around an opening curly bracket is not correct.</para>
    ///
    /// <para>An opening curly bracket should always be preceded by a single space, unless it is the first character on
    /// the line, or unless it is preceded by an opening parenthesis, in which case there should be no space between the
    /// parenthesis and the curly bracket.</para>
    ///
    /// <para>An opening curly bracket must always be followed by a single space, unless it is the last character on the
    /// line.</para>
    /// </remarks>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SA1012OpeningCurlyBracketsMustBeSpacedCorrectly : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "SA1012";
        internal const string Title = "Opening curly brackets must be spaced correctly";
        internal const string MessageFormat = "Opening curly bracket must{0} be {1} by a space.";
        internal const string Category = "StyleCop.CSharp.SpacingRules";
        internal const string Description = "An opening curly bracket within a C# element is not spaced correctly.";
        internal const string HelpLink = "http://www.stylecop.com/docs/SA1012.html";

        public static readonly DiagnosticDescriptor Descriptor =
            new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, AnalyzerConstants.DisabledNoTests, Description, HelpLink);

        private static readonly ImmutableArray<DiagnosticDescriptor> _supportedDiagnostics =
            ImmutableArray.Create(Descriptor);

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get
            {
                return _supportedDiagnostics;
            }
        }

        /// <inheritdoc/>
        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxTreeAction(HandleSyntaxTree);
        }

        private void HandleSyntaxTree(SyntaxTreeAnalysisContext context)
        {
            SyntaxNode root = context.Tree.GetCompilationUnitRoot(context.CancellationToken);
            foreach (var token in root.DescendantTokens())
            {
                switch (token.CSharpKind())
                {
                case SyntaxKind.OpenBraceToken:
                    HandleOpenBraceToken(context, token);
                    break;

                default:
                    break;
                }
            }
        }

        private void HandleOpenBraceToken(SyntaxTreeAnalysisContext context, SyntaxToken token)
        {
            if (token.IsMissing)
                return;

            bool precededBySpace;
            bool firstInLine;
            bool allowLeadingSpace;
            bool allowLeadingNoSpace;

            bool followedBySpace;
            bool lastInLine;

            firstInLine = token.HasLeadingTrivia || token.GetLocation()?.GetMappedLineSpan().StartLinePosition.Character == 0;
            if (firstInLine)
            {
                precededBySpace = true;
                allowLeadingSpace = true;
                allowLeadingNoSpace = true;
            }
            else
            {
                SyntaxToken precedingToken = token.GetPreviousToken();
                precededBySpace = precedingToken.HasTrailingTrivia;
                switch (precedingToken.CSharpKind())
                {
                case SyntaxKind.OpenParenToken:
                    allowLeadingNoSpace = true;
                    allowLeadingSpace = false;
                    break;

                default:
                    allowLeadingNoSpace = false;
                    allowLeadingSpace = true;
                    break;
                }
            }

            followedBySpace = token.HasTrailingTrivia;
            lastInLine = followedBySpace && token.TrailingTrivia.Any(SyntaxKind.EndOfLineTrivia);

            if (!firstInLine)
            {
                if (!allowLeadingSpace && precededBySpace)
                {
                    // Opening curly bracket must{ not} be {preceded} by a space.
                    context.ReportDiagnostic(Diagnostic.Create(Descriptor, token.GetLocation(), " not", "preceded"));
                }
                else if (!allowLeadingNoSpace && !precededBySpace)
                {
                    // Opening curly bracket must{} be {preceded} by a space.
                    context.ReportDiagnostic(Diagnostic.Create(Descriptor, token.GetLocation(), string.Empty, "preceded"));
                }
            }

            if (!lastInLine && !followedBySpace)
            {
                // Opening curly bracket must{} be {followed} by a space.
                context.ReportDiagnostic(Diagnostic.Create(Descriptor, token.GetLocation(), string.Empty, "followed"));
            }
        }
    }
}
