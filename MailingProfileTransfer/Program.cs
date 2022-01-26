//#define TestOnBogdanovComp

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
/// <summary>
/// временное приложение для переноса профилей рассылок из старой бд в новую
/// </summary>
/// <remarks>
/// Объектная модель приложения завязана на базу данных MessageDistributionSettings.
/// Сама база данных выстроена вокруг 3х сущностей:
/// Список профилей - MailingProfiles, который описывает конкретную рассылку 
/// сообщений;
/// Контакты - Contacts - список адресов, на которые идут рассылки;
/// События - Events - триггеры, которые запускают процессы создания рассылок.
/// </remarks>
namespace MailingProfileTransfer
{

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Задачи:
    /// Добавить работу с неперенесёнными рассылками по сканам в общий алгоритм.
    ///     это рассылки которые формируются на основе БД VBSiteClientsSettings
    /// Подумать, как на основе рассылки отправлять уведомительное письмо, 
    /// например, в клиентский отдел.
    /// Подумать, как просматривать лог изменений в рассылках.
    /// Перенести в net 5 и сделать web-морду данному функционалу.
    /// Продумать механизм отката на исходную, в случае неправильного редактирования.
    /// Сделать механизм для изменения рассылки сразу в нескольких профилях (не привязано к номеру компании)
    /// Добавить возможность сохранять почтовые адреса, удаляя все, которые не входят в множество. Можно также, просто сделать функционал обновления адресов за раз, т.е. когда, просто вводишь список адресов, а система сама разбирает, что надо добавлять, что удалять, а что не трогать.
    /// </remarks>
    class Program
    {
        static void Main(string[] args)
        {

#if TestOnBogdanovComp
            ChoiseConnectionString.ConnectionStringName = "newProfilesContextBogdanovNotebook";
#else
            bool connectionToDb = false;
            while (!connectionToDb)
            {
                if (!ChoiseConnectionString.ChoiseConnection())
                {
                    Console.Write("Попробовать снова подключиться? (y - если да): ");
                    string newConnectToDb = Console.ReadLine();
                    if (newConnectToDb != "y")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Вы не подключились к БД, программа будет закрыта, нажмите Enter");
                        Console.ReadLine();
                        return;
                    }
                }
                else
                {
                    connectionToDb = true;
                }
            }
#endif
            Decisions.Autorization();

            bool isWorking = true;
            while (isWorking)
            {
                Decisions.DecisionChoise();
                Console.Write("Начать решать новую задачу? (y - если да): ");
                if (Console.ReadLine() != "y") isWorking = false;
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Программа будет закрыта, нажмите Enter");

            Console.ReadLine();

            return;
            //список инн компании, если требуется фильтровать по ИНН
         /*   List<string> tin = new List<string> { };//"7724506852" };
            //адреса куда слать письма
            List<string> emails = new List<string> { "logist01@logli.ru" };//{ "vorontsova@eltransplus.ru", "verkhovets@eltransplus.ru", "samokar@eltransplus.ru", "Anna.Grishpitenko@bosch-climate.ru" };
            //ВН
            int pin = 2136;
            //название компании 
            string name = "Логли";//"7724506852 Бош Термотехника";
            //название рассылки (может отличаться если по ВН брокер, а рассылка на клиента брокера)
            string prName = "Логли";//"7724506852 Бош Термотехника";
            //добавляет рассылку ТСД на ВХ и рассылку о ТС 
            Decisions.AddNewCompany(pin, name, prName, emails, tin);
            //смена почтовых адресов рассылок
            //ChangeEmails(2000, new List<string> { });
            Console.WriteLine("Создание/изменение закончено. Нажмите Enter, чтобы выйти");
            Console.ReadLine();*/
        
        }

        static void Colors()
        {
            Action<ConsoleColor> dc = x =>
            {
                Console.ForegroundColor = x;
                Console.WriteLine(x.ToString());
            };

            Action<ConsoleColor> bc = x =>
            {
                Console.BackgroundColor = x;
                Console.WriteLine(x.ToString());
            };

            dc(ConsoleColor.Cyan);
            dc(ConsoleColor.DarkBlue);
            dc(ConsoleColor.DarkCyan);
            dc(ConsoleColor.DarkGreen);
            dc(ConsoleColor.DarkMagenta);
            dc(ConsoleColor.DarkRed);
            dc(ConsoleColor.DarkYellow);
            dc(ConsoleColor.Magenta);
            Console.ResetColor();
            bc(ConsoleColor.Cyan);
            bc(ConsoleColor.DarkBlue);
            bc(ConsoleColor.DarkCyan);
            bc(ConsoleColor.DarkGreen);
            bc(ConsoleColor.DarkMagenta);
            bc(ConsoleColor.DarkRed);
            bc(ConsoleColor.DarkYellow);
            bc(ConsoleColor.Magenta);
        }
    }
}