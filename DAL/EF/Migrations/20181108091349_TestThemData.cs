using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.EF.Migrations
{
    public partial class TestThemData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>("Description", "Item", "nvarchar(MAX)");

            migrationBuilder.DropForeignKey(
                name: "FK_Item_Event_EventId",
                table: "Item");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Item",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "Item",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<decimal>(
                name: "AverageEvaluation",
                table: "Item",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AddColumn<string>(
                name: "BrandName",
                table: "Item",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "Name", "Timestamp" },
                values: new object[,]
                {
                    { 2, "Laptop", null },
                    { 3, "Máy tính bảng", null },
                    { 4, "Phụ kiện", null }
                });

            migrationBuilder.InsertData(
                table: "Item",
                columns: new[] { "Id", "AverageEvaluation", "BrandName", "CategoryId", "Deleted", "Description", "EventId", "Image", "Inventory", "Name", "Price", "Timestamp", "View" },
                values: new object[,]
                {
                    { 1, null, "Apple", 1, false, "Đây là Iphone 5", null, "Iphone5-16G.png", 200, "Iphone 5", 5000000m, null, 0 },
                    { 3, null, "Apple", 1, false, "<div id=\"CauHinh\"><div class=\"CauHinh_TieuDe\">Bộ vi xử lý</div><ul><li>+ Tên bộ vi xử lý : Chip A7 Dual-Core.</li><li>+ Tốc độ : 1.3Ghz Cyclone.</li> <li>+ Bộ nhớ đệm : ARM v8-based</li><li>+ Hệ điều hành : iOS 7.</li> </ul> <div style=\"clear:both\"></div> <div class=\"CauHinh_TieuDe\">Bộ nhớ trong</div> <ul> <li>+ RAM : 1 GB.</li> <li>+ ROM : 16 GB.</li> </ul><div style=\"clear:both\"></div><div class=\"CauHinh_TieuDe\">Màn hình (Display)</div> <ul> <li>+ Độ lớn màn hình : 7.9 inches.</li> <li>+ Độ phân giải : LED-backlit IPS LCD, 16 triệu màu.</li> </ul><div style=\"clear:both\"></div><div class=\"CauHinh_TieuDe\">Camera</div><ul><li>+ Chính : 5 MP , 2592x1944 pixels.</li><li>+ Phụ : 1.2 MP , 720p  </li> <li>+ Chức năng : Nhận diện khuôn mặt, FaceTime, tự động lấy nét.</li><li>+ Quay phim : Full HD 1080p 30fps, chống rung.</li> </ul> <div style=\"clear:both\"></div> <div class=\"CauHinh_TieuDe\">Thông tin thêm</div> <ul><li>+ Kết nối : Wifi, GPRS, 3G, Bluetooth, USB, GPS.</li><li>+ Pin : Li-Po (23.8 Wh), Lên đến 10 giờ.</li><li>+ Trọng lượng : 341g.</li><li>+ Thời gian bảo hành : 24 tháng.</li>  </ul> <div style=\"clear:both\"></div> </div>", null, "Iphone5-16G.png", 200, "Iphone 5", 5000000m, null, 0 }
                });

            migrationBuilder.InsertData(
                table: "Item",
                columns: new[] { "Id", "AverageEvaluation", "BrandName", "CategoryId", "Deleted", "Description", "EventId", "Image", "Inventory", "Name", "Price", "Timestamp", "View" },
                values: new object[,]
                {
                    { 4, null, "Dell", 2, false, "Đây là DELL", null, "AP-ME294ZP.png", 300, "DELL 3440", 12390000m, null, 0 },
                    { 5, null, "Acer", 2, false, "<div id=\"CauHinh\" > <div class=\"CauHinh_TieuDe\" >Bộ vi xử lý</div> <ul> <li>+ Tên bộ vi xử lý : Intel® Core™ i7 380M.</li> <li>+ Tốc độ : 8.0Ghz.</li><li>+ Bộ nhớ đệm : 3MB Cache L3</li> </ul> <div style=\"clear:both\"></div><div class=\"CauHinh_TieuDe\">Bộ nhớ trong (RAM)</div><ul> <li>+ Loại Ram : DDR3 1066Mhz (PC3-8500).</li> <li>+ Dung lượng : 2GB.</li></ul> <div style=\"clear:both\"></div><div class=\"CauHinh_TieuDe\">Ổ đĩa cứng HDD</div> <ul> <li>+ Dung lượng : 320GB.</li><li>+ Kích thước : 2.5 inchs.</li> <li>+ Tốc độ vòng quay : 5400 rpm.</li></ul> <div style=\"clear:both\"></div> <div class=\"CauHinh_TieuDe\">Ổ đĩa quang (ODD)</div> <ul><li>+ Loại ổ đĩa quang : DVD+/- R/RW.</li>  </ul><div style=\"clear:both\"></div><div class=\"CauHinh_TieuDe\">Màn hình (Display)</div><ul><li>+ Độ lớn màn hình : 16 inchs.</li><li>+ Độ phân giải : HD (1366 x 768).</li> </ul> <div style=\"clear:both\"></div> <div class=\"CauHinh_TieuDe\">Đồ Họa (VGA)</div><ul><li>+ Bộ xử lý : Intel HD graphics.</li><li>+ Bộ nhớ đồ họa : Upto 1696MB.</li></ul> <div style=\"clear:both\"></div><div class=\"CauHinh_TieuDe\">Thông tin thêm</div><ul><li>+ Trọng lượng : 3.5 Kg.</li><li>+ Thời gian bảo hành : 24 Tháng.</li></ul><div style=\"clear:both\"></div></div>", null, "ACER-2215i.png", 200, "ACER-2215i", 5000000m, null, 0 },
                    { 6, null, "Samsung", 3, false, "Đây là Galaxy Tab 3", null, "AP-ME294ZP.png", 200, "Galaxy Tab-3-10-P5200", 5904568m, null, 0 },
                    { 8, null, null, 4, false, "Đây là tai nghe", null, "Iphone5-16G.png", 200, "Tai nghe", 5000000m, null, 0 }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Item_Event_EventId",
                table: "Item",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_Event_EventId",
                table: "Item");

            migrationBuilder.DeleteData(
                table: "Item",
                keyColumns: new[] { "Id", "Timestamp" },
                keyValues: new object[] { 1, null });

            migrationBuilder.DeleteData(
                table: "Item",
                keyColumns: new[] { "Id", "Timestamp" },
                keyValues: new object[] { 3, null });

            migrationBuilder.DeleteData(
                table: "Item",
                keyColumns: new[] { "Id", "Timestamp" },
                keyValues: new object[] { 4, null });

            migrationBuilder.DeleteData(
                table: "Item",
                keyColumns: new[] { "Id", "Timestamp" },
                keyValues: new object[] { 5, null });

            migrationBuilder.DeleteData(
                table: "Item",
                keyColumns: new[] { "Id", "Timestamp" },
                keyValues: new object[] { 6, null });

            migrationBuilder.DeleteData(
                table: "Item",
                keyColumns: new[] { "Id", "Timestamp" },
                keyValues: new object[] { 8, null });

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumns: new[] { "Id", "Timestamp" },
                keyValues: new object[] { 2, null });

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumns: new[] { "Id", "Timestamp" },
                keyValues: new object[] { 3, null });

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumns: new[] { "Id", "Timestamp" },
                keyValues: new object[] { 4, null });

            migrationBuilder.DropColumn(
                name: "BrandName",
                table: "Item");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Image",
                table: "Item",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "Item",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "AverageEvaluation",
                table: "Item",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Item_Event_EventId",
                table: "Item",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
