using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using OptionsHelper;
using Task = System.Threading.Tasks.Task;

namespace AsyncQuickExtender
{
    [ProvideOptionPage(typeof(DialogPageProvider.General), "QuickInfo Extender", "General", 0, 0, true)]
    [Guid(ProjectPackage.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]

    public sealed class ProjectPackage : AsyncPackage
    {
        public const string PackageGuidString = "a7f6c139-923b-4f18-8e7e-13643d3b7e13";

        public ProjectPackage()
        {
        }

         protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
        }
    }
}
