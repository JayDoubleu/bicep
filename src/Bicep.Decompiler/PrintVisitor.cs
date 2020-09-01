// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections.Generic;
using System.Text;
using Bicep.Core.Parser;
using Bicep.Core.Syntax;

namespace Bicep.Decompiler
{
    public class PrintVisitor : SyntaxVisitor
    {
        private int depth = 0;
        private readonly StringBuilder buffer;

        private void PrintSpace()
        {
            buffer.Append(" ");
        }

        private void PrintIndent()
        {
            for (var i = 0; i < depth; i++)
            {
                buffer.Append("  ");
            }
        }

        public static string Print(ProgramSyntax syntax)
        {
            var buffer = new StringBuilder();
            var visitor = new PrintVisitor(buffer);
            visitor.Visit(syntax);

            return buffer.ToString();
        }

        private PrintVisitor(StringBuilder buffer)
        {
            this.buffer = buffer;
        }

        protected override void VisitTokenInternal(Token token)
        {
            buffer.Append(token.Text);
        }

        public override void VisitFunctionArgumentSyntax(FunctionArgumentSyntax syntax)
        {
            Visit(syntax.Expression);
            VisitToken(syntax.Comma);
            if (syntax.Comma != null)
            {
                PrintSpace();
            }
        }

        public override void VisitObjectPropertySyntax(ObjectPropertySyntax syntax)
        {
            PrintIndent();
            Visit(syntax.Key);
            VisitToken(syntax.Colon);
            PrintSpace();
            Visit(syntax.Value);
            VisitTokens(syntax.NewLines);
        }

        public override void VisitObjectSyntax(ObjectSyntax syntax)
        {
            VisitToken(syntax.OpenBrace);
            VisitTokens(syntax.NewLines);

            depth++;
            foreach (var item in syntax.Properties)
            {
                Visit(item);
            }
            depth--;

            PrintIndent();
            VisitToken(syntax.CloseBrace);
        }

        public override void VisitArrayItemSyntax(ArrayItemSyntax syntax)
        {
            PrintIndent();
            Visit(syntax.Value);
            VisitTokens(syntax.NewLines);
        }

        public override void VisitArraySyntax(ArraySyntax syntax)
        {
            VisitToken(syntax.OpenBracket);
            VisitTokens(syntax.NewLines);

            depth++;
            foreach (var item in syntax.Items)
            {
                Visit(item);
            }
            depth--;

            PrintIndent();
            VisitToken(syntax.CloseBracket);
        }

        public override void VisitParameterDeclarationSyntax(ParameterDeclarationSyntax syntax)
        {
            VisitToken(syntax.ParameterKeyword);
            PrintSpace();
            Visit(syntax.Name);
            PrintSpace();
            Visit(syntax.Type);
            PrintSpace();
            Visit(syntax.Modifier);
            VisitToken(syntax.NewLine);
        }

        public override void VisitParameterDefaultValueSyntax(ParameterDefaultValueSyntax syntax)
        {
            VisitToken(syntax.AssignmentToken);
            PrintSpace();
            Visit(syntax.DefaultValue);
        }

        public override void VisitVariableDeclarationSyntax(VariableDeclarationSyntax syntax)
        {
            VisitToken(syntax.VariableKeyword);
            PrintSpace();
            Visit(syntax.Name);
            PrintSpace();
            VisitToken(syntax.Assignment);
            PrintSpace();
            Visit(syntax.Value);
            VisitToken(syntax.NewLine);
        }

        public override void VisitResourceDeclarationSyntax(ResourceDeclarationSyntax syntax)
        {
            VisitToken(syntax.ResourceKeyword);
            PrintSpace();
            Visit(syntax.Name);
            PrintSpace();
            Visit(syntax.Type);
            PrintSpace();
            VisitToken(syntax.Assignment);
            PrintSpace();
            Visit(syntax.Body);
            VisitToken(syntax.NewLine);
        }

        public override void VisitOutputDeclarationSyntax(OutputDeclarationSyntax syntax)
        {
            VisitToken(syntax.OutputKeyword);
            PrintSpace();
            Visit(syntax.Name);
            PrintSpace();
            Visit(syntax.Type);
            PrintSpace();
            VisitToken(syntax.Assignment);
            PrintSpace();
            Visit(syntax.Value);
            VisitToken(syntax.NewLine);
        }
    }
}
