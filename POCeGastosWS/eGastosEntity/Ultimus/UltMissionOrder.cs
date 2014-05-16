using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eGastosEntity.Ultimus
{
    public class UltMissionOrder
    {
        public double advance { get; set; }
        public bool advanceApply { get; set; }
        public string comment { get; set; }
        public int countAgencyWait { get; set; }
        public bool hotel { get; set; }
        public int idAgencyLog { get; set; }
        public int idAgencyResponse { get; set; }
        public int idMissionOrder { get; set; }
        public int idRequest { get; set; }
        public bool itinerary { get; set; }
        public double nationalCurrency { get; set; }
        public string objective { get; set; }
        public int statusAgencyProcess { get; set; }
        public int statusAgencySend { get; set; }
        public string travelId { get; set; }
        public string travelName { get; set; }
    }
}
