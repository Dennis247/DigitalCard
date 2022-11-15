using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIgitalCard.Lib.Models
{
    public class AppSettings
    {
        public string SecurityIv { get; set; }
        public string Securitykey { get; set; }
        public string ApiBaseUrl { get; set; }
    }
}
