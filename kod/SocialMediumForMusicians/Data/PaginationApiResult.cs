using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SocialMediumForMusicians.Controllers;
using SocialMediumForMusicians.Data.Models;

namespace SocialMediumForMusicians.Data
{
    public enum SortMusicians
    {
        ByAvgScoreDesc,
        ByAvgScoreAsc,
        ByPriceAsc,
        ByPriceDesc,
        Alphabetically
    }

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

        public static async Task<PaginationApiResult<MusiciansListDTO>> CreateAsync(
            IQueryable<MusiciansListDTO> srcElements, int pageIndex, int pageSize,
            int? type = null, string instrument = null, decimal minPrice = 0.0M, 
            decimal maxPrice = 1000.0M, double minAvgScore = 0.0, int sort = 0)
        {
            // get all elements
            List<MusiciansListDTO> elements = await srcElements.ToListAsync();

            // filter
            if (type.HasValue)
            {
                elements = elements.Where(m => m.Types != null && 
                                            m.Types.Contains((MusicianType)type))
                                   .ToList();
            }
            if (!string.IsNullOrEmpty(instrument))
            {
                elements = elements.Where(m => m.Instruments != null &&
                                            m.Instruments.Contains(instrument))
                                   .ToList();
            }
            elements = elements.Where(m => m.Price >= minPrice && m.Price <= maxPrice)
                               .ToList();
            elements = elements.Where(m => m.AvgScore >= minAvgScore).ToList();

            int totalCount = elements.Count;

            // sort
            switch ((SortMusicians)sort)
            {
                case SortMusicians.ByAvgScoreDesc:
                    elements = elements.OrderByDescending(m => m.AvgScore)
                                       .ThenBy(m => m.Price)
                                       .ThenBy(m => m.Name).ToList();
                    break;
                case SortMusicians.ByAvgScoreAsc:
                    elements = elements.OrderBy(m => m.AvgScore)
                                       .ThenBy(m => m.Price)
                                       .ThenBy(m => m.Price).ToList();
                    break;
                case SortMusicians.ByPriceAsc:
                    elements = elements.OrderBy(m => m.Price)
                                       .ThenByDescending(m => m.AvgScore)
                                       .ThenBy(m => m.Name).ToList();
                    break;
                case SortMusicians.ByPriceDesc:
                    elements = elements.OrderByDescending(m => m.Price)
                                       .ThenByDescending(m => m.AvgScore)
                                       .ThenBy(m => m.Name).ToList();
                    break;
                case SortMusicians.Alphabetically:
                default:
                    elements = elements.OrderBy(m => m.Name)
                                       .ThenByDescending(m => m.AvgScore)
                                       .ThenBy(m => m.Price).ToList();
                    break;
            }

            // pagination
            elements = elements.Skip(pageIndex * pageSize)
                               .Take(pageSize).ToList();
            return new PaginationApiResult<MusiciansListDTO>(elements, totalCount,
                                              pageIndex, pageSize);
        }

        public bool HasPreviousPage => PageIndex > 0;

        public bool HasNextPage => (PageIndex + 1) < TotalPages;
    }
}
