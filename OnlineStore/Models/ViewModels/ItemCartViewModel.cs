namespace OnlineStore.Models.ViewModels
{
    public class ItemCartViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public string Image { get; set; }

        //public ItemCart(int iMaSP)
        //{
        //    using (QuanLyBanHangEntities db = new QuanLyBanHangEntities())
        //    {
        //        this.Id = iMaSP;
        //        Item item = db.SANPHAMs.Single(n => n.MaSP == iMaSP);
        //        Item item =
        //        this.Name = sp.TenSP;
        //        this.Image = sp.HinhAnh;
        //        this.Price = sp.DonGia.Value;
        //        this.Quantity = 1;
        //        this.Amount = DonGia * SoLuong;
        //    }
        //}

        //public ItemCart(int iMaSP, int sl)
        //{
        //    using (QuanLyBanHangEntities db = new QuanLyBanHangEntities())
        //    {
        //        this.MaSP = iMaSP;
        //        SANPHAM sp = db.SANPHAMs.Single(n => n.MaSP == iMaSP);
        //        this.TenSP = sp.TenSP;
        //        this.HinhAnh = sp.HinhAnh;
        //        this.DonGia = sp.DonGia.Value;
        //        this.SoLuong = sl;
        //        this.ThanhTien = DonGia * SoLuong;
        //    }
        //}

        public ItemCartViewModel()
        {


        }

    }
}