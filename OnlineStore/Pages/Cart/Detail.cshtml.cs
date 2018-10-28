using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OnlineStore.Pages.Cart
{
    public class DetailModel : PageModel
    {
        public Item Item { get; set; }
        private IItemRepository _itemRepository;

        public DetailModel(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public void OnGet(int? id)
        {
            //Kiểm tra tham số truyền vào có rổng hay không
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Nếu không thì truy xuất csdl lấy ra sản phẩm tương ứng
            //SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id && n.DaXoa == false);
            Item = _itemRepository.Find(n => n.Id == id && n.Deleted == false);
            if (Item == null)
            {
                //Thông báo nếu như không có sản phẩm đó
                //return HttpNotFound();
            }
        }
    }
}