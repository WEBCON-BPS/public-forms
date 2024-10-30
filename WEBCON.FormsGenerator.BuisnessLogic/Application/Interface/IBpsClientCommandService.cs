using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WEBCON.FormsGenerator.BusinessLogic.Application.DTO;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Interface
{
    /// <summary>
    /// Definition for BPS API provider
    /// </summary>
    public interface IBpsClientCommandService
    {
        /// <summary>
        /// Starts element in BPS
        /// </summary>
        Task<StartElementResult> StartElement(IEnumerable<DTO.FormField> fields, Guid workflowGuid, Guid formTypeGuid, Guid stepPathGuid, Guid businessEntityGuid, ILogger logger, Credentials customCredentials = null);
    }
}
