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

    }

    //insert into the database
    }
