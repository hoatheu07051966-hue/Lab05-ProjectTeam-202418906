/****************
Mã sinh viên: 202418906
Họ tên: Lê Gia Huân
****************/

namespace LabW05a.Models
{
    /// <summary>
    /// Phòng thực hành - KẾT TẬP các thiết bị.
    /// Phòng chỉ giữ tham chiếu tới Device được tạo từ bên ngoài; thiết bị vẫn tồn tại
    /// khi bị gỡ khỏi phòng hoặc khi phòng bị hủy.
    /// </summary>
    public class LabRoom
    {
        private string _name = string.Empty;
        private int _capacity;
        private readonly List<Device> _devices = new List<Device>();

        public LabRoom(string roomId, string name, int capacity)
        {
            if (string.IsNullOrWhiteSpace(roomId))
                throw new ArgumentException("Mã phòng không được rỗng.", nameof(roomId));

            RoomId = roomId.Trim();
            Name = name;
            Capacity = capacity;
        }

        /// <summary>Mã phòng - chỉ thiết lập khi khởi tạo.</summary>
        public string RoomId { get; }

        /// <summary>Tên phòng.</summary>
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Tên phòng không được rỗng.");
                _name = value.Trim();
            }
        }

        /// <summary>Sức chứa (số chỗ ngồi), phải lớn hơn 0.</summary>
        public int Capacity
        {
            get => _capacity;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Sức chứa phải lớn hơn 0.");
                _capacity = value;
            }
        }

        /// <summary>Danh sách thiết bị chỉ đọc, bên ngoài không thể sửa trực tiếp.</summary>
        public IReadOnlyList<Device> Devices => _devices.AsReadOnly();

        /// <summary>
        /// Thêm thiết bị vào phòng. Không nhận null và không nhận mã trùng.
        /// </summary>
        public void AddDevice(Device device)
        {
            if (device == null)
                throw new ArgumentNullException(nameof(device), "Không chấp nhận thiết bị null.");

            if (FindDevice(device.Id) != null)
                throw new InvalidOperationException($"Phòng {RoomId} đã có thiết bị mã '{device.Id}'.");

            _devices.Add(device);
        }

        /// <summary>
        /// Gỡ thiết bị khỏi phòng. Trả về true nếu gỡ thành công.
        /// </summary>
        public bool RemoveDevice(string deviceId)
        {
            Device? device = FindDevice(deviceId);
            return device != null && _devices.Remove(device);
        }

        /// <summary>
        /// Tìm thiết bị theo mã (không phân biệt hoa thường). Trả về null nếu không có.
        /// </summary>
        public Device? FindDevice(string deviceId)
        {
            if (string.IsNullOrWhiteSpace(deviceId))
                return null;

            return _devices.FirstOrDefault(d =>
                string.Equals(d.Id, deviceId.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Tổng chi phí bảo trì dự kiến trong năm.
        /// Dùng đa hình: mỗi Device tự tính chi phí theo lớp của nó, không dùng if/switch theo kiểu.
        /// </summary>
        public decimal CalculateAnnualMaintenanceCost()
        {
            return _devices.Sum(d => d.CalculateAnnualMaintenanceCost());
        }

        /// <summary>
        /// Các thiết bị đang UnderMaintenance hoặc đã dùng trên 5 năm.
        /// </summary>
        public List<Device> GetDevicesRequiringMaintenance()
        {
            return _devices.Where(d => d.RequiresMaintenance()).ToList();
        }

        public override string ToString()
        {
            return $"Phòng {RoomId} - {Name} (sức chứa {Capacity}, {_devices.Count} thiết bị)";
        }
    }
}
