using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace COB.Teams.Provisioning.Entities
{
    public class SimpleGraphUser
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public string userPrincipalName { get; set; }
        public string surname { get; set; }
        public string officeLocation { get; set; }
        public string mail { get; set; }
        public string jobTitle { get; set; }
    }
}
