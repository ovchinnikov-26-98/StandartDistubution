namespace MailingProfileTransfer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("MailingLog")]
    public partial class MailingLog
    {
        public int Id { get; set; }

        [Required]
        public string ToAddress { get; set; }

        public string Header { get; set; }

        public string Body { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? SentTime { get; set; }

       
        /// <summary>
        /// ��������� ������ ��������
        /// </summary>
        [NotMapped]
        public int MailingType => GetMailingType();

        /// <summary>
        /// �������� ���� ��������
        /// </summary>
        /// <returns></returns>
        private int GetMailingType()// int
        {                        
            if (ChekForFirstMailing()) return 1;
            else if (ChekForSecondMailing()) return 2;
            else  if (ChekForThirdMailing()) return 3;
            else return 999;
        }


        /// <summary>
        /// �������� �� ������ ��� ��������
        /// </summary>
        /// <returns></returns>
        private bool ChekForThirdMailing()
        {
            string fraza = "���� ��������";
            bool res = Body.ToLower().Contains(fraza.ToLower());
            return res;          
           
        }

        /// <summary>
        /// �������� �� ������ ��� �������� 
        /// </summary>
        /// <returns></returns>
        private bool ChekForSecondMailing()
        {
            bool res = false;
            List<string> fraza = new List<string> { "����� ������", "����������� ��������", "���������� � �����������", "������ ��������������" };
            for (int i = 0; i < fraza.Count; i++)
            {
                res = Body.Contains(fraza[i]) || Header.Contains(fraza[i]);
                if (res) break;
            }               
            return res;
        }

        /// <summary>
        /// �������� �� ������ ��� ��������
        /// </summary>
        /// <returns></returns>
        private bool ChekForFirstMailing()
        {
            string fraza = "��������� ��������";
            bool res = Body.ToLower().Contains(fraza.ToLower());
            return res;

        }

        public void Description()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"ID ������: \n{Id}\n");
            Console.WriteLine($"������ ��������: \n{ToAddress}\n");
            Console.WriteLine($"����� ��������: \n{SentTime}\n");
            Console.WriteLine($"���������: \n{Header}\n");
            Console.WriteLine($"���� ������: \n{Body}\n");
            Console.ResetColor();
            
        }

        public void ShortDescription()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            if (ToAddress.Length > 40) Console.WriteLine("{0:0} {1,0} {2,-10} {3,-50} {4,-70}", 
                Id, MailingType, ToAddress.Substring(0, 30), Header, SentTime);
            else Console.WriteLine("{0:0} {1,0} {2,-10} {3,-50} {4,-70}",
                Id, MailingType, ToAddress.Substring(0, ToAddress.Length), Header, SentTime);
            Console.ResetColor();
        }        

        

    }
}
