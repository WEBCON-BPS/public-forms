using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Interface;


namespace WEBCON.FormsGenerator.Data
{
    internal class PaginatedList<T> : List<T>, IPagedList<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            if (TotalPages.Equals(0)) TotalPages = 1;

            this.AddRange(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }
        public int TotalItemCount => throw new NotImplementedException();

        public int PageSize => throw new NotImplementedException();
        public static async Task<IPagedList<T>> Create(ILiteQueryable<T> source, int pageIndex, int pageSize)
        {
            return await Task.Run(() =>
            {
                var count = source.Count();
                var items = source.Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToList();
                return new PaginatedList<T>(items, count, pageIndex, pageSize);
            });
        }
    }
}
