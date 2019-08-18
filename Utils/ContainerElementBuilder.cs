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

            foreach (var part in symbolToDisplay.ToDisplayParts())
            {
                runs.Add(new ClassifiedTextRun(part.Kind.ToClassificationTypeName(), part.ToString()));
            }
            _containerElementList.Add(new ContainerElement(ContainerElementStyle.Wrapped, new ImageElement(new ImageId(KnownMonikers.MethodPrivate.Guid, KnownMonikers.MethodPrivate.Id)), new ClassifiedTextElement(runs.ToArray())));
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
