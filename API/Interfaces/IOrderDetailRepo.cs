﻿using Data_Layer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IOrderDetailRepo
    {
        Task<IEnumerable<OrderDetail>> GetOrderDetailsAsync();

        void AddOrderDetail(OrderDetail orderDetail);

        void DeleteOrderDetail(int id);

        bool OrderDetailExists(int id);

        int GetProductId(string name);

        string GetProductName(int id);

        IEnumerable<OrderDetail> GetDetailsForOrder(string orderNum);
    }
}
