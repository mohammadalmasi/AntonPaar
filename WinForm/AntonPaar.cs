using AppService;
using System;

namespace WinForm
{
    public partial class AntonPaar : Form, IReporter
    {
        private IPlugin plugin;
        private Guid projectHandle;
        private DialogResult dialogResult;
        private OpenFileDialog openFileDialog;

        public AntonPaar()
        {
            InitializeComponent();
        }

        public void LogStatus(string status)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { listBox.Items.Add(status); }));
            }
        }

        public void LogProgress(decimal percentage)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { progress.Value = (int)percentage; }));
            }
        }

        private void btnPath_Click(object sender, EventArgs e)
        {
            openFileDialog = new OpenFileDialog();
            dialogResult = openFileDialog.ShowDialog();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            listBox.Items.Clear();
            if (dialogResult == DialogResult.OK)
            {
                projectHandle = Guid.NewGuid();
                plugin = new Plugin();
                Task.Factory.StartNew(() => { plugin.Start(projectHandle, openFileDialog.FileName, this); });
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() => { plugin.Stop(projectHandle); });
        }
    }
}