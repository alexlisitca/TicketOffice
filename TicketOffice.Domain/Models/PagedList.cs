using System.Collections;
using System.Collections.Generic;

namespace TicketOffice.Domain.Models
{
    public class PagedList<T>
    {
        private IEnumerable<T> _source;

        public IEnumerable<T> Data { get { return _source; } }
        public int TotalCount { get; }
        public int PageIndex { get; }
        public int TotalPages { get; }
        public int PageSize { get; }
        public bool HasNext { get; }
        public bool HasPrevious { get; }

        public PagedList(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
        {
            _source = source;
            TotalCount = totalCount;
            TotalPages = totalCount / pageSize + (totalCount % pageSize == 0 ? 0 : 1);
            PageIndex = pageIndex + 1;
            PageSize = pageSize;
            HasNext = pageIndex  != TotalPages - 1;
            HasPrevious = pageIndex != 0;
        }
    }
}
