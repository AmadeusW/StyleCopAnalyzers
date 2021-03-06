﻿namespace StyleCop.Analyzers.MaintainabilityRules
{
    using System.Collections.Immutable;
    using System.Composition;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeActions;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using StyleCop.Analyzers.SpacingRules;

    /// <summary>
    /// Implements a code fix for <see cref="SA1407ArithmeticExpressionsMustDeclarePrecedence"/>.
    /// </summary>
    /// <remarks>
    /// <para>To fix a violation of this rule, insert parenthesis within the arithmetic expression to declare the precedence of the operations.</para>
    /// </remarks>
    [ExportCodeFixProvider(nameof(SA1407SA1408CodeFixProvider), LanguageNames.CSharp)]
    [Shared]
    public class SA1407SA1408CodeFixProvider : CodeFixProvider
    {
        private static readonly ImmutableArray<string> _fixableDiagnostics =
            ImmutableArray.Create(SA1407ArithmeticExpressionsMustDeclarePrecedence.DiagnosticId, SA1408ConditionalExpressionsMustDeclarePrecedence.DiagnosticId);

        /// <inheritdoc/>
        public override ImmutableArray<string> GetFixableDiagnosticIds()
        {
            return _fixableDiagnostics;
        }

        /// <inheritdoc/>
        public override FixAllProvider GetFixAllProvider()
        {
            // In some cases the WellKnownFixAllProviders.BatchFixer screws up and 'merges' some parenthesis together
            // int a = 5 + y * b / 6 % z - 2;
            // gets transformed into
            // int a = 5 + (y * b / 6) % z) - 2;
            // which is invalid C# (a opening parenthesis is missing). So for now FixAll is disabled until
            // either WellKnownFixAllProviders.BatchFixer is fixed and does not remove some parenthesis, or
            // we implement a FixAllProvider that works in this case.
            return null;
        }

        /// <inheritdoc/>
        public override async Task ComputeFixesAsync(CodeFixContext context)
        {
            foreach (var diagnostic in context.Diagnostics)
            {
                if (!GetFixableDiagnosticIds().Contains(diagnostic.Id))
                    continue;

                var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
                SyntaxNode node = root.FindNode(diagnostic.Location.SourceSpan);
                if (node.IsMissing)
                    continue;
                BinaryExpressionSyntax syntax = node as BinaryExpressionSyntax;
                if (syntax != null)
                {
                    var newNode = SyntaxFactory.ParenthesizedExpression(syntax.WithoutTrivia())
                        .WithTriviaFrom(syntax)
                        .WithoutFormatting();

                    var syntaxRoot = await context.Document.GetSyntaxRootAsync(context.CancellationToken);

                    var newSyntaxRoot = syntaxRoot.ReplaceNode(node, newNode);

                    var changedDocument = context.Document.WithSyntaxRoot(newSyntaxRoot);

                    context.RegisterFix(CodeAction.Create("Add parenthesis", changedDocument), diagnostic);
                }
            }
        }
    }
}
