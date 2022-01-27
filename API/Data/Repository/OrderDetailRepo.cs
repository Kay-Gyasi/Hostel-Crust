using API.Interfaces;
using Data_Layer.Data_Context;
using Data_Layer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data.Repository
{
    public class OrderDetailRepo : IOrderDetailRepo
    {
        private readonly HostelContext db;

        public OrderDetailRepo(HostelContext db)
        {
            this.db = db;
        }

        public void AddOrderDetail(OrderDetail orderDetail)
        {
            db.OrderDetails.Add(orderDetail);
        }

        public void DeleteOrderDetail(int id)
        {
            var detail = this.db.OrderDetails.Find(id);

            if(detail != null)
                db.Remove(detail);
        }

        public bool OrderDetailExists(int id)
        {
            var info = this.db.OrderDetails.Find(id);

            return info != null;
        }

        public IEnumerable<OrderDetail> GetDetailsForOrder(string orderNum)
        {
            var details = new List<OrderDetail>();

            foreach(var i in db.OrderDetails)
            {
                if(i.OrderNum == orderNum)
                {
                    details.Add(i);
                }
            }
            return details;
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsAsync()
        {
            return await db.OrderDetails.ToListAsync();
        }

        public int GetProductId(string name)
        {
            return db.products.FirstOrDefault(x => x.Name == name).ProductID;
        }

        public string GetProductName(int id)
        {
            var product = db.products.Find(id);
            return product.Name;
        }
    }
}
