using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eGastosEntity.Ultimus
{
    public class UltApprovalHistory
    {
        public DateTime approveDate { get; set; }
        public string approverLogin { get; set; }
        public string approverName { get; set; }
        public string approveStatus { get; set; }
        public string comments { get; set; }
        public string stepName { get; set; }
        public string userEmail { get; set; }
    }
}
