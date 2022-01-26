using MailingProfileTransfer.Models;
using MailingProfileTransfer.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;

namespace MailingProfileTransfer
{
    static class Decisions
    {
        
        /// <summary>
        /// Работник, от лица которого будут проходить изменения.
        /// </summary>
        public static string Employee { get; set; }

        /// <summary>
        /// Представление пользователя, чтобы было понятно, кого записывать в лог.
        /// </summary>
        public static void Autorization()
        {
            bool isAutorization = false;
            while (!isAutorization)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Введите наименование человека, от имени которого" +
        " будут вноситься изменения: ");
                string person = Console.ReadLine();
                if (string.IsNullOrEmpty(person))
                {
                    Console.WriteLine("Представьтесь пожалуйста");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Employee = person;
                    Console.WriteLine($"Добро пожаловать {person}");
                    isAutorization = true;
                }
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Выбор решения проблемы.
        /// </summary>
        public static void DecisionChoise()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Выберите задачу, которую требуется решить:");
            Console.WriteLine("0 - Отмена");
            Console.WriteLine("1 - Создать новую компанию с новыми профилями");
            Console.WriteLine("2 - Просмотр информации о профилях");
            Console.WriteLine("3 - Редактировать профиль");
            Console.WriteLine("4 - Поиск отправленных писем");
            Console.Write("Введите номер: ");
            string decisionNumString = Console.ReadLine();
            Console.ResetColor();
            int decisionNum = 0;
            if (int.TryParse(decisionNumString, out decisionNum))
            {
                try
                {
                    switch (decisionNum)
                    {
                        case 0:
                            break;
                        case 1:
                            CreateNewCompany();
                            break;
                        case 2:
                            ProfileInformation();
                            break;
                        case 3:
                            EditProfileOrCompane();
                            break;
                        case 4:
                            FindLetter();
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Показать полную информацию?");
                    if (Helper.Accept())
                        Console.WriteLine(ex.StackTrace);
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Введён неправильный номер задачи");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Поиск писем
        /// </summary>
        private static void FindLetter()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Выберите задачу, которую требуется решить:");
            Console.WriteLine("0 - Отмена ");
            Console.WriteLine("1 - Поиск по email");
            Console.WriteLine("2 - Поиск по ключевому слову");
            Console.WriteLine("3 - Поиск по ВН");
            Console.WriteLine("4 - Поиск по интервалу времени");
            Console.Write("Введите номер: ");
            string decisionNumString = Console.ReadLine();
            Console.ResetColor();
            int decisionNum = 0;
            if (int.TryParse(decisionNumString, out decisionNum))
            {
                try
                {
                    switch (decisionNum)
                    {
                        case 0:
                            break;
                        case 1:
                            FindLetterByEmail();
                            break;
                        case 2:
                            FindLetterByKeyword();
                            break;
                        case 3:
                            FindLetterByPIN();
                            break;
                        case 4:
                            FindLetterByTimeInterval();
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Показать полную информацию?");
                    if (Helper.Accept())
                        Console.WriteLine(ex.StackTrace);
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Введён неправильный номер задачи");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Поиск за интервал времени
        /// </summary>
        private static void FindLetterByTimeInterval()
        {
            var timeInterval = Helper.SelectInterval();
            DatabaseFacade db = new DatabaseFacade();
            db.FindLetterByTimeInterval(timeInterval);

        }



        /// <summary>
        /// Поиск писемь по ВН
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private static void FindLetterByPIN()
        {            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Введите внутренний номер: ");
            int pin = int.Parse(Console.ReadLine());
            Console.ResetColor();
            var timeInterval = Helper.SelectInterval();
            DatabaseFacade db = new DatabaseFacade();
            db.FindLetterByPIN(pin, timeInterval);           
        }


        /// <summary>
        /// Поиск по ключевому слову
        /// </summary>
        private static void FindLetterByKeyword()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Введите ключевое слово: ");
            string keyword = Console.ReadLine().ToLower();
            Console.ResetColor();
            var timeInterval = Helper.SelectInterval();
            DatabaseFacade db = new DatabaseFacade();
            db.FindLetterByKeyword(keyword, timeInterval);

        }

        /// <summary>
        /// Поиск писем по имейлу 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private static void FindLetterByEmail()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Введите email: ");
            string email = Console.ReadLine().ToLower();
            Console.WriteLine("0 - Выбрать все рассылки\n" +
                "1 - Выбрать по первой рассылки\n" +
                "2 - Выбрать по второй рассылки\n" +
                "3 - Выбрать по третьей рассылки\n");
            Console.Write("Введите номер:");
            int number = 0;
            if(int.TryParse(Console.ReadLine(), out number))
            {
                Console.ResetColor();
                var timeInterval = Helper.SelectInterval();
                DatabaseFacade db = new DatabaseFacade();
                db.FindLettersByEmail(email, number, timeInterval);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Введён неправильный номер задачи");
                Console.ResetColor();
            }

        }    



        /// <summary>
        /// Просмотр инфо о профилях
        /// </summary>
        static void ProfileInformation()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Выберите задачу, которую требуется решить:");
            Console.WriteLine("0 - Отмена");
            Console.WriteLine("1 - Посмотреть информацию по профилям рассылок компании");
            Console.WriteLine("2 - Посмотреть инфо по отдельному профилю");
            Console.WriteLine("3 - Посмотреть информацию о профилях по E-mail");
            Console.Write("Введите номер задачи:");
            int choiseNum = 0;
            bool choiseParseResult = int.TryParse(Console.ReadLine(), out choiseNum);
            Console.ResetColor();
            if (choiseParseResult)
            {
                try
                {
                    switch (choiseNum)
                    {
                        case 0:
                            break;
                        case 1:
                            ShowCompanyProfiles();
                            break;
                        case 2:
                            ShowProfile();
                            break;
                        case 3:
                            ShowProfileByEmail();
                            break;
                        default:
                            break;


                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Показать полную информацию?");
                    if (Helper.Accept())
                        Console.WriteLine(ex.StackTrace);
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Введён неправильный номер задачи");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Редактированить Компанию или профиль
        /// </summary>
        static void EditProfileOrCompane()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Выберите задачу, которую требуется решить:");
            Console.WriteLine("0 - Отмена");
            Console.WriteLine("1 - Отредактировать все профили компании");
            Console.WriteLine("2 - Отредактировать профиль");
            Console.WriteLine("3 - Создать новый профиль");
            Console.WriteLine("4 - Отключить профиль");
            Console.Write("Введите номер задачи:");
            int choiseNum = 0;
            bool choiseParseResult = int.TryParse(Console.ReadLine(), out choiseNum);
            Console.ResetColor();
            if (choiseParseResult)
            {
                try
                {
                    switch (choiseNum)
                    {
                        case 0:
                            break;
                        case 1:
                            EditAllProfiles();
                            break;
                        case 2:
                            EditProfile();
                            break;
                        case 3:
                            CreateNewProfile();
                            break;
                        case 4:
                            DisablePrifile();
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Показать полную информацию?");
                    if (Helper.Accept())
                        Console.WriteLine(ex.StackTrace);
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Введён неправильный номер задачи");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Вывод инфо о конкретном профиле
        /// </summary>
        private static void ShowProfile()
        {
            DatabaseFacade db = new DatabaseFacade();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Запущена процедура вывода информации о профиле");
            Console.Write("Введите номер профиля для редактирования: ");
            string profileIdString = Console.ReadLine();
            int profileId;
            if (!int.TryParse(profileIdString, out profileId))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Неправильно введён ID профиля");
                Console.ResetColor();
                return;
            }

            if (db.CheckProfile(profileId))
            {
                db.ShowProfileByPin(profileId);
            }

            Console.ResetColor();
        }

        /// <summary>
        /// Показать информацию о профилях по E-Mail
        /// </summary>
        private static void ShowProfileByEmail()
        {
            DatabaseFacade db = new DatabaseFacade();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Запущена процедура вывода информации о профилях с текущим e-mail");
            Console.Write("Введите адрес: ");
            string profileEmail = Console.ReadLine();


            db.ShowProfilesByEmail(profileEmail.ToLower());

            Console.ResetColor();
        }

        /// <summary>
        /// Создание новой компании
        /// </summary>
        static void CreateNewCompany()
        {
            DatabaseFacade db = new DatabaseFacade();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Запущена процедура создания новой компании " +
                "с новыми профилями");
            Console.ResetColor();

            Console.Write("Введите внутренний номер (ВН) компании: ");
            int pin = int.Parse(Console.ReadLine());
            if (db.CheckPinNoError(pin))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Компания уже существует в БД. Показать информацию о профилях?");
                Console.ResetColor();
                if (Helper.Accept())
                    db.ShowCompanyInfo(pin);
            }
            else
            {
                TinsCollection tins = new TinsCollection();
                tins.FillNew();
                EmailCollection emails = new EmailCollection();
                emails.FillNew();

                Console.Write("Введите название компании: ");
                string companyName = Console.ReadLine();
                Console.Write("Введите название рассылки: ");
                string profileName = Console.ReadLine();
            SaveData:
                Console.ForegroundColor = ConsoleColor.DarkGray;

                Console.WriteLine("Вы ввели следующие данные:");
                Console.WriteLine($"Внутренний номер (ВН) компании: {pin}");
                Console.WriteLine($"Название компании: {companyName}");
                Console.WriteLine($"Название рассылки: {profileName}");
                tins.Description();
                emails.Description();
                Console.ResetColor();

                Console.Write("Если всё верно, чтобы сохранить введите y :");
                if (Console.ReadLine() == "y")
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Начинаем запись нового профиля...");

                    db.AddCompany(pin, emails, tins, companyName, profileName);


                    Console.WriteLine("Запись нового профиля закончена");
                    Console.ResetColor();
                }
                else
                {
                    Console.Write("Введённые данные будут удалены! " +
                        "Продолжить?");
                    if (!Helper.Accept())
                        goto SaveData;

                }

            }
        }


        
        /// <summary>
        /// Создание нового профиля
        /// </summary>
        private static void CreateNewProfile()
        {
            DatabaseFacade db = new DatabaseFacade();
            Console.ResetColor();
            Console.Write("Введите внутренний номер (ВН) искомой компании: ");
            int pin = int.Parse(Console.ReadLine());

            if (!db.CheckPin(pin))
            {

                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.WriteLine("Введите номер добавляемой рассылки:");           
            Console.WriteLine("1 - Регистрация ТСД на ВХ");
            Console.WriteLine("2 - Информация о ТС");
            Console.WriteLine("3 - Сканы документов");
            Console.WriteLine("4 - Все рассылки");

            Console.Write("Введите номер рассылки: ");
            int subscribe = int.Parse(Console.ReadLine());
            TinsCollection tins = new TinsCollection();
            tins.FillNew();
            EmailCollection emails = new EmailCollection();
            emails.FillNew();
            Console.Write("Введите название рассылки: ");
            string profileName = Console.ReadLine();
            db.AddProfile(pin, emails, tins, profileName, subscribe);
        }


        /// <summary>
        /// Отключкение профиля
        /// </summary>
        private static void DisablePrifile()
        {
            DatabaseFacade db = new DatabaseFacade();
            Console.ResetColor();
            Console.Write("Введите номер профиля для удаления: ");
            int pin = 0;
            if (int.TryParse(Console.ReadLine(), out pin))
            {
                db.DisableProfile(pin);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Неправильно введён ID профиля");
                Console.ResetColor();
                return;
            }
        }

        /// <summary>
        /// Редактировать все профили 
        /// </summary>
        private static void EditAllProfiles()
        {
            DatabaseFacade db = new DatabaseFacade();
            Console.ResetColor();
            Console.Write("Введите внутренний номер (ВН) искомой компании: ");
            int pin = int.Parse(Console.ReadLine());

            if (!db.CheckPin(pin))
            {

                return;
            }



            TinsCollection tins = new TinsCollection();
            //  tins.FillNew();
            //  tins.FillDel();
            EmailCollection emails = new EmailCollection();
            emails.FillNew();
            emails.FillDel();
            db.EditAllProfiles(pin, emails, tins);
        }

        /// <summary>
        /// Редактирование профиля
        /// </summary>
        static void EditProfile()
        {
            DatabaseFacade db = new DatabaseFacade();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Запущена процедура редактирования профиля");
            Console.Write("Введите номер профиля для редактирования: ");
            string profileIdString = Console.ReadLine();
            int profileId;
            if (!int.TryParse(profileIdString, out profileId))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Неправильно введён ID профиля");
                Console.ResetColor();
                return;
            }

            if (db.CheckProfile(profileId))
            {

                TinsCollection tins = new TinsCollection();
                //  tins.FillNew();
                //  tins.FillDel();
                EmailCollection emails = new EmailCollection();
                emails.FillNew();
                emails.FillDel();


                db.EditProfile(profileId, emails, tins);
            }

            Console.ResetColor();
        }


        /// <summary>
        /// Детализация информации по внутреннему номеру (ВН) компании
        /// </summary>
        static void ShowCompanyProfiles()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Запущена процедура просмотра профилей компании");
            Console.ResetColor();
            Console.Write("Введите внутренний номер (ВН) искомой компании: ");
            int pin = int.Parse(Console.ReadLine());
            DatabaseFacade db = new DatabaseFacade();
            if (!db.CheckPin(pin))
                return;
            db.ShowCompanyInfo(pin);


            Console.ResetColor();
        }


        /// <summary>
        /// Создание списка сущностей.
        /// </summary>
        /// <param name="empty"></param>
        /// <returns></returns>
        public static List<string> CreateListEmpties(string empty)
        {
            List<string> empties = new List<string>();
            while (true)
            {
                Console.Write($"{empty}: ");
                string item = Console.ReadLine();
                if (string.IsNullOrEmpty(item))
                {
                    return empties;
                }
                else
                {
                    empties.Add(item.ToLower());
                }
            }
        }

        /// <summary>
        /// Перезапись списка email адресов с нуля по всем профилям 
        /// внутреннего номера (ВН) компании.
        /// </summary>
        /// <param name="Pin"></param>
        /// <param name="emails"></param>
        static void ChangeEmails(int Pin, List<string> emails)
        {
            using (newProfilesContext context = new newProfilesContext())
            {
                var profiles = context.MailingProfiles.Where(p => p.Pin == Pin);
                foreach (var p in profiles)
                {
                    p.Contacts.Clear();
                    foreach (string e in emails)
                    {
                        var contact = context.Contacts.FirstOrDefault(c => c.Contact == e);
                        if (contact == null)
                        {
                            p.Contacts.Add(new Contacts { Contact = e });
                        }
                        else
                        {
                            p.Contacts.Add(contact);
                        }
                    }
                }
                context.SaveChanges();
            }
        }
        #region перенос старых рассылок
        /// <summary>
        /// цикл переноса старых рассылок в новую бд по 1 шт за раз
        /// </summary>
        private static void addOldToNewCycle()
        {
            while (true)
            {
                var oldProfiles = getOldProfilesFromDB();
                var oldProfile = oldProfiles.First();
                rename(oldProfile);
                File.WriteAllText("deactivated.txt", oldProfile.ProfileID.ToString());
                AddoldToNew(oldProfile);
                deactivate(oldProfile.ProfileID);
                Console.WriteLine($"перенес {oldProfile.ProfileID} {oldProfile.ProfileName}");
                Console.ReadKey();
            }
        }
        /// <summary>
        /// переименование рассылок
        /// </summary>
        /// <param name="p"></param>
        static void rename(MailingProfile p)
        {
            p.ProfileName = p.ProfileName.Replace("TC", " информация о ТС");
            p.ProfileName = p.ProfileName.Replace("TS", " информация о ТС");
            p.ProfileName = p.ProfileName.Replace("ts", " информация о ТС");
            p.ProfileName = p.ProfileName.Replace("tc", " информация о ТС");
            p.ProfileName = p.ProfileName.Replace("TC", " информация о ТС");
        }
        /// <summary>
        /// Запись в новую бд данных на основе старой рассылки
        /// </summary>
        /// <param name="oldProfile"></param>
        static void AddoldToNew(MailingProfile oldProfile)
        {
            using (newProfilesContext context = new newProfilesContext())
            {

                if (context.Companies.FirstOrDefault(c => c.Pin == oldProfile.PIN) == null)
                {
                    Companies comp = new Companies()
                    {
                        Name = oldProfile.CompanyName,
                        Pin = oldProfile.PIN
                    };
                    context.Companies.Add(comp);
                }
                List<Contacts> contacts = new List<Contacts>();

                foreach (string email in oldProfile.Emails)
                {
                    var contact = context.Contacts.FirstOrDefault(c => c.Contact == email);
                    if (contact == null)
                        contact = new Contacts { Contact = email };
                    contacts.Add(contact);
                }

                var events = context.Events.Where(e => e.TypeID == 2).ToList();
                Models.MailingProfiles newPr = new Models.MailingProfiles
                {
                    ChangedBy = "Vlad",
                    ContactType = "email",
                    TypeID = 2,
                    CustomsUsage = true,
                    TinsUsage = false,
                    Pin = oldProfile.PIN,
                    ProfileName = oldProfile.ProfileName,
                    Templates = context.Templates.First(t => t.TemplateID == 2),
                    Contacts = contacts,
                    IsActive = true,
                    LastChangeTime = DateTime.Now,
                    Events = events
                };
                context.MailingProfiles.Add(newPr);
                context.SaveChanges();
            }
        }
        /// <summary>
        /// Выключение старой рассылки в старой бд
        /// </summary>
        /// <param name="id"></param>
        static void deactivate(int id)
        {
            string connStr = ConfigurationManager.ConnectionStrings["Alta"].ConnectionString;
            using (var sqlConnection = new SqlConnection(connStr))
            {
                sqlConnection.Open();
                using (var command = new SqlCommand(
                     $@"update
 [NotificationClients].[dbo].[Profiles] 
 set IsActive = 0 where id = {id} ", sqlConnection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        #endregion
        static List<MailingProfile> getOldProfilesFromDB()
        {
            int innerId = 1;
            List<MailingProfile> result = new List<MailingProfile>();
            string connStr = ConfigurationManager.ConnectionStrings["Alta"].ConnectionString;
            using (var sqlConnection = new SqlConnection(connStr))
            {
                sqlConnection.Open();
                using (var sqlQuery = new SqlCommand(
                    $@"SELECT p.id, p.name, pin.PIN, l.Name
  FROM [NotificationClients].[dbo].[Profiles] p 
  left join NotificationClients.dbo.Users_Logins ul on ul.id_User = p.id_User
  left join NotificationClients.dbo.Logins l on ul.id_Login = l.id
  left join NotificationClients.dbo.Users_Pins pin on p.id_User = pin.id_User
  where IsActive = 1 and IsDeleted = 0 and id_AddressType = 1 and p.name is not null
  and p.SendAllTinsExcludingList = 1 and p.SendAllCustomsExcludingList = 1 and pin.PIN != '*'
  and MailingType = 1 and pin.PIN != '1290' and pin.PIN != '889'
  order by p.id desc ", sqlConnection))
                {
                    using (SqlDataReader sqlQueryResult = sqlQuery.ExecuteReader())
                    {
                        while (sqlQueryResult.Read())
                        {
                            int prId = sqlQueryResult.GetInt32(0);
                            string name = sqlQueryResult.GetString(1);
                            string PIN = sqlQueryResult.GetString(2);
                            string comName = sqlQueryResult.GetString(3);

                            if (!Int32.TryParse(PIN, out int parsed))
                                continue;
                            MailingProfile pr = new MailingProfile
                            {
                                InnerID = innerId,
                                CompanyName = comName,
                                PIN = parsed,
                                ProfileID = prId,
                                ProfileName = name
                            };
                            result.Add(pr);
                            innerId++;
                        }
                    }
                }
            }

            foreach (var profile in result)
            {
                using (var sqlConnection = new SqlConnection(connStr))
                {
                    sqlConnection.Open();
                    using (var sqlQuery = new SqlCommand(
                        $@"SELECT Address
  FROM [NotificationClients].[dbo].[Addresses] 
  where id_Profile = {profile.ProfileID}", sqlConnection))
                    {
                        using (SqlDataReader sqlQueryResult = sqlQuery.ExecuteReader())
                        {
                            while (sqlQueryResult.Read())
                            {
                                profile.Emails.Add(sqlQueryResult.GetString(0));
                            }
                        }
                    }
                }
            }
            return result;
        }

        static Settings getSimensProfilesFromDB()
        {
            List<SimensDepartment> result = new List<SimensDepartment>();
            List<dbData> data = new List<dbData>();
            string connStr = ConfigurationManager.ConnectionStrings["Alta"].ConnectionString;
            using (var sqlConnection = new SqlConnection(connStr))
            {
                sqlConnection.Open();
                using (var sqlQuery = new SqlCommand(
                    $@"SELECT ru.armId, u.Name, a.Address
  FROM [NotificationClients].[dbo].[Recipients_Users] ru
  left join NotificationClients.dbo.Users u on u.id = ru.id_User
  left join NotificationClients.dbo.Profiles p on p.id_User = u.id
  left join NotificationClients.dbo.Addresses a on a.id_Profile = p.id
  where p.MailingType = 4
  and armId in (
 '0102268367',  '0102268377', '0102268378' ,'0102268379','0102268387', '0102268391','0102268392', '0102279065', '0102279066' , '0102279067','0102279068','0102279069','0102279070','0102279071','0102288988', '0102302942',
 '0102303562', '0102590','010260209','0102281151')", sqlConnection))
                {
                    using (SqlDataReader sqlQueryResult = sqlQuery.ExecuteReader())
                    {
                        while (sqlQueryResult.Read())
                        {
                            string armid = sqlQueryResult.GetString(0);
                            string depName = sqlQueryResult.GetString(1);
                            string addr = sqlQueryResult.GetString(2);
                            data.Add(new dbData { ArmID = armid, Email = addr, Name = depName });
                        }
                    }
                }
            }
            foreach (var rawdata in data)
            {
                SimensDepartment inList = result.FirstOrDefault(d => d.ArmID == rawdata.ArmID);
                if (inList != null)
                    inList.Emails.Add(rawdata.Email);
                else
                    result.Add(new SimensDepartment
                    {
                        ArmID = rawdata.ArmID,
                        Name = rawdata.Name,
                        Emails = new List<string> { rawdata.Email }
                    });
            }

            Settings s = new Settings();
            s.DeclarantEmails = new List<string>();
            s.Departments = result;
            s.DHLMails = new List<string>();
            s.FilesFolder = "SimensDocsSenderFiles";
            s.TimerDelay = 90000;
            return s;
        }

       
    }
}