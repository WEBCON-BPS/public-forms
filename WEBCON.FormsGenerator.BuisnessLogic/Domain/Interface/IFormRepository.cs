using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model;

namespace WEBCON.FormsGenerator.BusinessLogic.Domain.Interface
{
    public interface IFormRepository : IRepository<Form>
    {
        Task<IPagedList<Form>> GetFilteredPaginated(Expression<Func<Form, bool>> predicate, int page, int pageSize, string sortOption);
    }
}
