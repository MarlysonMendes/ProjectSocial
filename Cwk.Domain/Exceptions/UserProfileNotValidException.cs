using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwkSocial.Domain.Exceptions
{
    public class UserProfileNotValidException : Exception
    {
        public UserProfileNotValidException()
        {
            ValidationErros = new List<string>();
        }

        public UserProfileNotValidException(string message) : base(message)
        {
            ValidationErros = new List<string>();
        }
    
        public List<string> ValidationErros { get; set; }
    }
}
