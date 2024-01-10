using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RanusWebAPI.Data;
using RanusWebAPI.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace RanusWebAPI.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public CustomersController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: api/customers
        [HttpGet]
        [SwaggerOperation("Get all customers")]
        public async Task<IActionResult> GetAllCustomers(
            [SwaggerParameter("Search term for filtering customers (Id or Name)", Required = false)] string? search,
            [SwaggerParameter("Field to sort by (e.g., 'id', 'name', 'contact', 'address', 'city', 'country', 'phone')", Required = false)] string? sortBy,
            [SwaggerParameter("Sort order ('asc' or 'des')", Required = false)] string? sortOrder)
        {
            
            try
            {
                IQueryable<Customer> query = _db.Customers;

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(c => c.CustomerId.Contains(search) || c.CompanyName.Contains(search));
                }


                if (!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortBy)
                    {
                        case "id":
                            query = sortOrder == "des" ? query.OrderByDescending(c => c.CustomerId) : query.OrderBy(c => c.CustomerId);
                            break;
                        case "name":
                            query = sortOrder == "des" ? query.OrderByDescending(c => c.CompanyName) : query.OrderBy(c => c.CompanyName);
                            break;
                        case "contact":
                            query = sortOrder == "des" ? query.OrderByDescending(c => c.ContactName) : query.OrderBy(c => c.ContactName);
                            break;
                        case "address":
                            query = sortOrder == "des" ? query.OrderByDescending(c => c.Address) : query.OrderBy(c => c.Address);
                            break;
                        case "city":
                            query = sortOrder == "des" ? query.OrderByDescending(c => c.City) : query.OrderBy(c => c.City);
                            break;
                        case "country":
                            query = sortOrder == "des" ? query.OrderByDescending(c => c.Country) : query.OrderBy(c => c.Country);
                            break;
                        case "phone":
                            query = sortOrder == "des" ? query.OrderByDescending(c => c.Phone) : query.OrderBy(c => c.Phone);
                            break;
                        default:
                            query = query.OrderBy(c => c.CustomerId);
                            break;
                    }
                }

                List<Customer> customers = await query.ToListAsync();

                return Ok(customers);
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        // GET: api/customers/page
        [HttpGet("page")]
        [SwaggerOperation("Get customers to page")]
        public async Task<IActionResult> GetCustomersToPage(
            [SwaggerParameter("Search term for filtering customers (Id or Name)", Required = false)] string? search,
            [SwaggerParameter("Field to sort by (e.g., 'id', 'name', 'contact', 'address', 'city', 'country', 'phone')", Required = false)] string? sortBy,
            [SwaggerParameter("Sort order ('asc' or 'des')", Required = false)] string? sortOrder,
            [SwaggerParameter("Page number", Required = false)] int? page,
            [SwaggerParameter("Number of items per page", Required = false)] int? limit)
        {
            try
            {
                int _page = page ?? 1;
                int _limit = limit ?? 20;
                int _totalPages = 1;
                List<Customer> _customers = new List<Customer>();
                IQueryable<Customer> query = _db.Customers;

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(c => c.CustomerId.Contains(search) || c.CompanyName.Contains(search));
                }

                _totalPages = (int)Math.Ceiling((double)query.Count() / _limit);

                if (!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortBy)
                    {
                        case "id":
                            query = sortOrder == "des" ? query.OrderByDescending(c => c.CustomerId) : query.OrderBy(c => c.CustomerId);
                            break;
                        case "name":
                            query = sortOrder == "des" ? query.OrderByDescending(c => c.CompanyName) : query.OrderBy(c => c.CompanyName);
                            break;
                        case "contact":
                            query = sortOrder == "des" ? query.OrderByDescending(c => c.ContactName) : query.OrderBy(c => c.ContactName);
                            break;
                        case "address":
                            query = sortOrder == "des" ? query.OrderByDescending(c => c.Address) : query.OrderBy(c => c.Address);
                            break;
                        case "city":
                            query = sortOrder == "des" ? query.OrderByDescending(c => c.City) : query.OrderBy(c => c.City);
                            break;
                        case "country":
                            query = sortOrder == "des" ? query.OrderByDescending(c => c.Country) : query.OrderBy(c => c.Country);
                            break;
                        case "phone":
                            query = sortOrder == "des" ? query.OrderByDescending(c => c.Phone) : query.OrderBy(c => c.Phone);
                            break;
                        default:
                            query = query.OrderBy(c => c.CustomerId);
                            break;
                    }
                }

                _customers = await query.Skip((_page - 1) * _limit).Take(_limit).ToListAsync();

                var result = new
                {
                    totalPages = _totalPages,
                    currentPage = _page,
                    pageSize = _limit,
                    listCustomers = _customers,
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }
    }
}
