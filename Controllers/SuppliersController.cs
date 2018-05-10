using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportStore.Api.Data;
using SportStore.Api.Models;
using SportStore.Api.Models.BindingTargets;

namespace SportStore.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrator")]
    public class SuppliersController : ControllerBase
    {
        private readonly DataContext _context;

        public SuppliersController(DataContext context) => _context = context;

        [HttpGet]
        public IEnumerable<Supplier> Get() => _context.Suppliers;

        [HttpPost]
        public IActionResult Create([FromBody] SupplierData data)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var s = data.Supplier;

            _context.Add(s);
            _context.SaveChanges();

            return Ok(s.SupplierId);
        }

        [HttpPut("{id}")]
        public IActionResult Edit(long id, [FromBody] SupplierData data)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var s = data.Supplier;
            s.SupplierId = id;

            _context.Update(s);
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]
        public void Delete(long id)
        {
            _context.Remove(new Supplier {SupplierId = id});
            _context.SaveChanges();
        }
    }
}