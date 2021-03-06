﻿namespace StyleCop.Analyzers.LayoutRules
{
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
    /// An element documentation header above a C# element is followed by a blank line.
    /// </summary>
    /// <remarks>
    /// <para>To improve the readability of the code, StyleCop requires blank lines in certain situations, and prohibits
    /// blank lines in other situations. This results in a consistent visual pattern across the code, which can improve
    /// recognition and readability of unfamiliar code.</para>
    ///
    /// <para>A violation of this rule occurs when the element documentation header above an element is followed by a
    /// blank line. For example:</para>
    ///
    /// <code language="csharp">
    /// /// <summary>
    /// /// Gets a value indicating whether the control is enabled.
    /// /// </summary>
    ///
    /// public bool Enabled
    /// {
    ///     get { return this.enabled; }
    /// }
    /// </code>
    ///
    /// <para>The code above would generate an instance of this violation, since the documentation header is followed by
    /// a blank line.</para>
    /// </remarks>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SA1506ElementDocumentationHeadersMustNotBeFollowedByBlankLine : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "SA1506";
        internal const string Title = "Element documentation headers must not be followed by blank line";
        internal const string MessageFormat = "TODO: Message format";
        internal const string Category = "StyleCop.CSharp.LayoutRules";
        internal const string Description = "An element documentation header above a C# element is followed by a blank line.";
        internal const string HelpLink = "http://www.stylecop.com/docs/SA1506.html";

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
            // TODO: Implement analysis
        }
    }
}
