namespace WEBCON.FormsGenerator.BusinessLogic.Application.Interface
{
    /// <summary>
    /// Service providing operations on the form 
    /// </summary>
    public interface IFormCommandService
    {
        /// <summary>
        /// Add new form
        /// </summary>
        /// <returns>Created form identifier</returns>
        int AddForm(DTO.Form formModel);
        /// <summary>
        /// Edit existing form
        /// </summary>
        void EditForm(DTO.Form formModel);
        /// <summary>
        /// Remove existing form
        /// </summary>
        /// <param name="formId">Removing form identifier</param>
        void RemoveForm(int formId);   
    }
}
