using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eGastosEntity.Ultimus
{
    public class UltItinerary
    {
        public int idItinerary { get; set; }
        public int idMissionOrder { get; set; }
        public int idConsecutive { get; set; }
        public int idLedgerAccount { get; set; }
        public string nameLedgerAccount { get; set; }
        public string departureHour { get; set; }
        public string returnHour { get; set; }
        public string observations { get; set; }
        public int travelType { get; set; }
        public string nameTravelType { get; set; }
        public string departureCountry { get; set; }
        public string departureCity { get; set; }
        public string arrivalCountry { get; set; }
        public string arrivalCity { get; set; }
        public DateTime departureDate { get; set; }
        public DateTime arrivalDate { get; set; }
        public bool status { get; set; }
    }
}
