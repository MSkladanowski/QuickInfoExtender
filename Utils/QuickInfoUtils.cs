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

        private async Task GetSemanticModel()
        {
            _semanticModel = await _document?.GetSemanticModelAsync();
            if (_semanticModel?.Language != "C#")
            {
                _semanticModel = null;
            }
        }

        private async Task<SyntaxNode> GetSyntaxNode()
        {
            SyntaxNode syntaxRoot = await _document?.GetSyntaxRootAsync();
            SyntaxNode methodNode = syntaxRoot.FindToken(_snapShotPoint).Parent;

            var constructorNode = methodNode?.Ancestors().FirstOrDefault();

            if (constructorNode != null && methodNode.IsKind(SyntaxKind.IdentifierName) && constructorNode.IsKind(SyntaxKind.ObjectCreationExpression))
            {
                return constructorNode;
            }
            return methodNode;
        }

        public async Task<ISymbol> GetSymbol()
        {
            return _semanticModel?.GetSymbolInfo(await GetSyntaxNode()).Symbol;
        }

        public async Task<List<ISymbol>> GetAllOverLoadsForMousePositionAsync()
        {
            GetSnapshotPoint();
            GetDocument();
            await GetSemanticModel().ConfigureAwait(false);
            var syntaxNode = await GetSyntaxNode().ConfigureAwait(false);
            ISymbol symbol = await GetSymbol().ConfigureAwait(false);
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
            return new List<ISymbol>();
        }
    }
}
