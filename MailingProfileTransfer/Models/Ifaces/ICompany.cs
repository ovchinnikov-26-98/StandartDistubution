using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailingProfileTransfer.Models

{
    /// <summary>
    /// Интерфейс компании.
    /// </summary>
    public interface ICompany
    {
        void Description();

        void AddProfile(string prName, EmailCollection emails, TinsCollection tins, List<Events> events);
    }
}
