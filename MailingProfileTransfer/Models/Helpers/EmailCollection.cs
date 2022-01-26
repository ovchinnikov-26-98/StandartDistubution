using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailingProfileTransfer
{
    [Serializable()]
    public class EmailCollection : ListCollection
    {
        /// <summary>
        /// Класс, собирающий и хранящий от пользователя почтовые адреса 
        /// </summary>


        public EmailCollection() : base()
        {
            
        }

        public override string AddMessage => "Добавить адреса для рассылок:";

        public override string DelMessage => "Удалить адреса для рассылок. \r\nЕсли надо удалить все адреса для рассылок, введите all: ";

        public override string Subject => "email";

        public override string PlannedMessage => "Список планируемых адресов рассылки:";

        public EmailCollection Clone()
        {
            return base.DeepCopy() as EmailCollection;

        }
    }
}
