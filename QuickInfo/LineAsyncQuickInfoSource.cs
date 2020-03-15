using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
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
            _quickInfoUtils = new OverloadGenerator(_textBuffer, session);
            var overloads = await _quickInfoUtils.GetAllOverLoadsForMousePositionAsync();
            ContainerElementBuilder containerBuilder = new ContainerElementBuilder();
            if (overloads.Count > 0)
            {
                if (overloads.Count > 0)
                {
                    foreach (var item in overloads)
                    {
                        containerBuilder.AddContainer(item);
                    }
                }

                var containers = containerBuilder.Build();
                return await Task.FromResult(new QuickInfoItem(session.ApplicableToSpan, containers)).ConfigureAwait(false);
            }
            return await Task.FromResult<QuickInfoItem>(null).ConfigureAwait(false);
        }

        public void Dispose()
        {
            // This provider does not perform any cleanup.
        }
    }
}
