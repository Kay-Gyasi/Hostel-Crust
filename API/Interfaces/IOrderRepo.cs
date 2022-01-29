namespace API.Interfaces
{
    public interface IOrderRepo
    {
        Task<IEnumerable<Orders>> GetOrdersAsync();

        void AddOrder(Orders Order);

        void DeleteOrder(int id);

        Task<Orders> GetOrderById(int id);

        int GetCustomerId(string name);

        string GetCustomerName(int id);

        Task<IEnumerable<OrderDetail>> GetOrderDetailsInOrder(string orderNum);
    }
}
