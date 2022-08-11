using Microsoft.Extensions.Configuration;
using Oiski.School.Web.VivaLaDaper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    public class SampleContext : DapperContext
    {
        public SampleContext(IConfiguration config) : base(config)
        {
        }
    }
}
