using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eGastosEntity.Ultimus
{
    public class UltExpenseAccountDetail
    {
        public string accountName { get; set; }
        public double amount { get; set; }
        public double discount { get; set; }
        public DateTime expenseDate { get; set; }
        public bool hasPAClient { get; set; }
        public bool healthProfessional { get; set; }
        public int idAccount { get; set; }
        public int idExpenseAccount { get; set; }
        public int idExpenseAccountDetail { get; set; }
        public string invoiceNumber { get; set; }
        public double IVA { get; set; }
        public string IVATypeId { get; set; }
        public string IVATypeName { get; set; }
        public int numberOfDiners { get; set; }
        public int observationId { get; set; }
        public string observationName { get; set; }
        public string place { get; set; }
        public bool status { get; set; }
        public double total { get; set; }
    }
}
