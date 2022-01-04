using API.Interfaces;
using Data_Layer.Data_Context;
using Data_Layer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data.Repository
{
    public class OrderRepo : IOrderRepo
    {
        private readonly HostelContext db;

        public OrderRepo(HostelContext db)
        {
            this.db = db;
        }

        public void AddOrder(Orders Order)
        {
            db.orders.Add(Order);
        }

        public void DeleteOrder(int id)
        {
            var order = db.orders.Find(id);
            db.orders.Remove(order);    
        }

        public int GetCustomerId(string name)
        {
            return db.users.FirstOrDefault(x => (x.FirstName + ' ' + x.LastName) == name).CustomerID;
        }

        public string GetCustomerName(int id)
        {
            var user = db.users.Find(id);

            var name = user.FirstName + ' ' + user.LastName;

            return name;
        }


        public async Task<Orders> GetOrderById(int id)
        {
            return await db.orders.FindAsync(id);
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsInOrder(string orderNum)
        {
            var details  = new List<OrderDetail>();

            var orderDetails = await db.OrderDetails.ToListAsync();

            foreach (var item in orderDetails)
            {
                if(item.OrderNum == orderNum)
                {
                    details.Add(item);
                }
            }

            return details;
        }

        public async Task<IEnumerable<Orders>> GetOrdersAsync()
        {
            return await db.orders.ToListAsync();
        }
    }
}
