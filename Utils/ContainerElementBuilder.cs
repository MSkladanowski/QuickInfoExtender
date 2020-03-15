using Microsoft.VisualStudio.Text.Adornments;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.Shell;
using System.Windows.Controls;
using System.Windows;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Core.Imaging;
using System;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.VisualStudio.Threading;

namespace QuickInfoUtils
{
    public   class ContainerElementBuilder
    {
        private List<ContainerElement> _containerElementList;
        public ContainerElementBuilder()
        {
            _containerElementList = new List<ContainerElement>();
        }

        public ContainerElement Build()
        {
            return new ContainerElement(ContainerElementStyle.Stacked, _containerElementList);
        }

        public void AddContainer(ISymbol symbolToDisplay)
        {
            List<ClassifiedTextRun> runs = new List<ClassifiedTextRun>();
            foreach (SymbolDisplayPart part in symbolToDisplay.ToDisplayParts(SymbolDisplayFormat.MinimallyQualifiedFormat))
            {
                runs.Add(new ClassifiedTextRun(part.Kind.ToClassificationTypeName(), part.ToString()));
            }
            _containerElementList.Add(new ContainerElement(ContainerElementStyle.Wrapped, CreateImageElement(symbolToDisplay), new ClassifiedTextElement(runs.ToArray())));
        }

        private static ImageElement CreateImageElement(ISymbol symbolToDisplay)
        {
            if (symbolToDisplay is IMethodSymbol)
            {
                if (((IMethodSymbol)symbolToDisplay).IsExtensionMethod)
                {
                    return new ImageElement(new ImageId(KnownMonikers.ExtensionMethod.Guid, KnownMonikers.ExtensionMethod.Id));
                }
                if (((IMethodSymbol)symbolToDisplay).IsSealed)
                {
                    return new ImageElement(new ImageId(KnownMonikers.MethodSealed.Guid, KnownMonikers.MethodSealed.Id));
                }
            }
            return new ImageElement(new ImageId(KnownMonikers.Method.Guid, KnownMonikers.Method.Id));
        }
    }
}
