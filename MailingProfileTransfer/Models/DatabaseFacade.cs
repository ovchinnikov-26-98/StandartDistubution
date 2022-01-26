using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MailingProfileTransfer.Models.Helpers;

namespace MailingProfileTransfer.Models
{
    /// <summary>
    /// Фасад для работы с обеими БД, дабы не было необходимости создавать несколько классов из главного приложения.
    /// </summary>
    public class DatabaseFacade
    {
       
        /// <summary>
        /// Проверка существования ВН. Если компания создана только в одной из БД, во второй создание произойдет автоматически.
        /// </summary>
        /// <param name="pin"></param>
        /// <returns></returns>
        public bool CheckPinNoError(int pin)
        {
            using (newProfilesContext npc = new newProfilesContext())
            using (VBClientsContext vbc = new VBClientsContext())
            {
                Companies n = npc.Companies.Where(x => x.Pin == pin).FirstOrDefault();
                Users v = vbc.Users.Where(x => x.Users_Pins.Select(y => y.PIN).Contains(pin.ToString())).FirstOrDefault();

                if (n != null && v == null)
                    vbc.AddCompany(pin, n.Name);
                
                else if (v != null && n == null)
                    npc.AddCompany(pin, v.Name);
                
                if (n!= null || v != null)
                {
                    ShowCompanyInfoShort(pin);
                    return true;
                }
                else
                {
                    
                    return false;
                }
                
            }

        }

        /// <summary>
        /// Проверка ВН
        /// </summary>
        /// <param name="pin"></param>
        /// <returns></returns>
        public bool CheckPin(int pin)
        {
            bool result = CheckPinNoError(pin);
            if (!result)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Компании с номером {pin} не существует.");
                Console.ResetColor();
            }
            return result;
        }

        /// <summary>
        /// Добавление компании с тремя рассылками. 
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="emails"></param>
        /// <param name="tins"></param>
        /// <param name="companyName"></param>
        /// <param name="profileName"></param>
        public void AddCompany(int pin, EmailCollection emails, TinsCollection tins, string companyName, string profileName)
        {
            using (newProfilesContext npc = new newProfilesContext())
            {
                //!!!!!! КОСТЫЛЬ !!!!!!!!
                List<Events> events = npc.Events.Where(e => e.TypeID == 2).ToList();
                var xxx = npc.AddCompany(pin, companyName);
                xxx.AddProfile(profileName, emails, tins,events);


                npc.SaveChanges();
            }

            using (VBClientsContext vbc = new VBClientsContext())
            {
                var xxx = vbc.AddCompany(pin, companyName);
                
                xxx.AddProfile(profileName, emails, tins,null);
             
                vbc.SaveChanges();

            }

            

        }

        /// <summary>
        /// Отключение профиля
        /// </summary>
        /// <param name="profilePin"></param>
        public void DisableProfile(int profilePin)
        {
            using (newProfilesContext npc = new newProfilesContext())
            {
                var profile = npc.MailingProfiles.Where(x => x.ProfileID == profilePin).FirstOrDefault();
                if (profile != null )
                {
                    Console.WriteLine("Подтвердите удаление профиля:");
                    profile.Description();
                    if (Helper.Accept())
                    {
                        profile.IsActive = false;
                        npc.SaveChanges();
                    }
                }
               


                
            }


        }

        /// <summary>
        /// Добавление новой рассылки. Варианты subscribeNumber:
        /// 1 - Информация о ТС
        /// 2 - Информация о декларации
        /// 3 - Сканы документов
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="emails"></param>
        /// <param name="tins"></param>
        /// <param name="profileName"></param>
        /// <param name="subscribeNumber"></param>
        public void AddProfile(int pin, EmailCollection emails, TinsCollection tins, string profileName, int subscribeNumber)
        {
            switch (subscribeNumber)
            {
                case 1:
                    using(newProfilesContext npc = new newProfilesContext())
                    {
                        var xxx = npc.Companies.Where(x => x.Pin == pin).FirstOrDefault();
                        if (xxx != null)
                        {
                            xxx.AddProfileWH(profileName, emails, tins);
                            npc.SaveChanges();
                        }
                    }
                    break;
                case 2:
                    using (newProfilesContext npc = new newProfilesContext())
                    {
                        var xxx = npc.Companies.Where(x => x.Pin == pin).FirstOrDefault();
                        if (xxx != null)
                        {
                            //!!!!!! КОСТЫЛЬ !!!!!!!!
                            List<Events> events = npc.Events.Where(e => e.TypeID == 2).ToList();
                            xxx.AddProfileTS(profileName, emails, tins, events);
                            npc.SaveChanges();
                        }
                    }
                    break;
                case 3:
                    using (newProfilesContext npc = new newProfilesContext())
                    {
                        var xxx = npc.Companies.Where(x => x.Pin == pin).FirstOrDefault();
                        if (xxx != null)
                        {
                            xxx.AddProfileCmr(profileName, emails, tins);
                            npc.SaveChanges();
                        }
                    }
                    break;
                case 4:
                    using (newProfilesContext npc = new newProfilesContext())
                    {
                        var xxx = npc.Companies.Where(x=> x.Pin == pin).FirstOrDefault();
                        if (xxx != null)
                        {
                            //!!!!!! КОСТЫЛЬ !!!!!!!!
                            List<Events> events = npc.Events.Where(e => e.TypeID == 2).ToList();
                            xxx.AddProfile(profileName, emails, tins, events);

                            npc.SaveChanges();
                        }
                    }
                    break;               
                default:
                    break;
            }

        }      



        /// <summary>
        /// Показать профили по имейлу
        /// </summary>
        /// <param name="profileEmail"></param>
        internal void ShowProfilesByEmail(string profileEmail)
        {
            using (newProfilesContext npc = new newProfilesContext())
            using (VBClientsContext vbc = new VBClientsContext())
            {                
                npc.MailingProfiles.Where(x => x.Contacts.Select(y=> y.Contact.ToLower()).Contains(profileEmail)).ToList().ForEach(z=> z.Description());
                vbc.Profiles.Where(x => x.Addresses.Select(y=> y.Address.ToLower()).Contains(profileEmail) && x.MailingType == 3).ToList().ForEach(z => z.Description());
            }
        }

        /// <summary>
        /// Короткое описание компании
        /// </summary>
        /// <param name="pin"></param>
        public void ShowCompanyInfoShort(int pin)
        {
            ShowCompanyInfo(pin, false);
        }
        /// <summary>
        /// Полное описание компании
        /// </summary>
        /// <param name="pin"></param>
        public void ShowCompanyInfo(int pin)
        {
            ShowCompanyInfo(pin, true);
        }

        private void ShowCompanyInfo(int pin, bool full)
        {
            using (newProfilesContext npc = new newProfilesContext())
            using (VBClientsContext vbc = new VBClientsContext())
            {
                
                var company_npc = npc.Companies.Where(x => x.Pin == pin).FirstOrDefault();
                var company_vbc = vbc.Users.Where(x => x.Users_Pins.Select(y => y.PIN).Contains(pin.ToString())).FirstOrDefault();
                if (company_npc != null)
                {
                    company_npc.Description();
                }   
                
                //Отключил вывод инфы из старой базы
               // else if (company_vbc != null)
               // {
               //     company_vbc.Description();
               // }

                if (full)
                {
                    Console.WriteLine("Профили:");
                    if (company_npc != null)
                        company_npc.ProfilesDescription();

                  //  if (company_vbc != null)
                    //    company_vbc.ProfilesDescription();
                }
            }
        }



        /// <summary>
        /// Редактировать все профили
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="emails"></param>
        /// <param name="tins"></param>
        public void EditAllProfiles(int pin, EmailCollection emails, TinsCollection tins)
        {
            using (newProfilesContext npc = new newProfilesContext())
            {
                
                var pr = npc.MailingProfiles.Where(x=> x.Companies.Pin == pin).ToList();
                foreach (var p in pr)
                    p.Edit(emails, tins);
                npc.SaveChanges();
            }

            using (VBClientsContext vbc = new VBClientsContext())
            {
                var pr = vbc.Profiles.Where(x => x.User.Users_Pins.Select(y => y.PIN).Contains(pin.ToString()));
                foreach (var p in pr.Where(x => x.MailingType == 3))
                    p.Edit(emails, tins);
                vbc.SaveChanges();

            }
        }

        /// <summary>
        /// Редактировать профиль
        /// </summary>
        /// <param name="pinProfile"></param>
        /// <param name="emails"></param>
        /// <param name="tins"></param>
        public void EditProfile(int pinProfile, EmailCollection emails, TinsCollection tins)
        {
            using (newProfilesContext npc = new newProfilesContext())
            using (VBClientsContext vbc = new VBClientsContext())
            {


                var pr_npc = npc.MailingProfiles.Where(x => x.ProfileID == pinProfile).FirstOrDefault();
                var pr_vbc = vbc.Profiles.Where(x => x.id == pinProfile && x.MailingType == 3).FirstOrDefault();

                if (pr_npc != null && pr_vbc != null)
                {
                   // Console.WriteLine("Профиль существует и в новой и в старой базе.");
                    pr_npc.Description();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Чтобы редактировать этот профиль введите 1.");

                    //pr_vbc.Description();
                    //Console.ForegroundColor = ConsoleColor.Yellow;
                    //Console.WriteLine("Чтобы редактировать этот профиль введите 2.");
                    //Console.WriteLine("");
                    Save:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Профиль для редактирования (чтобы отменить операцию введите 0):");
                    int ch = -1; 
                    int.TryParse(Console.ReadLine(),out ch);
                    switch (ch)
                    {
                        case 1:
                            pr_npc.Edit(emails, tins);
                            npc.SaveChanges();
                            break;
                        case 2:
                            pr_vbc.Edit(emails, tins);
                            vbc.SaveChanges();
                            break;
                        case 0:
                            return;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Ошибка выбора.");
                            Console.ResetColor();
                            goto Save;
                    }

                }
                
                
                
                else if (pr_npc != null)
                {
                    pr_npc.Edit(emails, tins);
                    npc.SaveChanges();
                    return;
                }
                else if (pr_vbc != null)
                {
                    pr_vbc.Edit(emails, tins);
                    vbc.SaveChanges();
                    return;
                }

            }

            

        }


        public void ShowProfileByPin(int pinProfile)
        {
            using (newProfilesContext npc = new newProfilesContext())
            using (VBClientsContext vbc = new VBClientsContext())
            {

                var pr_npc = npc.MailingProfiles.Where(x => x.ProfileID == pinProfile).FirstOrDefault();
                //var pr_vbc = vbc.Profiles.Where(x => x.id == pinProfile && x.MailingType == 3).FirstOrDefault();
                
                if (pr_npc != null)
                    pr_npc.Description();
         
                //if (pr_vbc != null)
                //    pr_vbc.Description();
            }
        }

        public bool CheckProfile(int pinProfile)
        {
            using (newProfilesContext npc = new newProfilesContext())
            using (VBClientsContext vbc = new VBClientsContext())
            {
                bool result;
                var pr_npc = npc.MailingProfiles.Where(x => x.ProfileID == pinProfile).FirstOrDefault();
                var pr_vbc = vbc.Profiles.Where(x => x.id == pinProfile && x.MailingType == 3).FirstOrDefault();

                if (pr_npc != null && pr_vbc != null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Профиль с номером {pinProfile.ToString()} существует и в новой и в старой базе.");
                    Console.ResetColor();
                    result = true;
                }
                else if ((pr_npc == null && pr_vbc != null) ||((pr_npc == null && pr_vbc != null)))
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
                return result;
            }
        }

        /// <summary>
        /// Поиск по имейлу
        /// </summary>
        /// <param name="email"></param>
        /// <param name="number"></param>
        public void FindLettersByEmail(string email, int number, TimeInterval timeInterval)
        {
            TableHeader();
            List<MailingLog> ls = contextByEmail(email, number, timeInterval);
            DetailedInfo(ls);

        }

        /// <summary>
        /// Поиск по ключевому слову
        /// </summary>
        /// <param name="keyword"></param>
        public void FindLetterByKeyword(string keyword, TimeInterval timeInterval)
        {
            TableHeader();
            var ls = contextByInterval(timeInterval);
            ls = ls.Where(e => e.Header.ToLower().Contains(keyword) || e.Body.Contains(keyword)).ToList();
            foreach (var item in ls)
            { 
                item.ShortDescription();
            }         
            DetailedInfo(ls);
        }

        /// <summary>
        /// Поиск по ВН
        /// </summary>
        /// <param name="PIN"></param>
        public void FindLetterByPIN(int PIN, TimeInterval timeInterval)
        {            
            TableHeader();
            List<string> n_tir = OracleContext.GetTirFromPin(PIN, timeInterval);            
            List<MailingLog> listOfLetters = contextByInterval(timeInterval);
            List<MailingLog> ls = new List<MailingLog>();
            foreach (var tir in n_tir)
            {
                var list = listOfLetters.Where(e => e.Header.ToLower().Contains(tir.ToLower()) 
                    || e.Body.Contains(tir.ToLower())).ToList();
                foreach (var item in list)
                {
                    item.ShortDescription();
                }
                ls.AddRange(list);
            }
            DetailedInfo(ls);
            
        }

        /// <summary>
        /// Поиск по отправленным письмам за интервал времени
        /// </summary>
        /// <param name="timeInterval"></param>
        public void FindLetterByTimeInterval(TimeInterval timeInterval)
        {
            TableHeader();
            List<MailingLog> listOfLetters = contextByInterval(timeInterval);
            foreach (var item in listOfLetters)
            {
                item.ShortDescription();
            }
            DetailedInfo(listOfLetters);
        }

        /// <summary>
        /// Контекст для поиска писем за интервал времени
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        private List<MailingLog> contextByInterval(TimeInterval timeInterval)
        {
            using (MalingLogContext ml = new MalingLogContext())
            {                
                var ls = ml.MailingLog.Where(e => e.SentTime >= timeInterval.Time_1 
                                       && e.SentTime <= timeInterval.Time_2)
                                      .OrderBy(e => e.SentTime).ToList();
                return ls;
            }
        }

        /// <summary>
        /// Контекст для поиска по имейлу
        /// </summary>
        /// <param name="email"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        private List<MailingLog> contextByEmail(string email, int number, TimeInterval timeInterval)
        {
            using (MalingLogContext ml = new MalingLogContext())
            {                
                var ls = ml.MailingLog.Where(e => e.ToAddress.ToLower()
                                       .Contains(email) && e.SentTime > timeInterval.Time_1 
                                       && e.SentTime <= timeInterval.Time_2)
                                       .OrderBy(e => e.SentTime).ToList();
                if (number != 0)
                {
                    ls = ls.Where(e => e.MailingType == number).ToList();
                }
                foreach (var l in ls) l.ShortDescription();
                return ls;
            }
            
        }

        
        /// <summary>
        /// Вывод подробной информации о письме 
        /// </summary>
        /// <param name="ls"></param>
        private void DetailedInfo(List<MailingLog> ls)
        {
            Console.WriteLine("Показать подробную информацичю?");
            if (Helper.Accept())            
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Пример ввода ");
                Console.WriteLine("Введите Id письма: 1 ");
                Console.WriteLine("Введите Id письма: 1,2,3,..,n");
                Console.WriteLine("Введите Id письма: 1-5");
                Console.Write("Введите номер: ");
                string idRead = Console.ReadLine();
                Console.ResetColor();
                Console.ResetColor();
                if (idRead.Contains("-"))
                {
                    int[] idNumber = idRead.Split('-').Select(e => int.Parse(e)).ToArray();
                    int indexStart = ls.IndexOf(ls.Where(e => e.Id == idNumber[0]).FirstOrDefault());
                    int indexEnd = ls.IndexOf(ls.Where(e => e.Id == idNumber[1]).FirstOrDefault());
                    for (int i = indexStart; i <= indexEnd; i++)
                    {
                        ls[i].Description();
                    }

                }
                else if (idRead.Contains(","))
                {
                    int[] number = idRead.Split(',').Select(e => int.Parse(e)).ToArray();
                    for (int i = 0; i < number.Length; i++)
                    {
                        var fd = ls.Where(e => e.Id == number[i]).FirstOrDefault();
                        if (fd != null) fd.Description();
                    }
                }
                else
                {
                    int id = int.Parse(idRead);
                    var fd = ls.Where(e => e.Id == id).FirstOrDefault();
                    if (fd != null) fd.Description();
                }
            }
        }


        /// <summary>
        /// Заголовок таблицы
        /// </summary>
        private void TableHeader()
        {
            string idMaling = "ID";
            string typeMailing = "ТИП";
            string adressMailing = "АДРЕС";
            string header = "ЗАГОЛОВОК";
            string date = "ДАТА ОТПРАВКИ";
            Console.WriteLine("{0:0} {1,-10} {2,-20} {3,-60} {4,-80}",
                idMaling, typeMailing, adressMailing, header, date);
        }
    }
}
