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
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.Text.Classification;
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
            return  new ContainerElement(ContainerElementStyle.Stacked, _containerElementList);
        }

        public void AddContainer(ISymbol symbolToDisplay)
        {
            List<ClassifiedTextRun> runs = new List<ClassifiedTextRun>();
            foreach (var part in symbolToDisplay.ToDisplayParts(SymbolDisplayFormat.MinimallyQualifiedFormat))
            {
                runs.Add(new ClassifiedTextRun(part.Kind.ToClassificationTypeName(), part.ToString()));
            }

            ImageElement icon = GetIcon((IMethodSymbol)symbolToDisplay);
            _containerElementList.Add(new ContainerElement(ContainerElementStyle.Wrapped, icon, new ClassifiedTextElement(runs.ToArray())));
        }

        private static ImageElement GetIcon(IMethodSymbol symbolToDisplay)
        {
            switch (symbolToDisplay.DeclaredAccessibility)
            {
                case Accessibility.NotApplicable:
                    return new ImageElement(new ImageId(KnownMonikers.MethodPublic.Guid, KnownMonikers.MethodPublic.Id));
                case Accessibility.Private:
                    return new ImageElement(new ImageId(KnownMonikers.MethodPrivate.Guid, KnownMonikers.MethodPrivate.Id));
                case Accessibility.ProtectedAndInternal:
                    return new ImageElement(new ImageId(KnownMonikers.MethodProtected.Guid, KnownMonikers.MethodProtected.Id));
                case Accessibility.Protected:
                    return new ImageElement(new ImageId(KnownMonikers.MethodProtected.Guid, KnownMonikers.MethodProtected.Id));
                case Accessibility.Internal:
                    return new ImageElement(new ImageId(KnownMonikers.MethodInternal.Guid, KnownMonikers.MethodInternal.Id));
                case Accessibility.ProtectedOrInternal:
                    return new ImageElement(new ImageId(KnownMonikers.MethodInternal.Guid, KnownMonikers.MethodInternal.Id));
                case Accessibility.Public:
                    return new ImageElement(new ImageId(KnownMonikers.MethodPublic.Guid, KnownMonikers.MethodPublic.Id));
            }
            if (symbolToDisplay.IsExtensionMethod)
            {
                return new ImageElement(new ImageId(KnownMonikers.ExtensionMethod.Guid, KnownMonikers.ExtensionMethod.Id));
            }
            return new ImageElement(new ImageId(KnownMonikers.Method.Guid, KnownMonikers.Method.Id));
        }

        public  void AddContainer(string buttonText,RoutedEventHandler buttonClick)
        {
            Button buttonelement = CreateButtonAsync(buttonText, buttonClick).Result;
            _containerElementList.Add(new ContainerElement(ContainerElementStyle.Wrapped, buttonelement));
        }

        private async Task<Button> CreateButtonAsync(string buttonText, RoutedEventHandler buttonClick)
        {
            Button buttonelement = null;
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            buttonelement = new Button();
            buttonelement.Content = buttonText;
            buttonelement.Visibility = Visibility.Visible;
            buttonelement.Background = System.Windows.Media.Brushes.Transparent;
            buttonelement.BorderThickness = new Thickness(0);
            buttonelement.Style = new Style();
            if (buttonClick != null)
            {
                buttonelement.Click += buttonClick;
            }
            await TaskScheduler.Default;
            return buttonelement;
        }
    }
}
