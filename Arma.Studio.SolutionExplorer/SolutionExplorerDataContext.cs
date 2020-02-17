﻿using Arma.Studio.Data;
using Arma.Studio.Data.IO;
using Arma.Studio.Data.TextEditor;
using Arma.Studio.Data.UI;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Arma.Studio.SolutionExplorer
{
    public class SolutionExplorerDataContext : DockableBase
    {
        public object InRenameMode
        {
            get => this._InRenameMode;
            set
            {
                this._InRenameMode = value;
                this.RaisePropertyChanged();
            }
        }
        private object _InRenameMode;

        public ICommand CmdReleaseRename => new RelayCommand(() => this.InRenameMode = null);
        public ICommand CmdOpen => new RelayCommand<FileFolderBase>((ffb) =>
        {
            if (ffb is File file)
            {
                if (file.PhysicalPath != null && System.IO.File.Exists(file.PhysicalPath))
                {
                    (Application.Current as IApp).MainWindow.OpenFile(file);
                }
                else
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show(
                        String.Format(Properties.Language.FileNotFound_Body_0filename, file.PhysicalPath),
                        Properties.Language.FileNotFound_Header,
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
        });

        public ICommand CmdAddPbo => new RelayCommand(() =>
        {
            var ofd = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                Title = Properties.Language.SolutionExplorer_PleaseSelectYourProjectFolder
            };
            var res = ofd.ShowDialog();
            if (res == CommonFileDialogResult.Ok)
            {
                var pbo = new PBO { Name = ofd.FileName };
                pbo.Rescan();
                this.FileManagement.Add(pbo);
            }
        });
        public ICommand CmdRescanPbo => new RelayCommand<PBO>((pbo) => { });






        public override string Title { get => Properties.Language.SolutionExplorer; set { throw new NotSupportedException(); } }

        public IFileManagement FileManagement => (Application.Current as IApp).MainWindow.FileManagement;

    }
}
