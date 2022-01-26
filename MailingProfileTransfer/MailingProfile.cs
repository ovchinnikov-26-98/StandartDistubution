using System.Collections.Generic;

namespace MailingProfileTransfer
{
    public class MailingProfile
    {
        public int InnerID { get; set; }
        public int ProfileID { get; set; }
        public string ProfileName { get; set; }
        public int PIN { get; set; }
        public string CompanyName { get; set; }
        public List<string> Emails { get; set; } = new List<string>();
    }
    public class MailingProfiles
    {
        public List<MailingProfile> Profiles { get; set; }
    }
}
