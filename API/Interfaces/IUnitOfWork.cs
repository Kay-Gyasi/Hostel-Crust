namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IOrderRepo OrderRepo { get; }

        IOrderDetailRepo DetailRepo { get; }

        ICategoryRepo CategoryRepo { get; }

        IProductRepo ProductRepo { get; }

        IUserRepo UserRepo { get; } 

        IProOrdersRepo ProOrdersRepo { get; }

        Task<bool> SaveAsync();
    }
}
