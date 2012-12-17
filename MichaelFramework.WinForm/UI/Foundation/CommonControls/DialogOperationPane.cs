using System;
using System.Windows.Forms;

namespace MichaelFramework.WinForm.UI.Foundation.CommonControls
{
    public partial class DialogOperationPane : UserControl
    {
        public event EventHandler OKButtonClick;

        public bool OKButtonEnabled
        {
            get { return btnOK.Enabled; }
            set { btnOK.Enabled = value; }
        }

        public DialogOperationPane()
        {
            InitializeComponent();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            this.Height = 38;
            base.OnSizeChanged(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            Form f = this.FindForm();
            if (f != null)
            {
                f.AcceptButton = btnOK;
                f.CancelButton = btnCancel;
            }
            this.Dock = DockStyle.Bottom;
            base.OnLoad(e);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (OKButtonClick != null)
                OKButtonClick(this, e);
        }
    }
}
