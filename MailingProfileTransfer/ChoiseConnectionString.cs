using System;
using System.Collections.Generic;
using MailingProfileTransfer.Models;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Data;


namespace MailingProfileTransfer
{
    /// <summary>
    /// Класс для работы со строкой подключения.
    /// </summary>
    static class ChoiseConnectionString
    {
        /// <summary>
        /// Строка подключения к базе данных MessageDistributionSettings
        /// </summary>
        public static string ConnectionStringName { get; set; }

        public static string ConnectionStringVBClientsName { get; set; }

        public static string ConnectionStringMailingLog { get; set; }

        public static string ConnectionStringOracle { get; set; }

        public static string connStrOracle { get; set; }

        /// <summary>
        /// Выбор подключения к БД.
        /// </summary>
        /// <returns>
        /// true - подключение к базе данных есть, 
        /// false - подключения нет
        /// </returns>
        public static bool ChoiseConnection()
        {
            bool result = false;
            List<AbstractDbContext> contexts = new List<AbstractDbContext>();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Выберите подключение к базе данных " +
                "MessageDistributionSettings (если по-умолчанию, боевая база данных, " +
                "расположенная на сервере srv-sql-01, то просто нажмите Enter:)");
            Console.WriteLine("1 - (или Enter) боевая база данных на сервере srv-sql-01");
            Console.Write("Выберите номер базы дынных: ");
            string dbNumInString = Console.ReadLine();
            Console.ResetColor();
            int dbNum = 0;
            if (string.IsNullOrEmpty(dbNumInString)) dbNumInString = "1";
            if (int.TryParse(dbNumInString, out dbNum))
            {
                switch (dbNum)
                {
                    case 1:
                        ConnectionStringName = "newProfilesContext";
                        ConnectionStringVBClientsName = "VBClientsContext";
                        ConnectionStringMailingLog = "MailingLogContext";
                        connStrOracle = ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString;
                        break;                                    
                    default:
                        ConnectionStringName = "";
                        break;
                }

                using (newProfilesContext contextProfiles = new newProfilesContext())
                {
                    result = Check(contextProfiles);

                }


                if (!String.IsNullOrEmpty(ConnectionStringVBClientsName) && result)
                {
                    using (Models.VBClientsContext contextVBClients = new Models.VBClientsContext())
                    {
                        result = Check(contextVBClients);    
                    }

                }

                using (MalingLogContext contextMailingLog = new MalingLogContext())
                {
                    result = CheckMailingDb(contextMailingLog);
                }

                using (OracleConnection connection = new OracleConnection(connStrOracle))
                {
                    connection.Open();    
                    result = CheckOracleDb(connection);
                }


            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Неправильно введён номер базы данных");
                Console.ResetColor();
                
            }
            return result;
        }

        private static bool Check(AbstractDbContext dbContext)
        {
            try
            {
                if (dbContext.Exists)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Вы успешно подсоединились к " +
                        $"базе данных {dbContext.DatabaseName} на сервере " +
                        $"{dbContext.ServerName.ToUpper()}");
                    Console.ResetColor();
                    return true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Неправильно введён номер базы данных или она недоступна:");
                    Console.ResetColor();
                    return false;
                }
            }
            catch (Exception err)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{err.Message}");
                Console.ResetColor();
                return false;
            }
        }

        private static bool CheckMailingDb(MalingLogContext dbContext)
        {
            try
            {
                if (dbContext.Exists)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Вы успешно подсоединились к " +
                        $"базе данных {dbContext.DatabaseName} на сервере " +
                        $"{dbContext.ServerName.ToUpper()}");
                    Console.ResetColor();
                    return true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Неправильно введён номер базы данных или она недоступна:");
                    Console.ResetColor();
                    return false;
                }
            }
            catch (Exception err)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{err.Message}");
                Console.ResetColor();
                return false;
            }
        }

        private static bool CheckOracleDb(OracleConnection dbContext)
        {
            try
            {
                if (dbContext.State == ConnectionState.Open)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Вы успешно подсоединились к " +
                        $"базе данных {dbContext.DatabaseName} на сервере " +
                        $"{dbContext.DataSource.ToUpper()}");
                    Console.ResetColor();
                    return true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Неправильно введён номер базы данных или она недоступна:");
                    Console.ResetColor();
                    return false;
                }
            }
            catch (Exception err)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{err.Message}");
                Console.ResetColor();
                return false;
            }
        }
    }
}