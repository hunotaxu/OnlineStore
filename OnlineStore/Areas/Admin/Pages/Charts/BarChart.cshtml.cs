using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL.EF;
using DAL.Models;
using OnlineStore.Models.ViewModels;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Text;
using DAL.Data.Entities;
using DAL.Repositories;

namespace OnlineStore.Pages.Admin.Charts
{
    public class BarChartModel : PageModel
    {
        private readonly DAL.EF.OnlineStoreDbContext _context;
        private IOrderRepository _orderRepository;
        private ILineItemRepository _lineItemRepository;

        public BarChartModel(DAL.EF.OnlineStoreDbContext context, IOrderRepository orderRepository, ILineItemRepository lineItemRepository)
        {
            _context = context;
            _orderRepository = orderRepository;
            _lineItemRepository = lineItemRepository;
            Thang1 = 0;
            Thang2 = 0;
            Thang3 = 0;
            Thang4 = 0;
            Thang5 = 0;
            Thang6 = 0;
            Thang7 = 0;
            Thang8 = 0;
            Thang9 = 0;
            Thang10 = 0;
            Thang11 = 0;
            Thang12 = 0;
        }

        public IList<DAL.Data.Entities.Order> Order { get; set; }
        public IList<LineItem> LineItems { get; set; }
        public decimal Thang1;
        public decimal Thang2;
        public decimal Thang3;
        public decimal Thang4;
        public decimal Thang5;
        public decimal Thang6;
        public decimal Thang7;
        public decimal Thang8;
        public decimal Thang9;
        public decimal Thang10;
        public decimal Thang11;
        public decimal Thang12;

        public ActionResult OnGetAsync()
        {
            IList<DAL.Data.Entities.Order> orders= _orderRepository.GetSome(o => o.DeliveryDate.Year == 2018).ToList();
            foreach (var order in orders)
            {
                switch (order.DeliveryDate.Month)
                {
                    case 1:
                        Thang1 += _lineItemRepository.GetItems(l => l.OrderId == order.Id).Sum(t => t.Amount);
                        break;
                    case 2:
                        Thang2 += _lineItemRepository.GetItems(l => l.OrderId == order.Id).Sum(t => t.Amount);
                        break;
                    case 3:
                        Thang3 += _lineItemRepository.GetItems(l => l.OrderId == order.Id).Sum(t => t.Amount);
                        break;
                    case 4:
                        Thang4 += _lineItemRepository.GetItems(l => l.OrderId == order.Id).Sum(t => t.Amount);
                        break;
                    case 5:
                        Thang5 += _lineItemRepository.GetItems(l => l.OrderId == order.Id).Sum(t => t.Amount);
                        break;
                    case 6:
                        Thang6 += _lineItemRepository.GetItems(l => l.OrderId == order.Id).Sum(t => t.Amount);
                        break;
                    case 7:
                        Thang7 += _lineItemRepository.GetItems(l => l.OrderId == order.Id).Sum(t => t.Amount);
                        break;
                    case 8:
                        Thang8 += _lineItemRepository.GetItems(l => l.OrderId == order.Id).Sum(t => t.Amount);
                        break;
                    case 9:
                        Thang9 += _lineItemRepository.GetItems(l => l.OrderId == order.Id).Sum(t => t.Amount);
                        break;
                    case 10:
                        Thang10 += _lineItemRepository.GetItems(l => l.OrderId == order.Id).Sum(t => t.Amount);
                        break;
                    case 11:
                        Thang11 += _lineItemRepository.GetItems(l => l.OrderId == order.Id).Sum(t => t.Amount);
                        break;
                    case 12:
                        Thang12 += _lineItemRepository.GetItems(l => l.OrderId == order.Id).Sum(t => t.Amount);
                        break;
                }
            }
            return Page();
        }

        public ActionResult BarChart()
        {
            return Page();
        }



    }
}
