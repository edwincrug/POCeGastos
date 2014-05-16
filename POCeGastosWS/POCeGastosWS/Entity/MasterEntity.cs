using eGastosEntity.Ultimus;

namespace eGastosWS.Entity
{
    public class MasterEntity
    {
        public UltApprovalHistory[] UltApprovalHistory { get; set; }
        public UltApprove UltApprove { get; set; }
        public UltAttachments[] UltAttachments { get; set; }
        public UltExpenseAccount UltExpenseAccount { get; set; }
        public UltExpenseAccountDetail[] UltExpenseAccountDetail { get; set; }
        public UltExpenseFlowVariables UltExpenseFlowVariables { get; set; }
        public UltFlobotVariables UltFlobotVariables { get; set; }
        public UltHotel[] UltHotel { get; set; }
        public UltItinerary[] UltItinerary { get; set; }
        public UltItineraryOptions[] UltItineraryOptions { get; set; }
        public UltItineraryOptionsDetail[] UltItineraryOptionsDetail { get; set; }
        public UltItineraryRate UltItineraryRate { get; set; }
        public UltMissionOrder UltMissionOrder { get; set; }
        public UltPAClient[] UltPAClient { get; set; }
        public UltRequest UltRequest { get; set; }
        public UltRequester UltRequester { get; set; }
        public UltResponsible UltResponsible { get; set; }
        public UltSAPResponse[] UltSAPResponse { get; set; }
    }
}
