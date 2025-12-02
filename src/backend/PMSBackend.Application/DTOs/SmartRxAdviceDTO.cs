namespace PMSBackend.Application.DTOs
{
    public class SmartRxAdviceDTO
    {
        public long SmartRxMasterId { get; set; }
        public long PrescriptionId { get; set; }
        public string Advice { get; set; }
        public string AdviceKeywordToRecommend { get; set; }
    }
}
