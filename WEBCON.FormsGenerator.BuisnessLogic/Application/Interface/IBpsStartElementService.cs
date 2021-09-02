using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WEBCON.FormsGenerator.BusinessLogic.Application.DTO;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Interface
{
    /// <summary>
    ///  Service to start new element through BPS API 
    /// </summary>
    public interface IBpsStartElementService
    {
        /// <summary>
        /// Starts element in BPS for specified form and fields with values
        /// </summary>
        Task<StartElementResult> Start(IEnumerable<KeyValuePair<Guid, object>> fields, Guid formGuid);
    }
}
