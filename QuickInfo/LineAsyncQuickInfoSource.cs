using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using System.Windows;
using System.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using OptionsHelper;
using QuickInfoUtils;

namespace QuickInfoExtender
{
    internal sealed class LineAsyncQuickInfoSource : IAsyncQuickInfoSource
    {
        private ITextBuffer _textBuffer;
        private OverloadGenerator _quickInfoUtils;
        public LineAsyncQuickInfoSource(ITextBuffer textBuffer)
        {
            _textBuffer = textBuffer;
        }

        public async Task<QuickInfoItem> GetQuickInfoItemAsync(IAsyncQuickInfoSession session, CancellationToken cancellationToken)
        {
            if (!_textBuffer.ContentType.DisplayName.Contains("CSharp")) return await System.Threading.Tasks.Task.FromResult<QuickInfoItem>(null);
            _quickInfoUtils = new OverloadGenerator(_textBuffer, session);
            var overloads = _quickInfoUtils.GetAllOverLoadsForMousePosition();
            ContainerElementBuilder uIHelper = new ContainerElementBuilder();
            GeneralOptions options = await GeneralOptions.GetLiveInstanceAsync();
            var showButton = options.ShowButtonToBrowser;
            if (overloads.Count > 0 || showButton)
            {
                if (overloads.Count > 0)
                {
                    foreach (var item in overloads)
                    {
                        uIHelper.AddContainer(item);
                    }
                }
                var symbol = _quickInfoUtils.GetSymbol();
                if (showButton && symbol != null)
                {
                    uIHelper.AddContainer("Search in browser", Buttonelement_Click);
                }

                var containers = uIHelper.Build();
                return await System.Threading.Tasks.Task.FromResult(new QuickInfoItem(session.ApplicableToSpan, containers));
            }
            return await System.Threading.Tasks.Task.FromResult<QuickInfoItem>(null);
        }

        private void Buttonelement_Click(object sender, RoutedEventArgs e)
        {
            GeneralOptions options = GeneralOptions.GetLiveInstanceAsync().Result;
            string message = options.SearchUrl;
            var symbol = _quickInfoUtils.GetSymbol();
            Process.Start($"{message}{symbol.ToDisplayString()}");
        }

        public void Dispose()
        {
            // This provider does not perform any cleanup.
        }
    }
}
