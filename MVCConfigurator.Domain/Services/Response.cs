using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCConfigurator.Domain.Services
{
    public class Response<T>
    {
        public T Entity { get; set; }
        public ErrorCode Error { get; set; }
        public bool Success { get{ return Error == ErrorCode.None; }}
    }
    public enum ErrorCode
    {
        None,
        InvalidState,
        DuplicateEntity,
        InvalidLogin
    }
}
