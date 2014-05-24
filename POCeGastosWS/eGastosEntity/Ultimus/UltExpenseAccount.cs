using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eGastosEntity.Ultimus
{
    public class UltExpenseAccount
    {
        public bool charged { get; set; }
        public bool creditCard { get; set; }
        public int idExpenseAccount { get; set; }
        public int idRequest { get; set; }
        public string nationalManagerLogin { get; set; }
        public string nationalManagerName { get; set; }
        public bool overdue { get; set; }
        public double totalMeal { get; set; }
        public double totalMiniEvent { get; set; }
        public double totalNationalMeal { get; set; }
        public bool isCFDI { get; set; }
        public bool strike { get; set; }
    }
}
