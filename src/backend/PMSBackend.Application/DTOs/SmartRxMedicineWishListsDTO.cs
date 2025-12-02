using PMSBackend.Application.CommonServices;

namespace PMSBackend.Application.DTOs
{
    public class SmartRxMedicineWishListsDTO
    {
        public List<SmartRxMedicineWishListDTO>? MedicineWishlist { get; set; }
        public ApiResponseResult? ApiResponseResult { get; set; }

        public bool? IsRewardUpdated { get; set; }
        public string? RewardTitle { get; set; }
        public double? TotalRewardPoints { get; set; }
        public string? RewardMessage { get; set; }
    }
}
