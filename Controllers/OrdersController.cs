using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportStore.Api.Data;
using SportStore.Api.Models;

namespace SportStore.Api.Controllers
{
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly DataContext _context;

        public OrdersController(DataContext context) => _context = context;

        [HttpGet]
        public IEnumerable<Order> Get() => _context.Orders
            .Include(o => o.Products).Include(o => o.Payment);

        [HttpPost("{id}")]
        public void MarkShipped(long id)
        {
            var order = _context.Orders.Find(id);

            if (order == null) return;

            order.Shipped = true;
            _context.SaveChanges();
        }

        [HttpPost]
        public IActionResult Create([FromBody] Order order)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            order.OrderId = 0;
            order.Shipped = false;
            order.Payment.Total = GetPrice(order.Products);

            ProcessPayment(order.Payment);

            if (order.Payment.AuthCode == null) return BadRequest("Payment rejected");

            _context.Add(order);
            _context.SaveChanges();

            return Ok(new
            {
                orderId = order.OrderId,
                authCode = order.Payment.AuthCode,
                amount = order.Payment.Total
            });
        }

        private decimal GetPrice(IEnumerable<CartLine> lines)
        {
            var ids = lines.Select(l => l.ProductId);

            return _context.Products
                .Where(p => ids.Contains(p.ProductId))
                .Select(p => lines.First(l => l.ProductId == p.ProductId).Quantity * p.Price)
                .Sum();
        }

        private void ProcessPayment(Payment payment)
        {
            // integrate your payment system here
            payment.AuthCode = "12345";
        }
    }
}