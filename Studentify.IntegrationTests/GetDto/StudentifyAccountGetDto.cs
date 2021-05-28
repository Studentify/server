using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Studentify.Models.HttpBody;

namespace Studentify.IntegrationTests.GetDto
{
    class StudentifyAccountGetDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        private static bool CompareWithDto(StudentifyAccountGetDto a, RegisterDto dto)
        {
            if (a is null) return false;
            if (dto is null) return false;
            if (a.FirstName != dto.FirstName) return false;
            if (a.LastName != dto.LastName) return false;
            if (a.Username != dto.Username) return false;
            if (a.Email != dto.Email) return false;
            return true;
        }

        public static bool operator ==(StudentifyAccountGetDto a, RegisterDto dto)
        {
            return CompareWithDto(a, dto);
        }

        public static bool operator !=(StudentifyAccountGetDto a, RegisterDto dto)
        {
            return !CompareWithDto(a, dto);
        }

        public override bool Equals(object o)
        {
            RegisterDto dto = o as RegisterDto;
            if (dto is null) return false;
            return CompareWithDto(this, dto);
        }
    }
}
