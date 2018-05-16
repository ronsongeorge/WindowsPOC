using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesLib
{
    public class Account
    {
        public int AccountID { get; set; }
        public string AccountName { get; set; }
        public string AccountDescription { get; set; }
        public DateTime CreatedOn { get; set; }
        public int BUID { get; set; }
    }
}
