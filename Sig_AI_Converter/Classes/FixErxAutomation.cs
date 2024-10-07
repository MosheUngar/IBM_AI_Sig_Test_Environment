using Dapper;
using DevExpress.Data.Linq.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sig_AI_Converter.Classes
    {
    public class FixErxAutomation
        {
        public Guid ErxID { get; set; }
        public string AI_Result_sig { get; set; }
        public string Fix_Sig { get; set; }
        public string Notes { get; set; }
    

        public void InsertFixErxAutomation(Guid ErxID, string AI_Result_sig, string Fix_Sig, string Notes)
            {
            using (var db = new SqlConnection(SqlConn.GetConnection()))
                {
                db.Execute("InsertFixAiErxAutomation_test", new
                    {
                    ErxID = ErxID,
                    AI_Result_sig = AI_Result_sig,
                    Fix_Sig = Fix_Sig,
                    Notes = Notes
                    }, commandType: CommandType.StoredProcedure);
                }
            }

        //gete data from the database
        public static List<FixErxAutomation> GetFixErxAutomation()
            {
            List<FixErxAutomation> fixErxAutomations = new List<FixErxAutomation>();
            using (var db = new SqlConnection(SqlConn.GetConnection()))
                {
                using (var command = new SqlCommand("SELECT * FROM dbo.FixAiErxAutomation_test\r\n", db))
                    {
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 0; // Set the command timeout here
                    db.Open();
                    using (var reader = command.ExecuteReader())
                        {
                        fixErxAutomations = reader.Parse<FixErxAutomation>().ToList();
                        }
                    }
                }
            return fixErxAutomations;
            }

        }

    //insert into the database
    }
