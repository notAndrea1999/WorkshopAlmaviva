using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WorkshopApi.Models.Db;

namespace WorkshopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly WorkShopDbContext _context;

        public BookController(WorkShopDbContext context)
        {
            _context = context;
        }

   
        [HttpGet]
        
        public async Task<ActionResult<IEnumerable<Book>>> GetBook([FromQuery] PaginationFilter filter, [FromQuery] string word = "")
        {
            try
            {
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
                var quryable = _context.Book.AsQueryable();


                if (!string.IsNullOrEmpty(word))
                {
                    quryable = quryable.Where(b => b.Title.ToLower().Contains(word) || b.Author.ToLower().Contains(word));
                }

                if (User.IsInRole("user"))
                {
                    quryable = quryable.Where(b => b.Availability == true);
                }

                var pagedData = await PaginatedList<Book>.CreateAsync(quryable, validFilter.PageNumber, validFilter.PageSize);

                Console.WriteLine($"Word: {word}, PageNumber: {validFilter.PageNumber}, PageSize: {validFilter.PageSize}, " +
                    $"TotalItems: {pagedData.TotalItems}, TotalPages: {pagedData.TotalPages}");

                return Ok(pagedData);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // GET: api/Book/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Book.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // PUT: api/Book/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Book
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            _context.Book.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        // DELETE: api/Book/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Book.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
    }
}
