using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eGastosEntity.Ultimus
{
    public class UltRequest
    {
        public int areaId { get; set; }
        public string areaText { get; set; }
        public string arrival { get; set; }
        public int CeCoCode { get; set; }
        public int CeCoMiniCode { get; set; }
        public string CeCoMiniName { get; set; }
        public string CeCoName { get; set; }
        public int companyCode { get; set; }
        public string companyName { get; set; }
        public string currencyId { get; set; }
        public string currencyName { get; set; }
        public string departureDate { get; set; }
        public double exchangeRate { get; set; }
        public int idRequest { get; set; }
        public string initiatorLogin { get; set; }
        public string initiatorName { get; set; }
        public bool isMiniEvent { get; set; }
        public string PAClientId { get; set; }
        public string PAClientName { get; set; }
        public bool pasteur { get; set; }
        public string PEPElementId { get; set; }
        public string PEPElementName { get; set; }
        public DateTime requestDate { get; set; }
        public string responsibleEmployeeNum { get; set; }
        public string responsibleLogin { get; set; }
        public string responsibleName { get; set; }
        public string responsiblePayMethod { get; set; }
        public string responsibleUserName { get; set; }
        public string returnDate { get; set; }
        public bool salesForce { get; set; }
        public int status { get; set; }
        public string statusName { get; set; }
        public int type { get; set; }
        public string typeName { get; set; }
        public int ultimusNumber { get; set; }
    }
}
