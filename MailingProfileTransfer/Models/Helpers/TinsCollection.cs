using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailingProfileTransfer
{
    [Serializable()]
    public class TinsCollection : ListCollection
    {
        /// <summary>
        /// Класс, собирающий и хранящий от пользователя ИНН 
        /// </summary>
        public TinsCollection() : base()
        {
           
        }

        public override string AddMessage => "Добавить ИНН, если требуется фильтрация:";

        public override string DelMessage => "Удалить ИНН. \r\nЕсли надо удалить все фильтрации по ИНН, введите all: ";

        public override string Subject => "ИНН";

        public override string PlannedMessage => "Список планируемых адресов рассылки:";

        public TinsCollection Clone()
        {
            return base.DeepCopy() as TinsCollection;
                
        }
    }
}
