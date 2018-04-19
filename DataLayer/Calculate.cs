using EntitiesLib;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer
{
    public static class Calculate 
    {
        public static decimal CalculateSum(IEnumerable<EmployeeDetails> elements,CalculationType fieldName,CalculationType condition)
        {
            if (condition != CalculationType.None)
                elements=GetDataWithCondition(elements, condition);

            if(fieldName == CalculationType.Revenue)
                return elements.Sum(a=>a.Revenue);
            else if(fieldName == CalculationType.Salary)
                return elements.Sum(a => a.Salary);    
            else
                return 0;
        }

        public static decimal CalculateSumWithVerticalName(IEnumerable<EmployeeDetails> elements, CalculationType fieldName, CalculationType condition, string verticalName)
        {
            elements = elements.Where(a => a.VerticalName == verticalName);

            if (condition != CalculationType.None)
                elements = GetDataWithCondition(elements, condition,verticalName);
            
            if (fieldName == CalculationType.Revenue)
                return elements.Sum(a => a.Revenue);
            else if (fieldName == CalculationType.Salary)
                return elements.Sum(a => a.Salary);
            else
                return 0;
        }

        private static IEnumerable<EmployeeDetails> GetDataWithCondition(IEnumerable<EmployeeDetails> elements, CalculationType condition){

            if (condition == CalculationType.IsOnsite)
                return elements.Where(a => a.IsOnsite);
            else if (condition == CalculationType.IsOffShore)
                return elements.Where(a => !a.IsOnsite);
            else if (condition == CalculationType.IsAccMgmt)
                return elements.Where(a => a.AccountID == 1);
            else if (condition == CalculationType.IsBillable)
                return elements.Where(a => a.IsBillable);
            else if (condition == CalculationType.IsNonBillable)
                return elements.Where(a => !a.IsBillable);
            else
                return new List<EmployeeDetails>();
        }

        private static IEnumerable<EmployeeDetails> GetDataWithCondition(IEnumerable<EmployeeDetails> elements, CalculationType condition,string verticalName)
        {            
            if (condition == CalculationType.IsOnsite)
                return elements.Where(a => a.IsOnsite);
            else if (condition == CalculationType.IsOffShore)
                return elements.Where(a => !a.IsOnsite);
            else if (condition == CalculationType.IsAccMgmt)
                return elements.Where(a => a.AccountID == 1);
            else if (condition == CalculationType.IsBillable)
                return elements.Where(a => a.IsBillable);
            else if (condition == CalculationType.IsNonBillable)
                return elements.Where(a => !a.IsBillable);
            else
                return new List<EmployeeDetails>();
            
        }
        
        public static decimal CalculateGM(decimal firstelement,decimal secondelement,bool isPercent)
        {
            if (isPercent)
                return CalculateGMPercent(firstelement, secondelement);
            else
                return CalculateGM(firstelement, secondelement);
        }

        public static decimal CalculateGM(decimal firstelement,decimal secondelement)
        {
            if (firstelement == 0)
                return 0;

            return ((firstelement - secondelement) / (firstelement));
        }

        public static decimal CalculateGMPercent(decimal firstelement, decimal secondelement)
        {
            if (firstelement == 0)
                return 0;
            else
                return ((firstelement - secondelement) / (firstelement)) * 100;
        }

        public static decimal CalculatePercent(decimal firstelement, decimal secondelement)
        {
            if (secondelement == 0 || secondelement == null)
                return 0;
            else
                return (firstelement/secondelement)*100;
        }

    }
}
