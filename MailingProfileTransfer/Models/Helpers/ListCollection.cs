using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MailingProfileTransfer
{
    [Serializable()]
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

        public virtual object DeepCopy()
        {
            object figure = null;
            using (MemoryStream tempStream = new MemoryStream())
            {
                BinaryFormatter binFormatter = new BinaryFormatter(null,
                    new StreamingContext(StreamingContextStates.Clone));

                binFormatter.Serialize(tempStream, this);
                tempStream.Seek(0, SeekOrigin.Begin);

                figure = binFormatter.Deserialize(tempStream);
            }
            return figure;

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

        public void Description()
        {
            Console.ForegroundColor = ConsoleColor.White;
            if (newItems.Any())
            {
                Console.WriteLine($"Список {Subject} для добавления:");
                newItems.ForEach(x => Console.WriteLine($"    {x}"));
            }
            if (delItems.Any())
            {
                Console.WriteLine($"Список {Subject} для удаления:");
                delItems.ForEach(x => Console.WriteLine($"    {x}"));
            }
        }
        


     }
}
