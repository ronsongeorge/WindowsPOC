using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesLib
{
    public class AccountMonthData
    {
        public string MonthName { get; set; }
        public int Year { get; set; }
        public string AccountID { get; set; }
        public List<AccountVerticalData> AccMonthData { get; set; }
        public List<EmployeeDetails> CostAndRevenueData { get; set; }
    }
}
