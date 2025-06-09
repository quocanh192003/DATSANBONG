using System.Net;

namespace DATSANBONG.Models
{
    public class APIResponse
    {
        private List<string> _errorMessages;

        public APIResponse()
        {
            _errorMessages = new List<string>();
        }

        public HttpStatusCode Status { get; set; }

        public bool IsSuccess { get; set; } = true;

        public List<string> ErrorMessages
        {
            get => _errorMessages ??= new List<string>();
            set => _errorMessages = value ?? new List<string>();
        }

        public object Result { get; set; }
    }
}
