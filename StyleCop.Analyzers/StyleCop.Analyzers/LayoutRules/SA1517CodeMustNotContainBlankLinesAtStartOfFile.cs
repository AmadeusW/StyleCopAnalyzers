﻿namespace StyleCop.Analyzers.LayoutRules
{
    using System;
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.CodeAnalysis.CSharp;


    /// <summary>
    /// The code file has blank lines at the start.
    /// </summary>
    /// <remarks>
    /// <para>To improve the layout of the code, StyleCop requires no blank lines at the start of files.</para>
    ///
    /// <para>A violation of this rule occurs when one or more blank lines are at the start of the file.</para>
    /// </remarks>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SA1517CodeMustNotContainBlankLinesAtStartOfFile : DiagnosticAnalyzer
    {
        /// <summary>
        /// The ID for diagnostics produced by the <see cref="SA1517CodeMustNotContainBlankLinesAtStartOfFile"/>
        /// analyzer.
        /// </summary>
        public const string DiagnosticId = "SA1517";
        private const string Title = "Code must not contain blank lines at start of file";
        private const string MessageFormat = "Code must not contain blank lines at start of file";
        private const string Category = "StyleCop.CSharp.LayoutRules";
        private const string Description = "The code file has blank lines at the start.";
        private const string HelpLink = "http://www.stylecop.com/docs/SA1517.html";

        private static readonly DiagnosticDescriptor Descriptor =
            new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, AnalyzerConstants.DisabledNoTests, Description, HelpLink);

        private static readonly ImmutableArray<DiagnosticDescriptor> SupportedDiagnosticsValue =
            ImmutableArray.Create(Descriptor);

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get
            {
                return SupportedDiagnosticsValue;
            }
        }

        /// <inheritdoc/>
        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxTreeAction(this.HandleSyntaxTreeAnalysis);
        }

        private void HandleSyntaxTreeAnalysis(SyntaxTreeAnalysisContext context)
        {
            var firstToken = context.Tree.GetRoot().GetFirstToken(includeZeroWidth: true);

            if (firstToken.HasLeadingTrivia)
            {
                foreach (var trivia in firstToken.LeadingTrivia)
                {
                    if (trivia.IsKind(SyntaxKind.EndOfLineTrivia))
                    {
                        context.ReportDiagnostic(Diagnostic.Create(Descriptor, Location.Create(context.Tree, trivia.FullSpan)));
                        break;
                    }
                }
            }
        }
    }
}
