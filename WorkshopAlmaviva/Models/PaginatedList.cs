using Microsoft.EntityFrameworkCore;

public class PaginatedList<T>
{
    public int PageNumber { get;  set; }
    public int PageSize { get;  set; }
    public int TotalItems { get;  set; }
    public int TotalPages { get;  set; }
    public List<T> Items { get; private set; }


    public PaginatedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalItems = count;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        Items = items;
    }

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }
}
