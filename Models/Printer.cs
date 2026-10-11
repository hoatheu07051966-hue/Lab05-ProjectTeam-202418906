/****************
Mã sinh viên: 202418906
Họ tên: Lê Gia Huân
****************/

namespace LabW05a.Models
{
    /// <summary>
    /// Máy in thông thường (không có khả năng mạng).
    /// Máy in có mạng được tách thành lớp con NetworkPrinter.
    /// </summary>
    public class Printer : Device
    {
        private int _pagesPrinted;

        public Printer(string id, string name, int yearInService, decimal purchasePrice,
                       PrinterType type, int pagesPrinted, bool isColor,
                       DeviceStatus status = DeviceStatus.Active)
            : base(id, name, yearInService, purchasePrice, status)
        {
            Type = type;
            PagesPrinted = pagesPrinted;
            IsColor = isColor;
        }

        /// <summary>Loại máy in: laser hoặc phun.</summary>
        public PrinterType Type { get; set; }

        /// <summary>Số trang đã in (không âm).</summary>
        public int PagesPrinted
        {
            get => _pagesPrinted;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Số trang đã in không được âm.");
                _pagesPrinted = value;
            }
        }

        /// <summary>
        /// Thuộc tính bổ sung: máy in màu hay không.
        /// Cần thiết vì công thức chi phí có điều kiện "+300.000 nếu là máy in màu",
        /// trong khi loại máy in (laser/phun) không cho biết in màu hay đơn sắc.
        /// </summary>
        public bool IsColor { get; set; }

        /// <summary>Có hỗ trợ mạng hay không - máy in thường thì không.</summary>
        public virtual bool SupportsNetwork => false;

        public override string DeviceType => "Máy in";

        /// <summary>
        /// 4% giá mua; +500.000 nếu đã in trên 100.000 trang; +300.000 nếu là máy in màu.
        /// </summary>
        public override decimal CalculateAnnualMaintenanceCost()
        {
            decimal cost = PurchasePrice * 0.04m;
            if (PagesPrinted > 100_000) cost += 500_000m;
            if (IsColor) cost += 300_000m;
            return cost;
        }

        public override string ToString()
        {
            return base.ToString() +
                   $" | Loại: {Type}, Màu: {(IsColor ? "Có" : "Không")}, Đã in: {PagesPrinted:N0} trang, " +
                   $"Mạng: {(SupportsNetwork ? "Có" : "Không")}";
        }
    }
}
