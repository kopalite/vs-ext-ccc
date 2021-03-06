﻿using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;

namespace CodeContracts.Contrib.Helpers
{
    internal class VSModelHelper
    {
        //public static MenuCommand GetCreateCodeContractCommand(IServiceProvider provider)
        //{
        //    var commandService = provider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
        //    return commandService.FindCommand(new CommandID(Guid.Parse("8a37555e-23b6-4c43-b200-702b66b0326d"), 256));
        //}

        //public static MenuCommand GetCreateContractProxyCommand(IServiceProvider provider)
        //{
        //    var commandService = provider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
        //    return commandService.FindCommand(new CommandID(Guid.Parse("19db6c4c-af0a-46b8-80ff-02a586ed1574"), 256));
        //}

        //public static MenuCommand GetCreateProxyAttributeCommand(IServiceProvider provider)
        //{
        //    var commandService = provider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
        //    return commandService.FindCommand(new CommandID(Guid.Parse("61063f2f-f3b9-4140-8fae-2ed8036d9640"), 256));
        //}

        public static IEnumerable<ProjectItem> GetSelectedItems(IServiceProvider provider)
        {
            var appObject = provider.GetService(typeof(DTE)) as EnvDTE80.DTE2;
            var explorer = appObject.ToolWindows.SolutionExplorer;
            Array selectedItems = (Array)explorer.SelectedItems;
            if (selectedItems != null)
            {
                foreach (UIHierarchyItem selectedItem in selectedItems)
                {
                    yield return selectedItem.Object as ProjectItem;
                }
            }
        }

        public static IEnumerable<Project> GetSelectedProjects(IServiceProvider provider)
        {
            var appObject = provider.GetService(typeof(DTE)) as EnvDTE80.DTE2;
            var explorer = appObject.ToolWindows.SolutionExplorer;
            Array selectedItems = (Array)explorer.SelectedItems;
            if (selectedItems != null)
            {
                foreach (UIHierarchyItem selectedItem in selectedItems)
                {
                    yield return selectedItem.Object as Project;
                }
            }
        }

        public static IEnumerable<ProjectItem> GetAllCodeContracts(IServiceProvider provider)
        {
            var appObject = provider.GetService(typeof(DTE)) as EnvDTE80.DTE2;
            var solutionDirectory = Path.GetDirectoryName(appObject.Solution.FullName);
            var contractFilePattern = string.Format("*{0}.cs", IdentifiersHelper.CodeContractClassFileSuffix);
            var codeContractFiles = GetFiles(solutionDirectory, contractFilePattern);
            foreach (var codeContractFile in codeContractFiles)
            {
                yield return appObject.Solution.FindProjectItem(codeContractFile);
            }
        }

        private static IEnumerable<string> GetFiles(string directory, string fileNamePattern)
        {
            var directoryInfo = new DirectoryInfo(directory);
            var files = directoryInfo.GetFiles(fileNamePattern).Select(f => f.FullName).ToArray();
            foreach (var subDirectory in directoryInfo.GetDirectories())
            {
                files = files.Union(GetFiles(subDirectory.FullName, fileNamePattern)).ToArray();
            }
            return files;
        }

        public static void ShowMessage(IServiceProvider provider, string title, string message)
        {
            // Show a message box to prove we were here
            VsShellUtilities.ShowMessageBox(
                provider,
                message,
                title,
                OLEMSGICON.OLEMSGICON_INFO,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }
    }
}
