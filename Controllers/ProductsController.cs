﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportStore.Api.Data;
using SportStore.Api.Models;
using SportStore.Api.Models.BindingTargets;

namespace SportStore.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductsController(DataContext context) => _context = context;

        [HttpGet]
        public IEnumerable<Product> Get(string category, string search, bool related = false)
        {
            IQueryable<Product> query = _context.Products;

            if (!string.IsNullOrWhiteSpace(category))
            {
                var catLower = category.ToLower();
                query = query.Where(p => p.Category.ToLower().Contains(catLower));
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchLower = search.ToLower();
                query = query.Where(p =>
                    p.Name.ToLower().Contains(searchLower) || p.Description.ToLower().Contains(searchLower));
            }

            if (!related) return query;

            query = query.Include(p => p.Supplier).Include(p => p.Ratings);
            var data = query.ToList();

            data.ForEach(p =>
            {
                if (p.Supplier != null)
                {
                    p.Supplier.Products = null;
                }

                p.Ratings?.ForEach(r => r.Product = null);
            });

            return data;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var product = await _context.Products
                .Include(p => p.Supplier).ThenInclude(s => s.Products)
                .Include(p => p.Ratings)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product != null)
            {
                if (product.Supplier != null)
                {
                    product.Supplier.Products = product.Supplier.Products.Select(p =>
                        new Product
                        {
                            ProductId = p.ProductId,
                            Name = p.Name,
                            Category = p.Category,
                            Description = p.Description,
                            Price = p.Price
                        });
                }

                if (product.Ratings != null)
                {
                    foreach (var rating in product.Ratings)
                    {
                        rating.Product = null;
                    }
                }

                return Ok(product);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Create([FromBody] ProductData data)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var p = data.Product;

            if (p.Supplier != null && p.Supplier.SupplierId != 0)
            {
                _context.Attach(p.Supplier);
            }

            _context.Add(p);
            _context.SaveChanges();

            return Ok(p.ProductId);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] ProductData data)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var p = data.Product;
            p.ProductId = id;

            if (p.Supplier != null && p.Supplier.SupplierId != 0)
            {
                _context.Attach(p.Supplier);
            }

            _context.Update(p);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPatch("{id}")]
        public IActionResult Update(long id, [FromBody] JsonPatchDocument<ProductData> patch)
        {
            var product = _context.Products
                .Include(p => p.Supplier)
                .First(p => p.ProductId == id);
            var data = new ProductData {Product = product};

            patch.ApplyTo(data, ModelState);

            if (!ModelState.IsValid || !TryValidateModel(data)) return BadRequest(ModelState);

            if (product.Supplier != null && product.Supplier.SupplierId != 0)
            {
                _context.Attach(product.Supplier);
            }

            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]
        public void Delete(long id)
        {
            _context.Products.Remove(new Product {ProductId = id});
            _context.SaveChanges();
        }
    }
}