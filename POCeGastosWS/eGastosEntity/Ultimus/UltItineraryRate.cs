using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eGastosEntity.Ultimus
{
    public class UltItineraryRate
    {
        public int IdItineraryRate { get; set; }
        public int idMissionOrder { get; set; }
        public double IVA { get; set; }
        public int lineStatus { get; set; }
        public string lineStatusName { get; set; }
        public double otherTaxes { get; set; }
        public double realRate { get; set; }
        public double TUA { get; set; }
    }
}
