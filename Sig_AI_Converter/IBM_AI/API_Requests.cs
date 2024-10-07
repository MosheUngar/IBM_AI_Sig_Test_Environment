using DevExpress.XtraPrinting.Native.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sig_AI_Converter.IBM_AI
    {
    public class API_Requests
        {
        //create base url
        public static string BaseUrl = "https://application-0c.1mi90o2od622.us-east.codeengine.appdomain.cloud/";
        }

    public class PrescriptionBatch
        {
        public string id { get; set; }
        public string text { get; set; }
        public string drug_id { get; set; }
        public string drug_name { get; set; }
        public string drug_form { get; set; }
        public string drug_route { get; set; }
        public string drug_strength { get; set; }
        }

    public class PrescriptionBatchResponse
        {
        public string trace_id { get; set; }
        public string batch_id { get; set; }

        //http request to post to get the batch id
        public PrescriptionBatchResponse GetPrescriptionBatchResponse(string prescriptionBatches)
            {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, API_Requests.BaseUrl + "start_prescription_batch/");
            request.Headers.Add("Accept", "application/json");
            var content = new StringContent(prescriptionBatches, null, "application/json");
            request.Content = content;
            var response = client.SendAsync(request);
            return JsonConvert.DeserializeObject<PrescriptionBatchResponse>(response.Result.Content.ReadAsStringAsync().Result);
            }

        public Sig_AI_Converter.IBM_AI.Root PrescriptionBatchResults(string id)
            {
            //http request to get the batch results
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{API_Requests.BaseUrl}batch_result/?batch_id={id}");
            var response = client.SendAsync(request);
            var json = response.Result.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<Sig_AI_Converter.IBM_AI.Root>(response.Result.Content.ReadAsStringAsync().Result);
            }
        }

    //request to chack if batch is done
    public class ProcessingPrescription
        {
        public string batch_id { get; set; }
        public int processing_time { get; set; }
        }
    public class RootProcessingPrescription
        {
            public string trace_id { get; set; }
            public List<ProcessingPrescription> processing_prescriptions { get; set; }


        //http request to get the batch results
        public RootProcessingPrescription processing_prescriptions_results()
            {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{API_Requests.BaseUrl}list_running_batches/");
            var response = client.SendAsync(request);
            var json = response.Result.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<RootProcessingPrescription>(response.Result.Content.ReadAsStringAsync().Result);


            }


        public class provide_user_feedback
            {
            public string prescription_id { get; set; }
            public string corrected_transcription { get; set; }
            }

        public class provide_user_feedback_response
            {
            public string trace_id { get; set; }

            //http request to post to send user feedback
            public provide_user_feedback_response SendUserFeedback(string json)
                {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, API_Requests.BaseUrl + "provide_user_feedback/");
                request.Headers.Add("Accept", "application/json");
                var content = new StringContent(json, null, "application/json");
                request.Content = content;
                var response = client.SendAsync(request);
                return JsonConvert.DeserializeObject<provide_user_feedback_response>(response.Result.Content.ReadAsStringAsync().Result);
                }

            }


        }
    }



