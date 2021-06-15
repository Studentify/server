using System;
using System.Collections.Generic;
using System.Linq;
using Studentify.Models.HttpBody;
using Studentify.Models;

namespace Studentify.IntegrationTests.GetDto
{
    class StudentifyEventGetDto
    {
        public int Id { get; set; }
        public String EventType { get; set; }
        public String Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public String Description { get; set; }
        public int AuthorId { get; set; }

        private static bool CompareWithDto(StudentifyEventGetDto m, StudentifyEventDto dto)
        {
            if (m is null) return false;
            if (dto is null) return false;
            if (m.Name != dto.Name) return false;
            if (m.ExpiryDate != dto.ExpiryDate) return false;
            if (m.Description != dto.Description) return false;
            if (m.Description != dto.Description) return false;
            return true;
        }

        private static bool Compare(StudentifyEventGetDto m1, StudentifyEventGetDto m2)
        {
            if (m1 is null) return false;
            if (m2 is null) return false;
            if (m1.Id != m2.Id) return false;
            if (m1.EventType != m2.EventType) return false;
            if (m1.Name != m2.Name) return false;
            if (m1.CreationDate != m2.CreationDate) return false;
            if (m1.ExpiryDate != m2.ExpiryDate) return false;
            if (m1.Description != m2.Description) return false;
            if (m1.AuthorId != m2.AuthorId) return false;
            return true;
        }

        public static bool operator ==(StudentifyEventGetDto a, StudentifyEventDto dto)
        {
            return CompareWithDto(a, dto);
        }

        public static bool operator !=(StudentifyEventGetDto a, StudentifyEventDto dto)
        {
            return !CompareWithDto(a, dto);
        }

        public static bool operator ==(StudentifyEventGetDto m1, StudentifyEventGetDto m2)
        {
            return Compare(m1, m2);
        }

        public static bool operator !=(StudentifyEventGetDto m1, StudentifyEventGetDto m2)
        {
            return !Compare(m1, m2);
        }

        public override bool Equals(object o)
        {
            MeetingDto dto = o as MeetingDto;
            if (dto is null) return false;
            return CompareWithDto(this, dto);
        }
    }
}
