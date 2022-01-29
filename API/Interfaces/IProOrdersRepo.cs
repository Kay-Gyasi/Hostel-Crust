namespace API.Interfaces
{
    public interface IProOrdersRepo
    {
        Task<IEnumerable<ProcessedOrders>> GetProcessedOrders();

        void AddProcessedOrder(ProcessedOrders order);

        void DeleteProcessedOrder(int id);
    }
}
