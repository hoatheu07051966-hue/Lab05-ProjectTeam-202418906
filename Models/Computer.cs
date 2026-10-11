/****************
Mã sinh viên: 202418906
Họ tên: Lê Gia Huân
****************/

namespace LabW05a.Models
{
    /// <summary>
    /// Máy tính: kế thừa Device và luôn có khả năng kết nối mạng (thực thi INetworkable).
    /// </summary>
    public class Computer : Device, INetworkable
    {
        private int _ramGb;
        private string _processor = string.Empty;
        private readonly NetworkConnection _network = new NetworkConnection();

        public Computer(string id, string name, int yearInService, decimal purchasePrice,
                        int ramGb, string processor, bool hasDedicatedGpu,
                        DeviceStatus status = DeviceStatus.Active)
            : base(id, name, yearInService, purchasePrice, status)
        {
            RamGb = ramGb;
            Processor = processor;
            HasDedicatedGpu = hasDedicatedGpu;
        }

        /// <summary>Dung lượng RAM (GB), phải lớn hơn 0.</summary>
        public int RamGb
        {
            get => _ramGb;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Dung lượng RAM phải lớn hơn 0.");
                _ramGb = value;
            }
        }

        /// <summary>Loại bộ xử lý.</summary>
        public string Processor
        {
            get => _processor;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Loại bộ xử lý không được rỗng.");
                _processor = value.Trim();
            }
        }

        /// <summary>Có GPU rời hay không.</summary>
        public bool HasDedicatedGpu { get; set; }

        public override string DeviceType => "Máy tính";

        /// <summary>
        /// 5% giá mua; +2% nếu có GPU rời; +1% nếu đã dùng trên 5 năm.
        /// </summary>
        public override decimal CalculateAnnualMaintenanceCost()
        {
            decimal rate = 0.05m;
            if (HasDedicatedGpu) rate += 0.02m;
            if (YearsInUse > 5) rate += 0.01m;
            return PurchasePrice * rate;
        }

        // ===== Thực thi INetworkable (ủy quyền cho NetworkConnection) =====
        public string IpAddress => _network.IpAddress;
        public bool IsConnected => _network.IsConnected;
        public void Connect(string ipAddress) => _network.Connect(ipAddress);
        public void Disconnect() => _network.Disconnect();

        public override string ToString()
        {
            return base.ToString() +
                   $" | RAM: {RamGb}GB, CPU: {Processor}, GPU rời: {(HasDedicatedGpu ? "Có" : "Không")}";
        }
    }
}
