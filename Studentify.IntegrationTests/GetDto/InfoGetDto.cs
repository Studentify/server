using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Studentify.Models.HttpBody;
using Studentify.Models;
using Studentify.Models.StudentifyEvents;

namespace Studentify.IntegrationTests.GetDto
{
    class InfoGetDto
    {
        public int Id { get; set; }
        public string EventType { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public InfoCategory Category { get; set; }

        private static bool CompareWithDto(InfoGetDto getDto, InfoDto dto)
        {
            //if (getDto is null || dto is null) return false;
            //if (getDto.ExpiryDate != dto.ExpiryDate) return false;
            return true;
        }

        private static bool ComapreGetDtos(InfoGetDto a, InfoGetDto b)
        {
            if (a is null || b is null) return false;
            if (a.Id != b.Id) return false;
            if (a.EventType != b.EventType) return false;
            if (a.CreationDate != b.CreationDate) return false;
            if (a.ExpiryDate != b.ExpiryDate) return false;
            if (a.Description != b.Description) return false;
            if (a.AuthorId != b.AuthorId) return false;
            if (a.Category != b.Category) return false;
            return true;
        }

        public static bool operator ==(InfoGetDto getDto, InfoDto dto)
        {
            return CompareWithDto(getDto, dto);
        }

        public static bool operator !=(InfoGetDto getDto, InfoDto dto)
        {
            return !CompareWithDto(getDto, dto);
        }

        public static bool operator ==(InfoGetDto a, InfoGetDto b)
        {
            return ComapreGetDtos(a, b);
        }

        public static bool operator !=(InfoGetDto a, InfoGetDto b)
        {
            return !ComapreGetDtos(a, b);
        }

        public override bool Equals(object o)
        {
            InfoDto dto = o as InfoDto;
            if (dto is null) return false;
            return CompareWithDto(this, dto);
        }
    }
}
