using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICVNL_SistemaLogistica.API.Models
{
    public class RequestApi<T>
    {
        public bool ExecutionOK;
        public string Message;
        public T Data;
        public int NumRows;
    }
}