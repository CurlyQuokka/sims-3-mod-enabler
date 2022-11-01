using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace sims3_mod_enabler
{
    internal partial class LoadingForm : Form
    {

        private IReportProgress reporter;
        internal LoadingForm(IReportProgress reporter, string windowTitle, string windowText)
        {
            this.reporter = reporter;
            this.reporter.PropertyChanged += changed_PropertyChanged;
            this.ControlBox = false;
            this.TopMost = true;
            InitializeComponent();
            this.Text = windowTitle;
            windowTextLabel.Text = windowText;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
        }

        private void changed_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsHandleCreated) {
                if (e.PropertyName == "Progress")
                {
                    Invoke(() =>
                    {
                        int progress = reporter.GetProgress();
                        loadingPackageBar.Value = progress;
                        // this is dirty hack to get the progress bar filled as much as possible
                        if (progress > 0)
                        {
                            loadingPackageBar.Value = progress - 1;
                        }
                    });
                }
                else
                {
                    Invoke(() => { Close(); });
                }
            }
        }
    }
}
