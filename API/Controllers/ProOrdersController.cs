﻿using API.DTOs;
using API.Interfaces;
using Data_Layer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    public class ProOrdersController : BaseController
    {
        private readonly IUnitOfWork uow;

        public ProOrdersController(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        [HttpGet("GetProOrders")]
        public async Task<IActionResult> GetProOrders()
        {
            var orders = await uow.ProOrdersRepo.GetProcessedOrders();
            return Ok(orders);
        }

        [HttpDelete("DeleteProOrder/{id}")]
        public async Task<IActionResult> DeleteProOrder(int id)
        {
            uow.ProOrdersRepo.DeleteProcessedOrder(id);
            await uow.SaveAsync();
            return Ok();
        }


        [HttpPost("PostProOrder")]
        public async Task<IActionResult> PostProOrder(ProOrdersDto proOrderDto)
        {
            ProcessedOrders Order = new();

            Order.OrderID = proOrderDto.OrderID;
            Order.isFulfilled = proOrderDto.isFulfilled;
            Order.CustomerID = uow.OrderRepo.GetCustomerId(proOrderDto.Customer);
            Order.OrderNum = proOrderDto.OrderNum;
            Order.AdditionalInfo = proOrderDto.AdditionalInfo;
            Order.DeliveryLocation = proOrderDto.DeliveryLocation;
            Order.isDelivery = proOrderDto.isDelivery;

            uow.ProOrdersRepo.AddProcessedOrder(Order);

            await uow.SaveAsync();

            return CreatedAtAction("GetProOrders", new { id = Order.OrderID }, proOrderDto);
        }
    }
}
