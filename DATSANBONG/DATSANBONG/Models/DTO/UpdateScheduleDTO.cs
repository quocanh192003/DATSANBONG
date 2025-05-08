namespace DATSANBONG.Models.DTO
{
    public class UpdateScheduleDTO
    {
        public string? thu { get; set; }
        public TimeSpan? GioBatDau { get; set; }
        public TimeSpan? GioKetThuc { get; set; }
        public decimal? GiaThue { get; set; }
        public string? TrangThai { get; set; }
    }
}
