using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eGastosEntity.Ultimus
{
    public class UltSAPResponse
    {
        public int company { get; set; }
        public int docNumber { get; set; }
        public int idRequest { get; set; }
        public int idResponse { get; set; }
        public string type { get; set; }
        public int year { get; set; }
    }
}
