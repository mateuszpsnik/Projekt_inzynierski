using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SocialMediumForMusicians.Data
{
    public class PaginationApiResult<T>
    {
        public List<T> Elements { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        private PaginationApiResult(List<T> elements, int totalCount, int pageIndex,
                                    int pageSize)
        {
            Elements = elements;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        // I use singleton design pattern to return Task (result will be awaited)
        public static async Task<PaginationApiResult<T>> CreateAsync(
            IQueryable<T> srcElements, int pageIndex, int pageSize)
        {
            int totalCount = await srcElements.CountAsync();
            var elements = await srcElements.Skip(pageIndex * pageSize)
                                            .Take(pageSize).ToListAsync();
            return new PaginationApiResult<T>(elements, totalCount, 
                                              pageIndex, pageSize);
        }

        public bool HasPreviousPage => PageIndex > 0;

        public bool HasNextPage => (PageIndex + 1) < TotalPages;
    }
}
