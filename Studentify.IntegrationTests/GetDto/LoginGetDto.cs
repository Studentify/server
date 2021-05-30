using System;

namespace Studentify.IntegrationTests.GetDto
{
    class LoginGetDto
    {
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
        public StudentifyAccountGetDto User { get; set; }
    }
}
