using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwkSocial.Application.Models
{
    public class OperationResult<T>
    {
        public T PayLoad { get; set; }
        public bool IsErro { get; set; }
        public List<Error> Erros { get; set; } = new List<Error>();
    }
}
