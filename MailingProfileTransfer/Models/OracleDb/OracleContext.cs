using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using MailingProfileTransfer.Models.Helpers;
//using System.Data.OracleClient;

namespace MailingProfileTransfer.Models
{
    public class OracleContext
    {
        static string _connStrOracle;
        static string ConnStrOracle
        {
            get
            {
                _connStrOracle = ChoiseConnectionString.connStrOracle;
                return _connStrOracle;
            }
        }
        
        /// <summary>
        /// Метод поиска тир из бд по ВН
        /// </summary>
        /// <param name="pin"></param>
        /// <returns></returns>
        public static List<string> GetTirFromPin(int pin, TimeInterval timeInterval)
        {
            List<string> list = new List<string>();
            string queryString = $@"select s.n_tir
                        from kb_spros s 
                        left join kb_zak z on s.id_zak=z.id
                        where z.id_klient = {pin} and 
                        s.dt_zakaz >= '{timeInterval.Time_1.ToShortDateString()}' and 
                        s.dt_zakaz <= '{timeInterval.Time_2.ToShortDateString()}'";
            using (OracleConnection oracleConnection = new OracleConnection(ConnStrOracle))
            {

                OracleCommand command = new OracleCommand(queryString, oracleConnection);
                oracleConnection.Open();
                OracleDataReader reader = command.ExecuteReader();
                while ((reader != null) && reader.Read())
                {
                    string n_tir = reader.IsDBNull(0) ? "" 
                        : reader.GetString(0);
                    list.Add(n_tir);
                }
                reader.Close();
            }
            return list;
            
        }
    }
}
