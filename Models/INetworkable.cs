/****************
Mã sinh viên: 202418906
Họ tên: Lê Gia Huân
****************/

namespace LabW05a.Models
{
    /// <summary>
    /// Năng lực kết nối mạng. Đây là một "khả năng" độc lập với cây kế thừa Device,
    /// nên được biểu diễn bằng interface thay vì đặt vào lớp cơ sở.
    /// </summary>
    public interface INetworkable
    {
        /// <summary>Địa chỉ IP đang hoạt động (rỗng nếu chưa kết nối).</summary>
        string IpAddress { get; }

        /// <summary>Kết nối thiết bị vào mạng với địa chỉ IP cho trước.</summary>
        void Connect(string ipAddress);

        /// <summary>Ngắt kết nối mạng.</summary>
        void Disconnect();

        /// <summary>Thiết bị có đang kết nối hay không.</summary>
        bool IsConnected { get; }
    }
}
