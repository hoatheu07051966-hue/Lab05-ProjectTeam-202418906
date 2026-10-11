/****************
Mã sinh viên: 202418906
Họ tên: Lê Gia Huân
****************/

namespace LabW05a.Models
{
    /// <summary>
    /// Trạng thái hoạt động của thiết bị.
    /// </summary>
    public enum DeviceStatus
    {
        Active,            // Đang hoạt động
        UnderMaintenance,  // Đang bảo trì
        Retired            // Đã ngừng sử dụng
    }

    /// <summary>
    /// Loại máy in: laser hoặc phun.
    /// </summary>
    public enum PrinterType
    {
        Laser,
        Inkjet
    }
}
