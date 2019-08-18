using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.CSharp;

namespace QuickInfoUtils
{
    public class OverloadGenerator
    {
        private readonly ITextBuffer _textBuffer;
        private readonly IAsyncQuickInfoSession _session;
        private SnapshotPoint _snapShotPoint;
        private Document _document;
        private SemanticModel _semanticModel;
        public OverloadGenerator(ITextBuffer textBuffer, IAsyncQuickInfoSession asyncQuickInfo)
        {
            _textBuffer = textBuffer;
            _session = asyncQuickInfo;
        }

        private void GetSnapshotPoint()
        {
            _snapShotPoint = _session.GetTriggerPoint(_textBuffer.CurrentSnapshot).GetValueOrDefault();
        }

        private void GetDocument()
        {
            _document = _snapShotPoint.Snapshot.GetOpenDocumentInCurrentContextWithChanges();
        }

        private void GetSemanticModel()
        {
            _semanticModel = _document?.GetSemanticModelAsync().Result;
            if (_semanticModel?.Language != "C#")
            {
                _semanticModel = null;
            }
        }

        private SyntaxNode GetSyntaxNode()
        {
            SyntaxNode methodNode = _document?.GetSyntaxRootAsync().Result.FindToken(_snapShotPoint).Parent;

            var constructorNode = methodNode?.Ancestors().FirstOrDefault();

            if (constructorNode != null && methodNode.IsKind(SyntaxKind.IdentifierName) && constructorNode.IsKind(SyntaxKind.ObjectCreationExpression))
            {
                return constructorNode;
            }
            else
            {
                return methodNode;
            }
        }
        public ISymbol GetSymbol()
        {
            return _semanticModel?.GetSymbolInfo(GetSyntaxNode()).Symbol;
        }

        public List<ISymbol> GetAllOverLoadsForMousePosition()
        {
            GetSnapshotPoint();
            GetDocument();
            GetSemanticModel();
            var syntaxNode = GetSyntaxNode();
            ISymbol symbol = GetSymbol();
            List<ISymbol> overloadMembers = GetOverloads(syntaxNode);

            if (symbol != null)
            {
                overloadMembers.Remove(symbol);
            }

            return overloadMembers;
        }

        private List<ISymbol> GetOverloads(SyntaxNode syntaxNode)
        {
            if (syntaxNode != null)
            {
                return _semanticModel.GetMemberGroup(syntaxNode).ToList();
            }
            else
            {
                return new List<ISymbol>();
            }
        }
    }
}
