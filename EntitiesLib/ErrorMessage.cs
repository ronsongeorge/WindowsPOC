﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesLib
{
    public class ErrorMessage
    {
        private string FileType1;
        private string p;

        public string FileType { get; set; }
        public string Message { get; set; }

      public ErrorMessage(string FileType,string Message)
        {
            this.FileType = FileType;
            this.Message = Message;
        }

        
    }

}
