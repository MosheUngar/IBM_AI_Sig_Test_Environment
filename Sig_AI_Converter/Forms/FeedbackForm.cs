using DevExpress.XtraEditors;
using Newtonsoft.Json;
using Sig_AI_Converter.Classes;
using Sig_AI_Converter.IBM_AI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Sig_AI_Converter.IBM_AI.RootProcessingPrescription;

namespace Sig_AI_Converter.Forms
    {
    public partial class FeedbackForm : DevExpress.XtraEditors.XtraForm
        {
        string _AI_Sig_Response = "";

        public FeedbackForm(string prescription_id, string AI_Sig_Response)
            {
            InitializeComponent();
            textEdit1.Text = prescription_id;
            _AI_Sig_Response = AI_Sig_Response;

            }

        private void simpleButton2_Click(object sender, EventArgs e)
            {
            //close the form
            this.Close();
            }

        private void simpleButton1_Click(object sender, EventArgs e)
            {
        //show splash screen wait form
        WaitForm1 waitForm1 = new WaitForm1();
            waitForm1.Show();

        repit:
            provide_user_feedback_response provide_User_Feedback = new provide_user_feedback_response();
            var json = "{\r\n  \"feedback_prescriptions\": [\r\n    {\r\n      \"prescription_id\": \"" + textEdit1.Text + "\",\r\n      \"corrected_transcription\": \"" + textEdit2.Text + "\"\r\n    }\r\n  ]\r\n}";
            var response = provide_User_Feedback.SendUserFeedback(json);
            //if response.trace id is not null then the feedback was successful
            if (response.trace_id != null)
                {
                FixErxAutomation fixErxAutomation = new FixErxAutomation();
                fixErxAutomation.InsertFixErxAutomation(new Guid(textEdit1.Text), _AI_Sig_Response, textEdit2.Text, richTextBox1.Text);
                //close the form
                waitForm1.Close();
                XtraMessageBox.Show("Feedback was successful" + Environment.NewLine + "trace_id: " + response.trace_id, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                }
            else
                {
                goto repit;
                }


            // Code to repeat the feedback
        }
        }
    }