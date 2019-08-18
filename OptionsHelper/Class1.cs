using Microsoft.VisualStudio.Shell;

using System;
using System.Runtime.InteropServices;
using System.Threading;

using Task = System.Threading.Tasks.Task;
using OptionsHelper;

namespace OptionsHelper
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideOptionPage(typeof(DialogPageProvider.General), "QuickInfo Extender", "General", 0, 0, true)]
    [Guid("8bb519a5-4864-43b0-8684-e2f2f723100c")]
    public sealed class OptionsPackage : AsyncPackage
    {
        protected override Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            return base.InitializeAsync(cancellationToken, progress);
        }
    }
}
