using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailingProfileTransfer
{
    public class Helper
    {
        public static bool AcceptChanges()
        {
            Console.Write("Подтвердите изменения (введите y - если да): ");
            return Accept();
        }

        public static bool Accept()
        {
            return (Console.ReadLine().ToLower().Trim() == "y");
        }

    }
}
