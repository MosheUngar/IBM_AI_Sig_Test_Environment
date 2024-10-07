using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sig_AI_Converter.Classes
    {
    public class DirectionsInfo
        {
        public Guid eRxMsgId { get; set; }
        public string Drug { get; set; }
        public string Directions { get; set; }
        public string NDC { get; set; }
        public string FormName { get; set; }
        public string RouteName { get; set; }
        public string Strength { get; set; }
        public string Batch_id { get; set; }
        public bool Valid { get; set; }

        //insert data into the database
        public void InsertDirectionsInfo(Guid eRxMsgId, string drug, string Directions, string NDC, string FormName, string RouteName, string Sterngth, string Batch_ID)
            {
            using (var db = new SqlConnection(SqlConn.GetConnection()))
                {
                db.Execute("InsertIntoErxInfoForSig_Test", new
                    {
                    eRxMsgId = eRxMsgId,
                    Drug = drug,
                    Directions = Directions,
                    NDC = NDC,
                    FormName = FormName,
                    RouteName = RouteName,
                    Sterngth = Sterngth,
                    Batch_id = Batch_ID
                    }, commandType: CommandType.StoredProcedure);
                }
            }

        //create get from table distinct batch id
        public static List<string> GetBatchID()
            {
            List<string> batchID = new List<string>();
            using (var db = new SqlConnection(SqlConn.GetConnection()))
                {
                using (var command = new SqlCommand("SELECT DISTINCT Batch_id FROM dbo.ErxInfoForSig_Test\r\n", db))
                    {
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 0; // Set the command timeout here
                    db.Open();
                    using (var reader = command.ExecuteReader())
                        {
                        batchID = reader.Parse<string>().ToList();
                        }
                    }
                }
            return batchID;
            }

        //get data from procedure using dapper
        public static List<DirectionsInfo> GetDirectionsInfo(bool NewRecoreds, string Batch_id)
            {
            List<DirectionsInfo> directionsInfo = new List<DirectionsInfo>();
            using (var db = new SqlConnection(SqlConn.GetConnection()))
                {
                using (var command = new SqlCommand("GeErxInfoForSig_Test", db))
                    {
                    command.Parameters.Add(new SqlParameter("@NewData", NewRecoreds));
                    command.Parameters.Add(new SqlParameter("@BatchID", Batch_id));
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 0; // Set the command timeout here
                    db.Open();
                    using (var reader = command.ExecuteReader())
                        {
                        directionsInfo = reader.Parse<DirectionsInfo>().ToList();
                        }
                    }
                }
            return directionsInfo;
            }

        public void UpdateDirectionsInfo(string id)
            {
            using (var db = new SqlConnection(SqlConn.GetConnection()))
                {
                db.Execute("UpdateErxInfoForSig_Test", new
                    {
                    Erx_Id = id
                    }, commandType: CommandType.StoredProcedure);
                }

            }
        }
  
    }
