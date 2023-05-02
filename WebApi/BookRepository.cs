using Microsoft.EntityFrameworkCore;
using WebApi.Entities;

namespace WebApi
{
    public interface IBookRepository
    {
        Task<Book?> GetBookByIdAsync(Guid id);

        Task<IEnumerable<Book>> GetAllBooksAsync();

        Task<Book?> AddBookAsync(Book book);
    }

    public class BookRepository : IBookRepository
    {
        private readonly BookstoreDbContext _dbContext;
        private readonly ILogger<BookRepository> _logger;

        public BookRepository(BookstoreDbContext dbContext, ILogger<BookRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Book?> AddBookAsync(Book book)
        {
            if (book == null || string.IsNullOrWhiteSpace(book.Title))
            {
                return null;
            }

            try
            {
                book.Id = Guid.NewGuid();
                _dbContext.Books.Add(book);

                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    return book;
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not save a book.");
                return null;
            }
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _dbContext.Books.ToListAsync();
        }

        public async Task<Book?> GetBookByIdAsync(Guid id)
        {
            return await _dbContext.Books.FirstOrDefaultAsync(b => b.Id.Equals(id));
        }
    }
}
