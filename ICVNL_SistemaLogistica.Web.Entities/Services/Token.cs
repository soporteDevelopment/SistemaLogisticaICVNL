﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities.Services
{
    public class Token
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string infofin_token { get; set; }
        public string infofin_usuario { get; set; }
        public string issued { get; set; }
        public string expires { get; set; }
    }

}
