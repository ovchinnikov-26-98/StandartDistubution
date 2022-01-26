using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailingProfileTransfer
{
    public abstract class ListCollection
    {
        public bool DeleteAll { get; set; }
        public List<string> newItems;
        public List<string> delItems;

        //Шаблоны сообщений в консоли
        public abstract string AddMessage { get; } //Добавление новых элементов
        public abstract string DelMessage { get;  } //Удаление существующих
        public abstract string Subject { get;  } // ключевое слово при добавлении, по идее переделать бы метод CreateListEmpties
        public abstract string PlannedMessage { get; } //Сообщение при подтверждении изменений

        public ListCollection()
        {
            newItems = new List<string>();
            delItems = new List<string>();
            
        }

        public void FillNew()
        {
            Console.ResetColor();
            Console.WriteLine(AddMessage);
            newItems = Decisions.CreateListEmpties(Subject);
        }

        public void FillDel()
        {
            Console.ResetColor();
            Console.Write(DelMessage);
            if (Console.ReadLine().Trim().ToLower() == "all")
            {
                DeleteAll = true;
                return;
            }
            delItems = Decisions.CreateListEmpties(Subject);
        }

        


     }
}
