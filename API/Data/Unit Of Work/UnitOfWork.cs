namespace API.Data.Unit_Of_Work
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HostelContext db;

        public UnitOfWork(HostelContext db)
        {
            this.db = db;
        }

        public IOrderRepo OrderRepo => new OrderRepo(db);

        public ICategoryRepo CategoryRepo => new CategoryRepo(db);

        public IProductRepo ProductRepo => new ProductRepo(db);

        public IUserRepo UserRepo => new UserRepo(db);

        public IOrderDetailRepo DetailRepo => new OrderDetailRepo(db);

        public IProOrdersRepo ProOrdersRepo => new ProOrdersRepo(db);

        #region SaveAsync
        public async Task<bool> SaveAsync()
        {
            return await db.SaveChangesAsync() > 0;
        }
        #endregion
    }
}
