﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arma.Studio.UI.AvalonDock
{
    public class DockableBaseOnDocumentClosingEventArgs
    {
        public bool Cancel { get; set; }
        public DockableBaseOnDocumentClosingEventArgs()
        {
            this.Cancel = false;
        }
    }
}
