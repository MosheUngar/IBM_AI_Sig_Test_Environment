using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sig_AI_Converter.Classes
    {
    public class SqlConn
        {
        public static string GetConnection()
            {
            return "data source=FRAMEWORKS;initial catalog=DocuWiz;user id=docuwiz_user;password=docuWIZ1;MultipleActiveResultSets=True;App=EntityFramework";
            }
        }
    }
