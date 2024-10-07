using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using DevExpress.XtraSplashScreen;
using Newtonsoft.Json;
using Sig_AI_Converter.Classes;
using Sig_AI_Converter.Forms;
using Sig_AI_Converter.IBM_AI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sig_AI_Converter
    {
    public partial class MainForm : DevExpress.XtraBars.Ribbon.RibbonForm
        {

        Sig_AI_Converter.IBM_AI.Root root = new Sig_AI_Converter.IBM_AI.Root();

        public MainForm()
            {
            InitializeComponent();
            //the create new batch button disable
            barButtonItem2.Enabled = false;
            layoutControlGroup5.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //set panel 2 visibility to false
            splitContainer1.Panel2Collapsed = true;
            showBtnForSig();
            }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
            {
            //fill the grid with data
            //show wait form
            splashScreenManager1.ShowWaitForm();
            gridControlErxInfoForSig_Test.DataSource = Classes.DirectionsInfo.GetDirectionsInfo(true, "");
            //enable the create new batch button
            splitContainer1.Panel2Collapsed = true;
            barButtonItem2.Enabled = true;
            showBtnForSig();
            //hide wait form
            splashScreenManager1.CloseWaitForm();
            }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
            {
            //if grid is empty return
            if (gridViewErxInfoForSig_Test.RowCount == 0)
                {
                XtraMessageBox.Show("No data to process", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
                }

            //open the splash screen
            splashScreenManager1.ShowWaitForm();

            //get the data from the grid add each row to a PrescriptionBatch
            List<PrescriptionBatch> prescriptionBatches = new List<PrescriptionBatch>();
            for (int i = 0; i < gridViewErxInfoForSig_Test.RowCount && prescriptionBatches.Count < 50; i++)
                {
                var id = gridViewErxInfoForSig_Test.GetRowCellValue(i, "eRxMsgId")?.ToString() ?? string.Empty;
                var text = gridViewErxInfoForSig_Test.GetRowCellValue(i, "Directions")?.ToString() ?? string.Empty;
                var drugId = gridViewErxInfoForSig_Test.GetRowCellValue(i, "NDC")?.ToString() ?? string.Empty;
                var drugName = gridViewErxInfoForSig_Test.GetRowCellValue(i, "Drug")?.ToString() ?? string.Empty;
                var drugForm = gridViewErxInfoForSig_Test.GetRowCellValue(i, "FormName")?.ToString() ?? string.Empty;
                var drugRoute = gridViewErxInfoForSig_Test.GetRowCellValue(i, "RouteName")?.ToString() ?? string.Empty;
                var drugStrength = gridViewErxInfoForSig_Test.GetRowCellValue(i, "Strength")?.ToString() ?? string.Empty;

                //add a check to the column
                gridViewErxInfoForSig_Test.SetRowCellValue(i, "Checked", true);

                prescriptionBatches.Add(new PrescriptionBatch
                    {
                    id = id,
                    text = text,
                    drug_id = drugId,
                    drug_name = drugName,
                    drug_form = drugForm,
                    drug_route = drugRoute,
                    drug_strength = drugStrength
                    });

                }

            //add the word "prescriptions":  before the array of prescriptions
            var json = JsonConvert.SerializeObject(new { prescriptions = prescriptionBatches }, Formatting.Indented);

            //request the batch id
            var response = new PrescriptionBatchResponse().GetPrescriptionBatchResponse(json);

            //foreach record in the grid add to table
            //get the data from the grid
            var GridData = gridViewErxInfoForSig_Test.DataSource as List<Classes.DirectionsInfo>;

            foreach (var data in GridData)
                {
                //add the record to the table
                new Classes.DirectionsInfo().InsertDirectionsInfo(data.eRxMsgId, data.Drug, data.Directions, data.NDC, data.FormName, data.RouteName, data.Strength, response.batch_id);

                }


            //enable the create new batch button
            barButtonItem2.Enabled = false;


            //get results from batch id
            splashScreenManager1.CloseWaitForm();

            //show message that the batch is complete with the batch id
            XtraMessageBox.Show("The batch is complete with the batch id " + response.batch_id, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }


        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
            {
   
            //get batch ids from the table
            var batchIds = Classes.DirectionsInfo.GetBatchID();

            //show the batch form
            Batchs batchs = new Batchs(batchIds);
            batchs.ShowDialog();



            //get the batch id
            var batch_id = batchs.batch_id;

            //if the batch id is empty return
            if (string.IsNullOrEmpty(batch_id))
                {
                return;
                }

            //open splash screen
            splashScreenManager1.ShowWaitForm();

            //enable the create new batch button
            barButtonItem2.Enabled = false;

            //get all processing prescriptions
            var processingPrescriptions = new RootProcessingPrescription().processing_prescriptions_results();

            //check if batch_id is in the processingPrescriptions list
            if (processingPrescriptions.processing_prescriptions.Any(x => x.batch_id == batch_id))
                {
                //close the splash screen
                splashScreenManager1.CloseWaitForm();
                //if it is then show message that the batch is still processing
                XtraMessageBox.Show("The batch is still processing", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
                }
            else
                {
                //request the batch results
                root = null;
                root = new PrescriptionBatchResponse().PrescriptionBatchResults(batch_id);
                }

            //fill the grid with data
            gridControlErxInfoForSig_Test.DataSource = Classes.DirectionsInfo.GetDirectionsInfo(false, batch_id);
            showResult();
            //close the splash screen
            splashScreenManager1.CloseWaitForm();
            }


        public void showBtnForSig()
            {
            //if panel 2 is collapsed enable the buttons
            if (splitContainer1.Panel2Collapsed)
                {
                barButtonItem10.Enabled = false;
                barButtonItem11.Enabled = false;
                barCheckItem4.Enabled = false;
                }
            else
                {
                barButtonItem10.Enabled = true;
                barButtonItem11.Enabled = true;
                barCheckItem4.Enabled = true;
                }


            }

        public void showResult()
            {
            //refresh the grid
            //check if there is data in the grid
            if (gridViewErxInfoForSig_Test.RowCount != 0 && root.batch_id != null)
                {
                try
                    {
                    //get the id of the selected row
                    var id = gridViewErxInfoForSig_Test.GetRowCellValue(gridViewErxInfoForSig_Test.FocusedRowHandle, "eRxMsgId")?.ToString() ?? string.Empty;

                    //get the prescription from the root
                    var prescription = root.prescriptions.FirstOrDefault(x => x.id == id);

                    //if prescription is null return
                    if (prescription == null)
                        {
                        return;
                        }


                    //convert the result JSON to class
                    // display the results in layoutControlGroup1

                    var result = JsonConvert.DeserializeObject<List<Result_Details>>(prescription.results);
                    splitContainer1.Panel2Collapsed = false;
                  
                    //set the main values
                    labelControl1411.Text = prescription.id;
                    labelControl14.Text = prescription.valid ? "Yes" : "No";
                    labelControl141.Text = prescription.error_message?.ToString() ?? "No Error";
                    labelControl1412.Text = root.batch_id;

                    //set request information
                    //get selected row drug name
                    labelControl14121.Text = gridViewErxInfoForSig_Test.GetRowCellValue(gridViewErxInfoForSig_Test.FocusedRowHandle, "Drug")?.ToString() ?? string.Empty;
                    labelControl14111.Text = gridViewErxInfoForSig_Test.GetRowCellValue(gridViewErxInfoForSig_Test.FocusedRowHandle, "Directions")?.ToString() ?? string.Empty;
                    labelControl142.Text = gridViewErxInfoForSig_Test.GetRowCellValue(gridViewErxInfoForSig_Test.FocusedRowHandle, "FormName")?.ToString() ?? string.Empty;
                    labelControl1413.Text = gridViewErxInfoForSig_Test.GetRowCellValue(gridViewErxInfoForSig_Test.FocusedRowHandle, "RouteName")?.ToString() ?? string.Empty;
                    labelControl14131.Text = gridViewErxInfoForSig_Test.GetRowCellValue(gridViewErxInfoForSig_Test.FocusedRowHandle, "Strength")?.ToString() ?? string.Empty;

                    // set the grid to the result
                    labelControl1.Text = result[0].generated_sig;
                    labelControl2.Text = result[0].f_usage_instruction;
                    labelControl3.Text = result[0].f_method_of_usage;
                    labelControl4.Text = result[0].f_frequency_of_usage;
                    labelControl5.Text = result[0].f_time_of_usage;
                    labelControl6.Text = result[0].f_total_dosage;
                    labelControl7.Text = result[0].f_duration;
                    labelControl8.Text = result[0].f_symptoms;
                    labelControl9.Text = result[0].f_special_instructions;
                    labelControl10.Text = result[0].prn ? "Yes" : "No";
                    labelControl11.Text = result[0].times_per_day?.ToString() ?? "0";
                    labelControl12.Text = result[0].times_per_month?.ToString() ?? "0";
                    labelControl13.Text = result[0].quantity_per_dose?.ToString() ?? "0";

                    //if there is a second result then display it
                    if (result.Count > 1)
                        {
                        layoutControlGroup5.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        labelControl15.Text = result[1].generated_sig;
                        labelControl21.Text = result[1].f_usage_instruction;
                        labelControl31.Text = result[1].f_method_of_usage;
                        labelControl41.Text = result[1].f_frequency_of_usage;
                        labelControl51.Text = result[1].f_time_of_usage;
                        labelControl61.Text = result[1].f_total_dosage;
                        labelControl71.Text = result[1].f_duration;
                        labelControl81.Text = result[1].f_symptoms;
                        labelControl91.Text = result[1].f_special_instructions;
                        labelControl101.Text = result[1].prn ? "Yes" : "No";
                        labelControl111.Text = result[1].times_per_day?.ToString() ?? "0";
                        labelControl121.Text = result[1].times_per_month?.ToString() ?? "0";
                        labelControl131.Text = result[1].quantity_per_dose?.ToString() ?? "0";
                        }
                    else
                        {
                        layoutControlGroup5.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        }

                    //show the buttons
                    showBtnForSig();





                    }
                catch (Exception ex)
                    {
                    //if there is an error then show the error
                    XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }


                }
            }

        private void gridViewErxInfoForSig_Test_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
            {
            showResult();
            }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
            {
            //if panel 2 is collapsed return
            if (splitContainer1.Panel2Collapsed)
                {
                //show message that there no sig to approve
                XtraMessageBox.Show("There are no Sig to approve", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

                }


            //get the selected row id 
            var id = gridViewErxInfoForSig_Test.GetRowCellValue(gridViewErxInfoForSig_Test.FocusedRowHandle, "eRxMsgId")?.ToString() ?? string.Empty;

            //update the record in the database and refresh the grid
            new Classes.DirectionsInfo().UpdateDirectionsInfo(id);

            //add valid to the row
            gridViewErxInfoForSig_Test.SetRowCellValue(gridViewErxInfoForSig_Test.FocusedRowHandle, "Valid", "True");

            }

        private void gridViewErxInfoForSig_Test_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
            {
            //if valid is true then set the background color to green
            if (e.Column.FieldName == "Valid")
                {
                var valid = gridViewErxInfoForSig_Test.GetRowCellValue(e.RowHandle, "Valid")?.ToString() ?? string.Empty;
                if (valid == "True")
                    {
                    e.Appearance.BackColor = Color.LightGreen;
                    }
                else
                    {
                    e.Appearance.BackColor = Color.LightCoral;
                    }
                }
            }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
            {
            //if panel 2 is collapsed return
            if (splitContainer1.Panel2Collapsed)
                {
                //show message that there no sig to approve
                XtraMessageBox.Show("There are no Sig to fix", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

                }

            //open the feedback form
            FeedbackForm feedbackForm = new FeedbackForm(gridViewErxInfoForSig_Test.GetRowCellValue(gridViewErxInfoForSig_Test.FocusedRowHandle, "eRxMsgId")?.ToString() ?? string.Empty, labelControl1.Text);
            feedbackForm.ShowDialog();
            }

        private void barCheckItem4_CheckedChanged(object sender, ItemClickEventArgs e)
            {
            //if the check box text is show only result then collapse the panel
            if (barCheckItem4.Checked)
                {
                splitContainer1.Panel1Collapsed = true;
                //set the text to show both
                barCheckItem4.Caption = "Show Both";
                }
            else
                {
                splitContainer1.Panel1Collapsed = false;
                //set the text to show only result
                barCheckItem4.Caption = "Show Only Result";
                }
            }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
            {
            FixSigData fixSigData = new FixSigData();
            fixSigData.ShowDialog();

            }

        }
    }
        
    
