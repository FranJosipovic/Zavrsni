using Microsoft.AspNetCore.Mvc;

namespace Zavrsni.TeamOps
{
    public class DbActionResult
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public object? Data { get; set; }
        public bool IsSuccess { get; set; }

        private object Value
        {
            get
            {
                return new
                {
                    StatusCode = this.StatusCode,
                    StatusMessage = this.StatusMessage,
                    Data = this.Data,
                    IsSuccess = this.IsSuccess,
                };
            }
        }

        public void SetOk(object data)
        {
            this.Data = data;
            this.IsSuccess = true;
            this.StatusMessage = "Successfully found";
            this.StatusCode = 200;
        }

        public void SetResultCreated(object data)
        {
            this.Data = data;
            this.IsSuccess = true;
            this.StatusMessage = "Successfully created";
            this.StatusCode = 201;
        }

        public void SetNotFound()
        {
            this.Data = null;
            this.IsSuccess = false;
            this.StatusMessage = "Object not found";
            this.StatusCode = 404;
        }

        public void SetInternalError()
        {
            this.Data = null;
            this.IsSuccess = false;
            this.StatusCode = 500;
            this.StatusMessage = "Internal server error";
        }

        public IActionResult GetResponseResult() => new ObjectResult(this.Value);
    }
}
