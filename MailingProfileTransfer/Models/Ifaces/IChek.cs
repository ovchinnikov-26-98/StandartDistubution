using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailingProfileTransfer.Models
{
    public interface IChek
    {
       
        string DatabaseName { get; }
        string ServerName { get; }
        bool Exists { get; }

    }

    


}


