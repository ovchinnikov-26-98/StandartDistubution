using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailingProfileTransfer.Models.Helpers;

namespace MailingProfileTransfer.Models
{
    /// <summary>
    /// Класс со статическими вспомогательными методами. 
    /// </summary>
    public class Helper
    {
        /// <summary>
        /// Запрашивает у пользователя подтверждение в консоли.
        /// </summary>
        /// <returns></returns>
        public static bool Accept()
        {
            Console.Write("Для подтверждения действия введите y:");
            return Console.ReadLine().ToLower().Trim() == "y";
        }

        /// <summary>
        /// Выбор интервала времени
        /// </summary>
        /// <param name="time_1"></param>
        /// <param name="time_2"></param>
        public static TimeInterval SelectInterval()
        {

            while (true)
            {
                try
                {
                    var timeInterval = new TimeInterval();
                    Console.WriteLine();
                    Console.WriteLine("Выбрать интервал времени отправленных писем ?" +
                        "\n(по умолчанию интервал 24 часа)");
                    if (Helper.Accept())
                    {
                        Console.Write("Введите дату начала интервала (формат dd.MM.yyyy): ");
                        timeInterval.Time_1 = DateTime.ParseExact(Console.ReadLine(), "dd.MM.yyyy",
                            new CultureInfo("ru-RU", false));
                        Console.WriteLine("Оставить сегодняшее число ?");
                        if (!Helper.Accept())
                        {
                            Console.Write("введите дату конца интервала (формат dd.MM.yyyy): ");
                            timeInterval.Time_2 = DateTime.ParseExact(Console.ReadLine(),
                                "dd.MM.yyyy", new CultureInfo("ru-RU", false));
                        }
                        else
                        {
                            timeInterval.Time_2 = DateTime.Now;
                        }
                    }
                    else
                    {
                        timeInterval.Time_1 = DateTime.Now.AddDays(-1);
                        timeInterval.Time_2 = DateTime.Now;
                    }

                    if (timeInterval.Time_2 > timeInterval.Time_1)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"Итервал c {timeInterval.Time_1} по {timeInterval.Time_2}");
                        Console.ResetColor();
                        return timeInterval;                     
                    }
                    else throw new Exception();
                }
                catch (FormatException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Показать полную информацию?");
                    if (Helper.Accept())
                        Console.WriteLine(ex.StackTrace);
                    Console.ResetColor();
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Не может конечная дата быть больше начальной!!!!");
                    Console.ResetColor();
                }

            }
        }
    }
}
