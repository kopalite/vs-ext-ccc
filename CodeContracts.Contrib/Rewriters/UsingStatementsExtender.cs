﻿using CodeContracts.Contrib.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace CodeContracts.Contrib.Rewriters
{
    internal class UsingStatementsExtender : CSharpSyntaxRewriter
    {
        private string _qualifiedName;

        public UsingStatementsExtender(string qualifiedName)
        {
            _qualifiedName = qualifiedName;
        }

        public override SyntaxNode VisitCompilationUnit(CompilationUnitSyntax node)
        {
            //Adding using statement for given qualified name.

            if (node.Usings.Any(u => u.Name.Str() == _qualifiedName))
            {
                return node;
            }

            var contractsUsingNode = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(_qualifiedName));
            return node.AddUsings(contractsUsingNode);
        }
    }
}
