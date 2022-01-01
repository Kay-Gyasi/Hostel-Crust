using API.DTOs;
using API.Interfaces;
using Data_Layer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class OrderDetailController : BaseController
    {
        private readonly IUnitOfWork uow;

        public OrderDetailController(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        [HttpGet("GetOrderDetails")]
        public async Task<IActionResult> GetOrderDetails()
        {
            var details = await uow.DetailRepo.GetOrderDetailsAsync();

            var detailsDto = from order in details
                             select new OrderDetailDto
                             {
                                 OrderNum = order.OrderNum,
                                 Product = uow.DetailRepo.GetProductName(order.ProductID),
                                 Price = order.Price,
                                 Quantity = order.Quantity,
                                 TotalPrice = order.TotalPrice
                             };
            return Ok(detailsDto);
        }

        [HttpDelete("DeleteOrderDetail/{id}")]
        public async Task<IActionResult> DeleteOrderDetail(int id)
        {
            uow.DetailRepo.DeleteOrderDetail(id);
            await uow.SaveAsync();

            return Ok(id);
        }

        [HttpPost("PostOrderDetail")]
        public async Task<IActionResult> PostOrderDetail(OrderDetailDto orderDetailDto)
        {
            var detail = new OrderDetail();
            detail.OrderNum = orderDetailDto.OrderNum;
            detail.ProductID = uow.DetailRepo.GetProductId(orderDetailDto.Product);
            detail.Price = orderDetailDto.Price;
            detail.Quantity = orderDetailDto.Quantity;
            detail.TotalPrice = orderDetailDto.TotalPrice;

            uow.DetailRepo.AddOrderDetail(detail);

            await uow.SaveAsync();

            return CreatedAtAction("GetOrderDetails", new { id = detail.OrderDetailID }, orderDetailDto);
        }

        [HttpGet("GetDetailsForOrders/orderNum")]
        public async Task<IActionResult> GetDetailsForOrders(string orderNum)
        {
            var details = await Task.Run(() => uow.DetailRepo.GetDetailsForOrder(orderNum));

            var detailsDto = from order in details
                             select new OrderDetailDto
                             {
                                 OrderNum = order.OrderNum,
                                 Product = uow.DetailRepo.GetProductName(order.ProductID),
                                 Price = order.Price,
                                 Quantity = order.Quantity,
                                 TotalPrice = order.TotalPrice
                             };
            return Ok(detailsDto);
        }
    }
}
