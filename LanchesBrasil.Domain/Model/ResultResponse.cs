namespace LanchesBrasil.Commons.Model
{
    public class ResultResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? Body { get; set; }
        public int StatusCode { get; set; }

        public ResultResponse()
        {

        }

        public ResultResponse(bool Success, string Message)
        {
            this.Message = Message;
            this.Success = Success;
        }

        public ResultResponse(bool Success, string Message, string Body)
        {
            this.Message = Message;
            this.Success = Success;
            this.Body = Body;
        }

        public ResultResponse(bool Success, string Message, string Body, int StatusCode)
        {
            this.Message = Message;
            this.StatusCode = StatusCode;
            this.Body = Body;
            this.Success = Success;
        }
    }
}
