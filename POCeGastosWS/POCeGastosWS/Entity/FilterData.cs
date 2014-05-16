namespace eGastosWS.Entity
{
    public class FilterData
    {
        public string TaskID { get; set; }
        public string UserLogin { get; set; }
        public string ProcessName { get; set; }
        public int IncidentNumber { get; set; }
        public bool isPasteur { get; set; }
        public string StepName { get; set; }
        public string ErrorMessage { get; set; }
    }
}
