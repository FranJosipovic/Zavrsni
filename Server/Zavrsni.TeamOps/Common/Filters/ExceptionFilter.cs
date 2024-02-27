using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Data.Entity.Core;

namespace Zavrsni.TeamOps.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var responseModel = new ResponseModel
            {
                Data = null,
                IsSuccess = false,
                Message = "Internal server error",
                StatusCode = 500
            };
            switch (context.Exception)
            {
                case ObjectNotFoundException e:
                    responseModel.Message = e.Message;
                    responseModel.StatusCode = 404;
                    context.Result = new ObjectResult(responseModel)
                    {
                        StatusCode = responseModel.StatusCode,
                    };
                    break;
                default:
                    context.Result = new ObjectResult(responseModel)
                    {
                        StatusCode = 500
                    };
                    break;
            }
        }
    }
}
