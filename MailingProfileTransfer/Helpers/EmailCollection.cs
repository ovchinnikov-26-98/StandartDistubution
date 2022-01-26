using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailingProfileTransfer
{
    public class EmailCollection : ListCollection
    {


        public EmailCollection() : base()
        {
           
        }

        public override string AddMessage => "Добавить адреса для рассылок:";

        public override string DelMessage => "Удалить адреса для рассылок. \r\n Если надо удалить все адреса для рассылок, введите all: ";

        public override string Subject => "email";

        public override string PlannedMessage => "Список планируемых адресов рассылки:";
    }
}
