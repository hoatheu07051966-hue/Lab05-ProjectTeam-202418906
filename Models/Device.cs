/****************
Mã sinh viên: 202418906
Họ tên: Lê Gia Huân
****************/

namespace LabW05a.Models
{
    /// <summary>
    /// Lớp trừu tượng biểu diễn thông tin chung của mọi thiết bị.
    /// Không thể tạo trực tiếp một "Device" chung chung vì không biết cách tính chi phí bảo trì.
    /// </summary>
    public abstract class Device
    {
        private string _name = string.Empty;
        private int _yearInService;
        private decimal _purchasePrice;

        protected Device(string id, string name, int yearInService, decimal purchasePrice,
                         DeviceStatus status = DeviceStatus.Active)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Mã thiết bị không được rỗng.", nameof(id));

            Id = id.Trim();
            Name = name;
            YearInService = yearInService;
            PurchasePrice = purchasePrice;
            Status = status;
        }

        /// <summary>Mã thiết bị - chỉ được thiết lập khi khởi tạo (không có setter).</summary>
        public string Id { get; }

        /// <summary>Tên thiết bị (không được rỗng).</summary>
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Tên thiết bị không được rỗng.");
                _name = value.Trim();
            }
        }

        /// <summary>Năm đưa vào sử dụng (không lớn hơn năm hiện tại).</summary>
        public int YearInService
        {
            get => _yearInService;
            set
            {
                if (value > DateTime.Now.Year)
                    throw new ArgumentException($"Năm đưa vào sử dụng ({value}) không được lớn hơn năm hiện tại.");
                _yearInService = value;
            }
        }

        /// <summary>Giá mua (phải lớn hơn 0).</summary>
        public decimal PurchasePrice
        {
            get => _purchasePrice;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Giá mua phải lớn hơn 0.");
                _purchasePrice = value;
            }
        }

        /// <summary>Trạng thái hoạt động.</summary>
        public DeviceStatus Status { get; set; }

        /// <summary>Số năm đã sử dụng tính đến năm hiện tại.</summary>
        public int YearsInUse => DateTime.Now.Year - YearInService;

        /// <summary>
        /// Thiết bị cần bảo trì nếu đang ở trạng thái UnderMaintenance hoặc đã dùng trên 5 năm.
        /// </summary>
        public bool RequiresMaintenance()
        {
            return Status == DeviceStatus.UnderMaintenance || YearsInUse > 5;
        }

        /// <summary>
        /// Chi phí bảo trì dự kiến trong một năm - mỗi loại thiết bị tự cài đặt (đa hình).
        /// </summary>
        public abstract decimal CalculateAnnualMaintenanceCost();

        /// <summary>Tên loại thiết bị dùng khi hiển thị.</summary>
        public abstract string DeviceType { get; }

        public override string ToString()
        {
            return $"[{DeviceType}] {Id} - {Name} | Năm SD: {YearInService} ({YearsInUse} năm) " +
                   $"| Giá: {PurchasePrice:N0} đ | Trạng thái: {Status}";
        }
    }
}
