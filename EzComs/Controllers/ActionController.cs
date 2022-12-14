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
    public class ActionController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public ActionController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "CreateAction")]
        public async Task<IAction> PostAsync(IAction action)
        {
            _logger.Log(LogLevel.Information, $"Creation of action type {action.GetType().Name} was requested");
            return ActionService.create(action);
        }

        [HttpPut(Name = "UpdateAction")]
        public async Task<IAction> PutAsync(IAction action)
        {
            _logger.Log(LogLevel.Information, $"Update of action type {action.GetType().Name} was requested");
            return ActionService.create(action);
        }


        [HttpGet(Name = "GetAction")]
        public async Task<IAction> GetAsync(string actionId)
        {
            _logger.Log(LogLevel.Information, $"Action with id {actionId} was requested");
            return ActionService.get(actionId);
        }

        [HttpGet(Name = "GetActionsByFlow")]
        public async Task<IEnumerable<IAction>> GetByFlowAsync(string flowId)
        {
            _logger.Log(LogLevel.Information, $"Flow with id {flowId} was requested");
            return ActionService.getAllByFlow(flowId);
        }

        [HttpDelete(Name = "DeleteAction")]
        public async Task<HttpStatusCode> DeleteAsync(IAction action)
        {
            _logger.Log(LogLevel.Information, $"Action with id {action.Id} was requested");
            ActionService.delete(action);
            return HttpStatusCode.NoContent;
        }
    }
}
