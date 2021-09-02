using System;
using System.Threading.Tasks;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Interface;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Interface
{
    /// <summary>
    /// Reponsible for query form data
    /// </summary>
    public interface IFormQueryService
    {
        /// <summary>
        /// Get forms list
        /// </summary>
        /// <returns>List of forms</returns>
        Task<IPagedList<Domain.Model.Form>> GetForms(string searchString, int pageNumber, string sortOption, int itemsOnPage);
        /// <summary>
        /// Get existing form object for provided identifier
        /// </summary>
        /// <param name="id">Form identifier</param>
        /// <returns></returns>
        DTO.Form GetForm(int id);
        /// <summary>
        /// Get existing form object for provided Guid
        /// </summary>
        /// <param name="guid">Form Guid</param>
        /// <returns></returns>
        DTO.Form GetForm(Guid guid);
    }
}
