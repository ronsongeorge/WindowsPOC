using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesLib
{
    public class BU
    {
        public int BUID { get; set; }
        public string BUName { get; set; }
        public string BUDescription { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<Account> BUAccountsList  { get; set; }
    }
}
