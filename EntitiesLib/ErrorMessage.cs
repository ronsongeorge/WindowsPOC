using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesLib
{
    public class ErrorMessage
    {
        public string File_Type { get; set; }
        public string Error_Message { get; set; }
        public string Emplpoyee_Name { get; set; }
        public int Row_Id { get; set; }

        public ErrorMessage(string FileType, string Message, string EmpNm, int RowId)
        {
            this.File_Type = FileType;
            this.Error_Message = Message;
            this.Emplpoyee_Name = EmpNm;
            this.Row_Id = RowId;
        }

        
    }

}
