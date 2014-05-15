using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eGastosEntity.Ultimus
{
    public class UltHotel
    {
        public string address { get; set; }
        public DateTime checkInDate { get; set; }
        public DateTime checkoutDate { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string hotelName { get; set; }
        public double hotelTax { get; set; }
        public int idConsecutive { get; set; }
        public int idHotel { get; set; }
        public int idLegerAccount { get; set; }
        public int idMissionOrder { get; set; }
        public int idRated { get; set; }
        public double IVA { get; set; }
        public int lineStatus { get; set; }
        public string lineStatusName { get; set; }
        public string nameLegerAccount { get; set; }
        public string observations { get; set; }
        public double otherTaxes { get; set; }
        public double quotedRate { get; set; }
        public double realRate { get; set; }
        public string reservation { get; set; }
        public bool status { get; set; }
        public string telephone { get; set; }
    }
}
