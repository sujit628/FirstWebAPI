using FirstWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly BrandDbContext _brandDbContext;

        public BrandController(BrandDbContext brandDbContext)
        {
            _brandDbContext = brandDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brand>>> GetBrands()
        {
            if (_brandDbContext.Brands == null)
            {
                return NotFound();
            }
            return await _brandDbContext.Brands.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Brand>> GetBrand(int id)
        {
            if (_brandDbContext.Brands == null)
            {
                return NotFound();
            }
            var brand = await _brandDbContext.Brands.FindAsync(id);
            if(brand == null)
            {
                return NotFound();
            }
            return brand;
        }
        [HttpPost]
        public async Task<ActionResult<Brand>> PostBrand(Brand brand)
        {
            _brandDbContext.Brands.Add(brand);
            await _brandDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBrand), new {id = brand.ID}, brand);
        }

        [HttpPut]
        public async Task<IActionResult> PutBrand(int id, Brand brand)
        {
            if(id != brand.ID)
            {
                return BadRequest();
            }
            _brandDbContext.Entry(brand).State = EntityState.Modified;

            try
            {
                await _brandDbContext.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if(!BrandAvailable(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok();
        }

        private bool BrandAvailable(int id)
        {
            return (_brandDbContext.Brands?.Any(x => x.ID == id)).GetValueOrDefault();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            if(_brandDbContext.Brands == null)
            {
                return NotFound();
            }
            var brand = await _brandDbContext.Brands.FindAsync(id);
            if(brand == null)
            {
                return NotFound();
            }
            _brandDbContext.Remove(brand);
            await _brandDbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
