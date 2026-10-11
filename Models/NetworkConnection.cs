/****************
Mã sinh viên: 202418906
Họ tên: Lê Gia Huân
****************/

using System.Net;

namespace LabW05a.Models
{
    /// <summary>
    /// Thành phần xử lý trạng thái kết nối mạng, dùng chung cho các lớp thực thi INetworkable
    /// (Computer, NetworkPrinter) để không lặp lại logic kiểm tra điều kiện kết nối.
    /// </summary>
    public class NetworkConnection
    {
        public string IpAddress { get; private set; } = string.Empty;
        public bool IsConnected { get; private set; }

        /// <summary>
        /// Kết nối với địa chỉ IP.
        /// Ném ngoại lệ nếu IP rỗng/sai định dạng hoặc thiết bị đang kết nối.
        /// </summary>
        public void Connect(string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
                throw new ArgumentException("Địa chỉ IP không được rỗng.", nameof(ipAddress));

            if (!IPAddress.TryParse(ipAddress.Trim(), out _))
                throw new ArgumentException($"Địa chỉ IP '{ipAddress}' không hợp lệ.", nameof(ipAddress));

            // Không cho phép kết nối lại khi đang kết nối
            if (IsConnected)
                throw new InvalidOperationException($"Thiết bị đang kết nối với {IpAddress}, hãy ngắt kết nối trước.");

            IpAddress = ipAddress.Trim();
            IsConnected = true;
        }

        /// <summary>
        /// Ngắt kết nối: IsConnected = false và xóa địa chỉ IP đang hoạt động.
        /// </summary>
        public void Disconnect()
        {
            IsConnected = false;
            IpAddress = string.Empty;
        }
    }
}
