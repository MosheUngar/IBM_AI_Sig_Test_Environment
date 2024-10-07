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
    public partial class FixSigData : DevExpress.XtraBars.Ribbon.RibbonForm
        {
        public FixSigData()
            {
            InitializeComponent();
            //get data from data base
            setData();

            }


        public void setData()
            {
            //open wait form
            WaitForm1 waitForm1 = new WaitForm1();
            waitForm1.Show();
            gridControl1.DataSource = Classes.FixErxAutomation.GetFixErxAutomation();
            //close wait form
            waitForm1.Close();
            }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
            {
            //download data to csv
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                gridControl1.ExportToCsv(saveFileDialog.FileName);
                XtraMessageBox.Show("Data has been saved to " + saveFileDialog.FileName, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


            }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
            {
            setData();
            }
        }
    }