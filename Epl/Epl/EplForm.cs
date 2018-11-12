using Epl.Presenter;
using Epl.View;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Epl
{
    public partial class EplForm : Form, IEplView
    {
        public EplForm()
        {
            InitializeComponent();
            openFileDialog.Filter = "CSV files(*.csv)| *.csv";
        }

        public DataTable SortedPayloadTable { set => dataGridView1.DataSource = value; }
        public string FileName { get => openFileDialog.FileName; set => openFileDialog.FileName = value; }
        public string ErrorMessage { set => MessageBox.Show(value); }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    dataGridView1.Visible = true;
                    var eplpresenter = new EplPresenter(this);
                    eplpresenter.ParsePayloadData(openFileDialog.FileName);
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
