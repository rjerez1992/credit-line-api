using System;
using Microsoft.AspNetCore.Mvc;
using CreditLineAPI.Models;
using CreditLineAPI.Services;
using System.Globalization;
using System.Threading;
using CreditLineAPI.Enums;

namespace CreditLineAPI.Controllers
{
    [Route("credit-line")]
    public class CreditLineApplicationController : Controller
    {
        private readonly ICreditLineApplicationService _creditLineApplicationService;

        public CreditLineApplicationController(ICreditLineApplicationService creditLineApplicationService)
        {
            _creditLineApplicationService = creditLineApplicationService;
        }

        [HttpGet("id")]
        public ObjectResult GetId()
        {
            return new OkObjectResult(new { Value = _creditLineApplicationService.GetNewApplicationId() });
        }

        [HttpPost("apply")]
        public ObjectResult Apply([FromBody] CreditLineApplication application)
        {
            if (application == null)
                return new BadRequestObjectResult(new { Message = Properties.strings.UnableToProcessApplication });

            application = _creditLineApplicationService.Validate(application);

            if (application.IsAccepted)
                return new OkObjectResult(application.CreditLineResume());

            if (application.HasRetriesLeft)
                return new BadRequestObjectResult(application.CreditLineResume());

            return new BadRequestObjectResult(new { Message = Properties.strings.SaleAgentContactNotice });
        }
    }
}
