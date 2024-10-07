using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sig_AI_Converter.IBM_AI
    {
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Prescription
        {
        public string id { get; set; }
        public string results { get; set; }
        public List<RootResult> root { get; set; }
        public bool valid { get; set; }
        public object error_message { get; set; }
        }

    public class Root
        {
        public string trace_id { get; set; }
        public string batch_id { get; set; }
        public List<Prescription> prescriptions { get; set; }
        }


    public class Result_Details 
        {
        public string generated_sig { get; set; }
        public bool prn { get; set; }
        public object times_per_day { get; set; }
        public object times_per_month { get; set; }
        public object quantity_per_dose { get; set; }
        public string f_usage_instruction { get; set; }
        public string f_method_of_usage { get; set; }
        public string f_frequency_of_usage { get; set; }
        public string f_time_of_usage { get; set; }
        public string f_total_dosage { get; set; }
        public string f_duration { get; set; }
        public string f_symptoms { get; set; }
        public string f_special_instructions { get; set; }
        }

    public class RootResult
        {
        public List<Result_Details> MyArray { get; set; }
        }


    }
