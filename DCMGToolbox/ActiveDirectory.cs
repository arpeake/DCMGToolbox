using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices;

namespace DCMGToolbox
{
    public class ActiveDirectory
    {
        public string GetDomainName()
        {
            string domainName = String.Empty;

            IPGlobalProperties ipgp = IPGlobalProperties.GetIPGlobalProperties();
            domainName = ipgp.DomainName;

            return domainName;
        }

        public void CreateUser(string username, string firstName, string lastName, string password, string emailAddress = "", bool passNeverExpires = false, bool userCannotChangePW = false, bool changePWOnNextLogon = false)
        {
            using (var pc = new PrincipalContext(ContextType.Domain))
            {                
                using (var up = new UserPrincipal(pc))
                {                    
                    up.SamAccountName = username;
                    up.EmailAddress = emailAddress;
                    up.SetPassword(password);
                    up.Enabled = true;
                    up.PasswordNeverExpires = passNeverExpires;
                    up.UserCannotChangePassword = userCannotChangePW;
                    if (changePWOnNextLogon)
                        up.ExpirePasswordNow();
                    up.Save();
                }
            }
        }

        public string GetDomainDN(string domain)
        {
            string DN = String.Empty;

            DirectoryContext context = new DirectoryContext(DirectoryContextType.Domain, domain);
            Domain d = Domain.GetDomain(context);
            DirectoryEntry de = d.GetDirectoryEntry();
            return de.Properties["DistinguishedName"].Value.ToString();
        }

        public List<string> GetOUList(string domain)
        {
            List<string> OUs = new List<string>();
            string dn = GetDomainDN(domain);
            DirectoryEntry startingPoint = new DirectoryEntry("LDAP://DC=YourCompany,DC=com");

            DirectorySearcher searcher = new DirectorySearcher(startingPoint);
            searcher.Filter = "(objectCategory=organizationalUnit)";

            foreach (SearchResult res in searcher.FindAll())
            {
                OUs.Add(res.Path);
            }

            return OUs;
        }
    }
}
