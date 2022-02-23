using API.Mailing_Service;

namespace API.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IUnitOfWork uow;
        private readonly IMail mail;

        public OrderController(IUnitOfWork uow, IMail mail)
        {
            this.uow = uow;
            this.mail = mail;
        }

        #region GetOrders
        [HttpGet("GetOrders")]
        public async Task<IActionResult> GetOrders()
        {
            var Orders = await uow.OrderRepo.GetOrdersAsync();

            if(Orders == null)
            {
                return NotFound();
            }

            var OrdersDto = from c in Orders
                            select new OrderDto()
                            {
                                OrderID = c.OrderID,
                                Customer = uow.OrderRepo.GetCustomerName(c.CustomerID),
                                isFulfilled = c.isFulfilled,
                                isDelivery = c.isDelivery,
                                AdditionalInfo = c.AdditionalInfo,
                                DeliveryLocation = c.DeliveryLocation,
                                DateOrdered = c.DateOrdered,
                                OrderNum = c.OrderNum
                            };

            return Ok(OrdersDto);
        }
        #endregion


        #region PostOrder
        [Authorize]
        [HttpPost("PostOrder")]
        public async Task<IActionResult> PostOrder(OrderDto OrdersDto)
        {
            Orders Order = new();

            Order.OrderID = OrdersDto.OrderID;
            Order.isFulfilled = OrdersDto.isFulfilled;
            Order.CustomerID = uow.OrderRepo.GetCustomerId(OrdersDto.Customer);
            Order.OrderNum = OrdersDto.OrderNum;
            Order.AdditionalInfo = OrdersDto.AdditionalInfo;
            Order.DeliveryLocation = OrdersDto.DeliveryLocation;
            Order.isDelivery = OrdersDto.isDelivery;
            Order.DateOrdered = OrdersDto.DateOrdered;

            uow.OrderRepo.AddOrder(Order);

            await uow.SaveAsync();

            return CreatedAtAction("GetOrders", new { id = Order.OrderID }, OrdersDto);
        }
        #endregion


        #region DeleteOrder
        [HttpDelete("DeleteOrder/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var isValid = await uow.OrderRepo.GetOrderById(id);
            if (isValid == null)
            {
                return BadRequest("No such Order exists");
            }

            uow.OrderRepo.DeleteOrder(id);
            await uow.SaveAsync();

            return NoContent();
        }
        #endregion


        #region PutOrder
        [HttpPut("PutOrder/{id}")]
        public async Task<IActionResult> PutOrder(int id, OrderDto OrdersDto)
        {
            var Order = await uow.OrderRepo.GetOrderById(id); 
                
            if (Order is null)
            {
                return BadRequest();
            }

            Order.OrderID = OrdersDto.OrderID;
            Order.CustomerID = uow.OrderRepo.GetCustomerId(OrdersDto.Customer);
            Order.isFulfilled = OrdersDto.isFulfilled;
            Order.OrderNum = OrdersDto.OrderNum;
            Order.isDelivery = OrdersDto.isDelivery;
            Order.DeliveryLocation = OrdersDto.DeliveryLocation;
            Order.AdditionalInfo = OrdersDto.AdditionalInfo;


            await uow.SaveAsync();

            return NoContent();
        }
        #endregion
    }
}
