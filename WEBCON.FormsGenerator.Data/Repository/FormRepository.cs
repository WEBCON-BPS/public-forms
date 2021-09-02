using System;
using System.Linq.Expressions;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Interface;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model;
using System.Threading.Tasks;
using LiteDB;

namespace WEBCON.FormsGenerator.Data.Repository
{
    class FormRepository: Repository<Form>, IFormRepository
    {
        public FormRepository(LiteDatabase db, string tableName) : base(db, tableName)
        {
        }
        public Task<IPagedList<Form>> GetFilteredPaginated(Expression<Func<Form, bool>> predicate, int page, int pageSize, string sortOption)
        {
            var result = collection.Query().Where(predicate);

            var resultSorted = sortOption switch
            {
                "name_desc" => result.OrderByDescending(s => s.Name),
                "Date" => result.OrderBy(s => s.Created),
                "date_desc" => result.OrderByDescending(s => s.Created),
                _ => result.OrderByDescending(s => s.Id),
            };
             return PaginatedList<Form>.Create(resultSorted, page, pageSize);
        }
    }
}
