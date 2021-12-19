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
    public class CategoriesController : ControllerBase
    {
        private readonly ProductCategoriesDomain domain;

        public CategoriesController(ProductCategoriesDomain domain)
        {
            this.domain = domain;
        }

        [HttpGet(Name = "GetCategories")]
        [ProducesResponseType(type: typeof(IEnumerable<ProductCategoryDto>), (int)HttpStatusCode.OK)]
        public IActionResult GetCategories([FromQuery] int page = 1)
        {
            return Ok(domain.GetProductCategories(page));
        }

        [HttpGet("metadata",Name = "GetCategoriesMetaData")]
        [ProducesResponseType(type: typeof(MetaDataDto), (int)HttpStatusCode.OK)]
        public IActionResult GetCategoriesMetaData()
        {
            var metaData = domain.GetProductCategoriesMetaData();
            return Ok(metaData);
        }

        [HttpGet("{id}", Name = "GetCategoryById")]
        [ProducesResponseType(type: typeof(ProductCategoryDto), (int)HttpStatusCode.OK)]
        public IActionResult GetCategory(Guid id)
        {
            var item = domain.GetProductCategory(id.ToString());
            if (item == null) return NotFound();

            return Ok(item);
        }

        [HttpPost(Name = "CreateCategory")]
        [Authorize("catalog.fullaccess")]
        [ProducesResponseType(type: typeof(ProductCategoryDto), (int)HttpStatusCode.Created)]
        public IActionResult PostCategory(ProductCategoryDto productCategory)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = domain.AddProductCategory(productCategory);

            domain.SaveChanges();
            productCategory.Id = id;

            return CreatedAtRoute("GetCategoryById", new { id }, productCategory);
        }

        [HttpPut(Name = "UpdateCategory")]
        [Authorize("catalog.fullaccess")]
        [ProducesResponseType(type: typeof(ProductCategoryDto), (int)HttpStatusCode.NoContent)]
        public IActionResult PutCategory(ProductCategoryDto productCategory)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            domain.UpdateProductCategory(productCategory);

            domain.SaveChanges();

            return NoContent();
        }

        [HttpDelete(Name = "DeleteCategory")]
        [Authorize("catalog.fullaccess")]
        [ProducesResponseType(type: typeof(ProductCategoryDto), (int)HttpStatusCode.OK)]
        public IActionResult DeleteCategory(Guid id)
        {
            var item = domain.DeleteProductCategory(id.ToString());

            domain.SaveChanges();

            return Ok(item);
        }

    }

}
