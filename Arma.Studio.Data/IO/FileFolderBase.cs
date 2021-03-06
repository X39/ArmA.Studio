﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.Data.IO
{
    public abstract class FileFolderBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string callee = "") => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callee));


        public string Name
        {
            get => this._Name;
            set
            {
                this._Name = value;
                this.RaisePropertyChanged();
                this.RaisePropertyChanged(nameof(this.FullPath));
                this.OnNameChanged();
            }
        }
        private string _Name;
        protected virtual void OnNameChanged() { }

        public string FullPath => this.ParentWeak.TryGetTarget(out var parent) ? System.IO.Path.Combine(parent.FullPath, this.Name) : this.Name;

        public FileFolderBase Parent
        {
            get => this.ParentWeak.TryGetTarget(out var target) ? target : null;
            set
            {
                this.ParentWeak.SetTarget(value);
                this.RaisePropertyChanged();
                if (this is IEnumerable<FileFolderBase> ffbs)
                {
                    foreach (var ffb in ffbs)
                    {
                        ffb.Parent = ffb.Parent;
                    }
                }
            }
        }

        public PBO PBO
        {
            get
            {
                var ffb = this;
                while (ffb != null && !(ffb is PBO))
                {
                    ffb = ffb.Parent;
                }
                return ffb as PBO;
            }
        }

        private readonly WeakReference<FileFolderBase> ParentWeak;
        public FileFolderBase()
        {
            this.ParentWeak = new WeakReference<FileFolderBase>(null);
        }

        public string GetText()
        {
            return System.IO.File.ReadAllText(this.FullPath);
        }
    }
}
