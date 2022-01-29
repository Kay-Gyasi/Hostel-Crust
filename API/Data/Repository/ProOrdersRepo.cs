namespace API.Data.Repository
{
    public class ProOrdersRepo : IProOrdersRepo
    {
        private readonly HostelContext db;

        public ProOrdersRepo(HostelContext db)
        {
            this.db = db;
        }

        public void AddProcessedOrder(ProcessedOrders order)
        {
            db.processedOrders.Add(order);
        }

        public void DeleteProcessedOrder(int id)
        {
            var order = db.processedOrders.Find(id);
            if(order != null)
            {
                db.processedOrders.Remove(order);
            }
        }

        public async Task<IEnumerable<ProcessedOrders>> GetProcessedOrders()
        {
            return await db.processedOrders.ToListAsync();
        }
    }
}
