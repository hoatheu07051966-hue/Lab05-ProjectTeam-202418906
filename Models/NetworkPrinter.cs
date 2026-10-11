/****************
Mã sinh viên: 202418906
Họ tên: Lê Gia Huân
****************/

namespace LabW05a.Models
{
    /// <summary>
    /// Máy in có khả năng kết nối mạng: kế thừa Printer và thực thi INetworkable.
    /// Biểu diễn yêu cầu "chỉ một số máy in có khả năng mạng".
    /// </summary>
    public class NetworkPrinter : Printer, INetworkable
    {
        private readonly NetworkConnection _network = new NetworkConnection();

        public NetworkPrinter(string id, string name, int yearInService, decimal purchasePrice,
                              PrinterType type, int pagesPrinted, bool isColor,
                              DeviceStatus status = DeviceStatus.Active)
            : base(id, name, yearInService, purchasePrice, type, pagesPrinted, isColor, status)
        {
        }

        public override bool SupportsNetwork => true;

        public override string DeviceType => "Máy in mạng";

        // ===== Thực thi INetworkable (ủy quyền cho NetworkConnection) =====
        public string IpAddress => _network.IpAddress;
        public bool IsConnected => _network.IsConnected;
        public void Connect(string ipAddress) => _network.Connect(ipAddress);
        public void Disconnect() => _network.Disconnect();
    }
}
