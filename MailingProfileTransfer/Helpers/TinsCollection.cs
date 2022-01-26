using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailingProfileTransfer
{
    public class TinsCollection : ListCollection
    {
        public TinsCollection() : base()
        {
            
        }

        public override string AddMessage => "Введите список ИНН компании, если требуется фильтровать по ИНН:";

        public override string DelMessage => "Удалить ИНН из списка. \r\n Если надо удалить все адреса для рассылок, введите all: ";

        public override string Subject => "ИНН";

        public override string PlannedMessage => "Список планируемых адресов рассылки:";
    }
}
