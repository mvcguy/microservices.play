using MG.Services.Catalog.Domain;
using MG.Services.Catalog.Domain.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;

namespace MG.Services.Catalog.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductsDomain domain;

        public ProductsController(ProductsDomain domain)
        {
            this.domain = domain;
        }

        [HttpGet("GetProductsByCategory", Name = "GetProductsByCategory")]
        [ProducesResponseType(type: typeof(IEnumerable<ProductDto>), (int)HttpStatusCode.OK)]
        public IActionResult GetProducts([FromQuery] Guid categoryId, [FromQuery] int page = 1)
        {
            return Ok(domain.GetProductsByCategoryId(categoryId, page));
        }

        [HttpGet(Name = "GetProducts")]
        [ProducesResponseType(type: typeof(IEnumerable<ProductDto>), (int)HttpStatusCode.OK)]
        public IActionResult GetProducts([FromQuery] int page = 1)
        {
            return Ok(domain.GetProducts(page));
        }

        [HttpGet("metadata", Name = "GetProductsMetaData")]
        [ProducesResponseType(type: typeof(MetaDataDto), (int)HttpStatusCode.OK)]
        public IActionResult GetProductsMetaData()
        {
            var metaData = domain.GetProductsMetaData();
            return Ok(metaData);
        }

        [HttpGet("{id}", Name = "GetProductById")]
        [ProducesResponseType(type: typeof(ProductDto), (int)HttpStatusCode.OK)]
        public IActionResult GetProduct(Guid id)
        {
            var item = domain.GetProduct(id);
            if (item == null) return NotFound();

            return Ok(item);
        }

        [HttpPost(Name = "CreateProduct")]
        [Authorize("catalog.fullaccess")]
        [ProducesResponseType(type: typeof(ProductDto), (int)HttpStatusCode.Created)]
        public IActionResult PostProduct(ProductDto productProduct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = domain.AddProduct(productProduct);

            domain.SaveChanges();
            productProduct.Id = id;

            return CreatedAtRoute("GetProductById", new { id }, productProduct);
        }

        [HttpPut(Name = "UpdateProduct")]
        [Authorize("catalog.fullaccess")]
        [ProducesResponseType(type: typeof(ProductDto), (int)HttpStatusCode.NoContent)]
        public IActionResult PutProduct(ProductDto productProduct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            domain.UpdateProduct(productProduct);

            domain.SaveChanges();

            return NoContent();
        }

        [HttpDelete(Name = "DeleteProduct")]
        [Authorize("catalog.fullaccess")]
        [ProducesResponseType(type: typeof(ProductDto), (int)HttpStatusCode.OK)]
        public IActionResult DeleteProduct(Guid id)
        {
            var item = domain.DeleteProduct(id);

            domain.SaveChanges();

            return Ok(item);
        }

    }
}
