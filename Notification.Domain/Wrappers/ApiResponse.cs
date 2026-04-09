using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Domain.Wrappers
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public static ApiResponse Fail(string msg) => new() { Success = false, Error = msg };
        public static ApiResponse Ok() => new() { Success = true };
    }
}
