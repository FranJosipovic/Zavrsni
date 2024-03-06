using Microsoft.AspNetCore.Mvc;

namespace Zavrsni.TeamOps
{
    public class ServiceActionResult
    {
        private int StatusCode { get; set; }
        private string StatusMessage { get; set; } = string.Empty;
        private object? Data { get; set; }
        private bool IsSuccess { get; set; }

        private object Value
        {
            get
            {
                return new ResponseModel
                {
                    StatusCode = this.StatusCode,
                    Message = this.StatusMessage,
                    Data = this.Data,
                    IsSuccess = this.IsSuccess,
                };
            }
        }

        public void SetOk(object? data, string message)
        {
            this.Data = data;
            this.IsSuccess = true;
            this.StatusMessage = message;
            this.StatusCode = 200;
        }

        public void SetAuthenticationFailed(string message)
        {
            this.Data = null;
            this.IsSuccess = false;
            this.StatusMessage = message;
            this.StatusCode = 401;
        }

        public void SetResultCreated(object data)
        {
            this.Data = data;
            this.IsSuccess = true;
            this.StatusMessage = "Successfully created";
            this.StatusCode = 201;
        }

        public void SetBadRequest(string message)
        {
            this.Data = null;
            this.IsSuccess = false;
            this.StatusMessage = message;
            this.StatusCode = 400;
        }

        public void SetNotFound(string message)
        {
            this.Data = null;
            this.IsSuccess = false;
            this.StatusMessage = message;
            this.StatusCode = 404;
        }

        public void SetInternalError()
        {
            this.Data = null;
            this.IsSuccess = false;
            this.StatusCode = 500;
            this.StatusMessage = "Internal server error";
        }

        public void UpdateData(object data)
        {
            this.Data = data;
        }

        public IActionResult GetResponseResult() => new ObjectResult(this.Value)
        {
            StatusCode = this.StatusCode,
        };
    }

}

