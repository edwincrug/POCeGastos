using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eGastosEntity.Ultimus
{
    public class UltItineraryOptionsDetail
    {
        public string airlineFlight { get; set; }
        public string arrival { get; set; }
        public DateTime arrivalDate { get; set; }
        public string departure { get; set; }
        public DateTime departureDate { get; set; }
        public int idItineraryOption { get; set; }
        public int idItineraryOptionsDetail { get; set; }
        public int idMissionOrder { get; set; }
        public double lapseTime { get; set; }
    }
}
