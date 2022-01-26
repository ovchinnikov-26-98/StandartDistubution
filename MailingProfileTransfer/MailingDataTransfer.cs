using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailingProfileTransfer.Models;

namespace MailingProfileTransfer
{
    public class MailingDataTransfer
    {

        public static void Work()
        {
            using (VBClientsContext vbc = new VBClientsContext())
            using (newProfilesContext npc = new newProfilesContext())
            {
                var thirdType = npc.MailingTypes.Where(x => x.TypeID == 3).FirstOrDefault();
                if (thirdType == null)
                {
                    thirdType = new MailingTypes() { TypeID = 3, IsFilterByCustoms = false, Description = "Скан копии сопроводительных документов" };

                }

                var thirdTemplate = npc.Templates.Where(x => x.TypeID == 3).FirstOrDefault();
                if (thirdTemplate == null)
                {
                    thirdTemplate = new Templates() { TypeID = 3, Description = "Письмо со ссылкой на скан документа", MessageSample = "Пример письма со скан-копиями" };

                }

                foreach (var comp in vbc.Users)
                {
                    try
                    {
                        int pin = int.Parse(comp.Users_Pins.First().PIN);
                        var company = npc.Companies.Where(x => x.Pin == pin).FirstOrDefault();
                        if (company == null)
                        {
                            company = new Companies() { Pin = pin, Name = comp.Name };
                            npc.Companies.Add(company);
                            npc.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }


                //Profiles
                foreach (var profileOld in vbc.Profiles.Where(x=> x.MailingType == 3 && x.IsActive.HasValue && x.IsActive.Value))
                {
                    try
                    {
                        if (!int.TryParse(profileOld.User.PIN, out int pin))
                            continue;
                        var profileName = String.IsNullOrEmpty(profileOld.Name) ? profileOld.User.Name + " скан-копии сопроводительных документов" : profileOld.Name;
                        
                        var company = npc.Companies.Where(x => x.Pin == pin).FirstOrDefault();
                        
                          

                        Models.MailingProfiles newProfile = new Models.MailingProfiles()
                        {
                            ProfileName = profileName,
                            Companies = company,
                            MailingTypes = thirdType,
                            ContactType = "email",
                            Templates = thirdTemplate,
                            TinsUsage = !profileOld.SendAllTinsExcludingList.Value,
                            CustomsUsage = true,
                            IsActive = profileOld.IsActive.Value,
                            LastChangeTime = profileOld.ModifiedTime != null ? profileOld.ModifiedTime.Value : DateTime.Now,
                            ChangedBy = "Andrey"
                        };

                        foreach (var email in profileOld.Addresses)
                        {
                            var emailChecked = npc.Contacts.Where(x => x.Contact.ToLower() == email.Address.ToLower()).FirstOrDefault();
                            if (emailChecked == null)
                            {
                                emailChecked = new Contacts()
                                {
                                    Contact = email.Address.ToLower()
                                };

                            }
                            newProfile.Contacts.Add(emailChecked);
                        }

                        foreach (var tit in profileOld.Profiles_Tins)
                        {
                            var tinChecked = npc.Tins.Where(x => x.Tin.Contains(tit.Tin)).FirstOrDefault();
                            if (tinChecked == null)
                            {
                                tinChecked = new Tins()
                                {
                                    Tin = tit.Tin
                                };
                            }

                            newProfile.Tins.Add(tinChecked);
                        }










                        npc.MailingProfiles.Add(newProfile);
                    }
                    catch (Exception ex)
                    {

                    }

                   
                }

                npc.SaveChanges();
            }
        }
    }
}
