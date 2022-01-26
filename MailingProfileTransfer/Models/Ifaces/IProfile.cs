using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailingProfileTransfer.Models
{
    /// <summary>
    /// Интерфейс рассылки.
    /// </summary>
    public interface IProfile
    {
        void Description(ConsoleColor color);
        void FillProfile(EmailCollection emails, TinsCollection tins);

        void Edit(EmailCollection email, TinsCollection tins);
    }
}
