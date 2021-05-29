using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Studentify.Models.HttpBody;

namespace Studentify.IntegrationTests.GetDto
{
    class LoginGetDto
    {
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
        public StudentifyAccountGetDto User { get; set; }
    }
}
