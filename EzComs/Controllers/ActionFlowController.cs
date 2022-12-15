using EzComs.Model.ActionContext;
using EzComs.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace EzComs.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActionFlowController : ControllerBase
    {
        private readonly ILogger<ActionFlowController> _logger;

        public ActionFlowController(ILogger<ActionFlowController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "CreateActionFlow")]
        public async Task<ActionFlow> PostAsync(ActionFlow actionFlow)
        {
            _logger.Log(LogLevel.Information, $"Creation of actionFlow type {actionFlow.GetType().Name} was requested");
            return ActionFlowService.create(actionFlow);
        }

        [HttpPut(Name = "UpdateActionFlow")]
        public async Task<ActionFlow> PutAsync(ActionFlow actionFlow)
        {
            _logger.Log(LogLevel.Information, $"Update of actionFlow type {actionFlow.GetType().Name} was requested");
            return ActionFlowService.create(actionFlow);
        }


        [HttpGet(Name = "GetActionFlow")]
        public async Task<ActionFlow> GetAsync(string actionFlowId)
        {
            _logger.Log(LogLevel.Information, $"Action with id {actionFlowId} was requested");
            return ActionFlowService.get(actionFlowId);
        }

        [HttpDelete(Name = "DeleteActionFlow")]
        public async Task<HttpStatusCode> DeleteAsync(ActionFlow actionFlow)
        {
            _logger.Log(LogLevel.Information, $"Action with id {actionFlow.Id} was requested");
            ActionFlowService.delete(actionFlow);
            return HttpStatusCode.NoContent;
        }
    }
}
