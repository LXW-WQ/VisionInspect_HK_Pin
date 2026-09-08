using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vison_Inspect_System._1_Web.WebService.Models
{
  public  class ApiResponseInfo
    {
        public bool IsSuccess { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public List<object> Content { get; set; }
    }
}
