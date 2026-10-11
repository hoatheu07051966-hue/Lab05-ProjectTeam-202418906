/****************
Mã sinh viên: 202418906
Họ tên: Lê Gia Huân
****************/

using System.Globalization;
using System.Text;
using LabW05a.Models;

namespace LabW05a
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Hiển thị tiếng Việt và định dạng số kiểu 1.500.000
            Console.OutputEncoding = Encoding.UTF8;
            CultureInfo.CurrentCulture = new CultureInfo("vi-VN");

            // ===== 0. Kiểm tra ràng buộc dữ liệu =====
            PrintTitle("0. KIỂM TRA DỮ LIỆU KHÔNG HỢP LỆ");
            TryAction("Tạo máy tính mã rỗng",
                () => new Computer("", "PC lỗi", 2022, 10_000_000m, 8, "i5", false));
            TryAction("Tạo máy in giá mua = 0",
                () => new Printer("PRX", "Máy in lỗi", 2022, 0m, PrinterType.Laser, 0, false));
            TryAction($"Tạo máy chiếu năm {DateTime.Now.Year + 1}",
                () => new Projector("PJX", "Máy chiếu lỗi", DateTime.Now.Year + 1, 5_000_000m, 3000, 0));

            // ===== Tạo thiết bị (tồn tại độc lập, chưa thuộc phòng nào) =====
            var pc1 = new Computer("PC01", "Lenovo ThinkStation P340", 2024, 32_000_000m, 32, "Intel Core i9", hasDedicatedGpu: true);
            var pc2 = new Computer("PC02", "Acer Veriton X2690", 2018, 12_000_000m, 8, "Intel Core i3", hasDedicatedGpu: false);
            var pr1 = new Printer("PR01", "Brother HL-L2321D", 2023, 3_000_000m, PrinterType.Laser, 8_000, isColor: false);
            var pr2 = new NetworkPrinter("PR02", "Canon G3770", 2022, 6_000_000m, PrinterType.Inkjet, 150_000, isColor: true);
            var pj1 = new Projector("PJ01", "Sony VPL-DX221", 2019, 14_000_000m, 3_200, 4_200, DeviceStatus.UnderMaintenance);

            List<Device> allDevices = new List<Device> { pc1, pc2, pr1, pr2, pj1 };

            var roomA = new LabRoom("P305", "Phòng Lab Trí tuệ nhân tạo", 25);
            var roomB = new LabRoom("P112", "Phòng Họp trực tuyến", 20);
            List<LabRoom> rooms = new List<LabRoom> { roomA, roomB };

            // ===== 1. Thêm thiết bị vào phòng =====
            PrintTitle("1. THÊM THIẾT BỊ VÀO PHÒNG");
            roomA.AddDevice(pc1);
            roomA.AddDevice(pc2);
            roomA.AddDevice(pr1);
            roomB.AddDevice(pr2);
            roomB.AddDevice(pj1);
            foreach (LabRoom room in rooms)
                Console.WriteLine($"Đã thêm: {room}");

            // ===== 2. Thêm thiết bị trùng mã / null =====
            PrintTitle("2. THỬ THÊM THIẾT BỊ TRÙNG MÃ VÀ NULL");
            var duplicate = new Computer("PC01", "Máy tính trùng mã", 2024, 20_000_000m, 16, "AMD Ryzen 5", false);
            TryAction("Thêm PC01 (trùng mã) vào P305", () => roomA.AddDevice(duplicate));
            TryAction("Thêm null vào P112", () => roomB.AddDevice(null!));

            // ===== 3. In danh sách thiết bị từng phòng =====
            PrintTitle("3. DANH SÁCH THIẾT BỊ TỪNG PHÒNG");
            foreach (LabRoom room in rooms)
            {
                Console.WriteLine(room);
                foreach (Device device in room.Devices)
                    Console.WriteLine($"   {device}");   // ToString() đa hình
                Console.WriteLine();
            }

            // ===== 4. Tổng chi phí bảo trì từng phòng =====
            PrintTitle("4. CHI PHÍ BẢO TRÌ DỰ KIẾN TRONG NĂM");
            foreach (LabRoom room in rooms)
            {
                Console.WriteLine(room);
                foreach (Device device in room.Devices)
                    Console.WriteLine($"   {device.Id,-6} {device.DeviceType,-12}: {device.CalculateAnnualMaintenanceCost(),15:N0} đ");
                Console.WriteLine($"   => TỔNG: {room.CalculateAnnualMaintenanceCost():N0} đ\n");
            }

            // ===== 5. Thiết bị cần bảo trì =====
            PrintTitle("5. THIẾT BỊ CẦN BẢO TRÌ");
            foreach (LabRoom room in rooms)
            {
                List<Device> needMaintenance = room.GetDevicesRequiringMaintenance();
                Console.WriteLine($"{room.RoomId}: {needMaintenance.Count} thiết bị");
                foreach (Device device in needMaintenance)
                {
                    string reason = device.Status == DeviceStatus.UnderMaintenance ? "đang bảo trì" : "";
                    if (device.YearsInUse > 5)
                        reason += (reason.Length > 0 ? ", " : "") + $"đã dùng {device.YearsInUse} năm";
                    Console.WriteLine($"   - {device.Id} {device.Name} ({reason})");
                }
            }

            // ===== 6. Kết nối mạng cho các đối tượng INetworkable =====
            PrintTitle("6. KẾT NỐI MẠNG");
            List<INetworkable> networkDevices = allDevices.OfType<INetworkable>().ToList();
            int hostNumber = 10;
            foreach (INetworkable net in networkDevices)
            {
                net.Connect($"192.168.1.{hostNumber++}");
                Console.WriteLine($"   {GetLabel(net)} đã kết nối: {net.IpAddress}");
            }
            TryAction("Kết nối lại PC01 khi đang kết nối", () => pc1.Connect("192.168.1.99"));
            TryAction("Kết nối PR02 với IP rỗng", () => { pr2.Disconnect(); pr2.Connect(""); });
            pr2.Connect("192.168.1.12");

            // ===== 7. Duyệt thiết bị mạng qua kiểu INetworkable =====
            PrintTitle("7. DUYỆT THIẾT BỊ MẠNG QUA INetworkable");
            foreach (INetworkable net in networkDevices)
                Console.WriteLine($"   {GetLabel(net),-6} | IsConnected = {net.IsConnected,-5} | IP = '{net.IpAddress}'");

            Console.WriteLine("\n   -> Ngắt kết nối tất cả:");
            foreach (INetworkable net in networkDevices)
            {
                net.Disconnect();
                Console.WriteLine($"   {GetLabel(net),-6} | IsConnected = {net.IsConnected,-5} | IP = '{net.IpAddress}'");
            }

            // ===== 8. Minh họa kết tập: thiết bị tồn tại độc lập với phòng =====
            PrintTitle("8. MINH HỌA KẾT TẬP");
            bool removed = roomA.RemoveDevice("PR01");
            Console.WriteLine($"Gỡ PR01 khỏi P305: {removed}");
            Console.WriteLine($"Tìm PR01 trong P305: {(roomA.FindDevice("PR01") == null ? "không còn" : "vẫn còn")}");
            Console.WriteLine($"Đối tượng PR01 vẫn tồn tại: {pr1}");
            roomB.AddDevice(pr1);
            Console.WriteLine($"Chuyển PR01 sang P112 -> {roomB}");

            Console.WriteLine("\nHoàn thành.");
        }

        /// <summary>In tiêu đề cho từng phần.</summary>
        static void PrintTitle(string title)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔" + new string('═', 68) + "╗");
            Console.WriteLine("║ " + title.PadRight(67) + "║");
            Console.WriteLine("╚" + new string('═', 68) + "╝");
            Console.ResetColor();
        }

        /// <summary>Thực hiện thao tác và in ra lỗi nếu bị từ chối.</summary>
        static void TryAction(string description, Action action)
        {
            try
            {
                action();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"   ✔ {description}");
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"   ✘ {description} → bị từ chối: {ex.Message}");
            }
            finally
            {
                Console.ResetColor();
            }
        }

        /// <summary>Lấy mã thiết bị để hiển thị (Device là lớp trừu tượng, không phải lớp cụ thể).</summary>
        static string GetLabel(INetworkable net)
        {
            return net is Device device ? device.Id : "?";
        }
    }
}
