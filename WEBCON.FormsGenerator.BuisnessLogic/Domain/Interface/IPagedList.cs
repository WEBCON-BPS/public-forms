using System.Collections;
using System.Collections.Generic;

namespace WEBCON.FormsGenerator.BusinessLogic.Domain.Interface
{
    public interface IPagedList<out T> : IPagedList, IEnumerable<T>, IEnumerable
    {
        T this[int index] { get; }

        int Count { get; }
    }
    public interface IPagedList
    {
        int TotalPages { get; }
        int TotalItemCount { get; }
        int PageIndex { get; }
        int PageSize { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get; }
    }
}
