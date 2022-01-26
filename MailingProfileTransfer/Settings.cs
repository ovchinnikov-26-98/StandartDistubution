using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailingProfileTransfer
{
    public class Settings
    {
        /// <summary>
        /// Рабочая папка модуля
        /// </summary>
        public string FilesFolder;
        /// <summary>
        /// Время в мс через которое проверяется БД
        /// </summary>
        public int TimerDelay;
        public List<string> DeclarantEmails;
        public List<string> DHLMails;
        public List<SimensDepartment> Departments;
        const int minTimerDelay = 9000;
    }
}
