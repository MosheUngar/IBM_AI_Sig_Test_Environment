using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sig_AI_Converter.Forms
    {

    public partial class Batchs : DevExpress.XtraEditors.XtraForm
        {
        public string batch_id { get; set; }

        public Batchs(List<string> Batches)
            {
            InitializeComponent();
            //set combo box data source
            comboBoxEdit1.Properties.Items.AddRange(Batches);
            }

        private void comboBoxEdit1_SelectedValueChanged(object sender, EventArgs e)
            {
            //set the batch id
            batch_id = comboBoxEdit1.Text;
            //close the form
            this.Close();
            }
        }
    }