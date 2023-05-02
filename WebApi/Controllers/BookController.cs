using Microsoft.AspNetCore.Mvc;
using WebApi.Entities;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<BookController> _logger;

        public BookController(IBookRepository bookRepository, ILogger<BookController> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var book = await _bookRepository.GetBookByIdAsync(id);
                if (book == null)
                {
                    return NotFound();
                }

                return Ok(book);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var books = await _bookRepository.GetAllBooksAsync();
                if (books == null || books.Count() == 0)
                {
                    return NotFound();
                }

                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Book book)
        {
            try
            {
                if (book == null || string.IsNullOrWhiteSpace(book.Title))
                {
                    return BadRequest();
                }

                var result = await _bookRepository.AddBookAsync(book);
                if(result == null)
                {
                    return BadRequest();
                }

                return Created($"api/book/{book.Id}", result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
