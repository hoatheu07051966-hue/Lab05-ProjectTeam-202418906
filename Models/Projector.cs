/****************
Mã sinh viên: 202418906
Họ tên: Lê Gia Huân
****************/

namespace LabW05a.Models
{
    /// <summary>
    /// Máy chiếu: phiên bản hiện tại không có yêu cầu kết nối mạng.
    /// </summary>
    public class Projector : Device
    {
        private int _brightnessLumens;
        private int _lampHours;

        public Projector(string id, string name, int yearInService, decimal purchasePrice,
                         int brightnessLumens, int lampHours,
                         DeviceStatus status = DeviceStatus.Active)
            : base(id, name, yearInService, purchasePrice, status)
        {
            BrightnessLumens = brightnessLumens;
            LampHours = lampHours;
        }

        /// <summary>Độ sáng (lumen), phải lớn hơn 0.</summary>
        public int BrightnessLumens
        {
            get => _brightnessLumens;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Độ sáng phải lớn hơn 0.");
                _brightnessLumens = value;
            }
        }

        /// <summary>Số giờ đã sử dụng bóng đèn (không âm).</summary>
        public int LampHours
        {
            get => _lampHours;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Số giờ sử dụng bóng đèn không được âm.");
                _lampHours = value;
            }
        }

        public override string DeviceType => "Máy chiếu";

        /// <summary>
        /// 3% giá mua; +1.500.000 nếu bóng đèn đã dùng trên 3.000 giờ.
        /// </summary>
        public override decimal CalculateAnnualMaintenanceCost()
        {
            decimal cost = PurchasePrice * 0.03m;
            if (LampHours > 3_000) cost += 1_500_000m;
            return cost;
        }

        public override string ToString()
        {
            return base.ToString() + $" | Độ sáng: {BrightnessLumens} lm, Bóng đèn: {LampHours:N0} giờ";
        }
    }
}
