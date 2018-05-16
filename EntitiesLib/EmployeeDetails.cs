
namespace EntitiesLib
{
    public class EmployeeDetails
    {
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string ManagerName { get; set; }
        public bool IsBillable { get; set; }
        public decimal Salary { get; set; }
        public string VerticalName { get; set; }
        public bool IsOnsite { get; set; }
        public decimal Revenue { get; set; }
        public int AccountID { get; set; }
        public int EmployeeTypeID { get; set; }
        public decimal SeatCost { get; set; }
    }
}
