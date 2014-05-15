using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eGastosEntity.Ultimus
{
    public class UltItineraryOptions
    {
        public bool confirmed { get; set; }
        public int idItineraryOption { get; set; }
        public int idMissionOrder { get; set; }
        public int idRate { get; set; }
        public DateTime lastDayPurchase { get; set; }
        public string observations { get; set; }
        public double quoteRate { get; set; }
    }
}
