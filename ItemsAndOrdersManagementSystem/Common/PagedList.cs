using Microsoft.EntityFrameworkCore;

namespace ItemsAndOrdersManagementSystem.Common
{
    public class PagedList<T>
    {
        public int TotalCount { get; private set; }
        public List<T> Data { get; private set; } = new List<T>();
        public PagedList(List<T> items, int count)
        {
            TotalCount = count;
            Data.AddRange(items);
        }
        async public static Task<PagedList<T>> ToPagedList(IQueryable<T> source, int skip, int take)
        {
            var count = await source.CountAsync();
            if (skip == 0 && take == 0)
            {
                return new PagedList<T>(await source.ToListAsync(), count);
            }
            var items = await source.Skip(skip).Take(take).ToListAsync();
            return new PagedList<T>(items, count);
        }

    }
}
