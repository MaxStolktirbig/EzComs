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
        public async Task<ActionFlow?> PostAsync(ActionFlow actionFlow)
        {
            _logger.Log(LogLevel.Information, $"Creation of ActionFlow type {actionFlow.GetType().Name} was requested");
            try
            {
                return ActionFlowService.create(actionFlow);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }

        [HttpPut(Name = "UpdateActionFlow")]
        public async Task<ActionFlow?> PutAsync(ActionFlow actionFlow)
        {
            _logger.Log(LogLevel.Information, $"Update of ActionFlow type {actionFlow.GetType().Name} was requested");
            
            try
            {
                return ActionFlowService.save(actionFlow);
            } catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;  
        }


        [HttpGet(Name = "GetActionFlow")]
        public async Task<ActionFlow?> GetAsync(string actionFlowId)
        {
            _logger.Log(LogLevel.Information, $"ActionFlow with id {actionFlowId} was requested");
            try
            {
                return ActionFlowService.get(actionFlowId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }

        [HttpDelete(Name = "DeleteActionFlow")]
        public async Task<HttpStatusCode> DeleteAsync(ActionFlow actionFlow)
        {
            _logger.Log(LogLevel.Information, $"ActionFlow with id {actionFlow.id} was requested");
            ActionFlowService.delete(actionFlow);
            return HttpStatusCode.NoContent;
        }
    }
}
