using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eGastosEntity.Ultimus
{
    public class UltGetThere
    {
        public int idGetThere { get; set; }
        public int idMissionOrder { get; set; }
        public int conceptId { get; set; }
        public string conceptText { get; set; }
        public bool lowCost { get; set; }
        public string justification { get; set; }
        public string cheapestRate { get; set; }
        public bool outPolitic { get; set; }
        public string outPoliticMessage { get; set; }
    }
}
