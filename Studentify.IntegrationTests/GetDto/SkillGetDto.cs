using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Studentify.Models.HttpBody;
using Studentify.Models;
using Studentify.Models.DTO;

namespace Studentify.IntegrationTests.GetDto
{
    class SkillGetDto
    {
        public int Id{ get; set; }
        public string Name { get; set; }
        public int Rate { get; set; }
        public int OwnerId { get; set; }
        public StudentifyAccountGetDto Owner { get; set; }

        private static bool CompareGetDtos(SkillGetDto left, SkillGetDto right)
        {
            if (left is null ^ right is null) return false;
            if (left.Id != right.Id) return false;
            if (left.Name != right.Name) return false;
            if (left.Rate != right.Rate) return false;
            if (left.OwnerId != right.OwnerId) return false;
            return true;
        }

        private static bool CompareWithDto(SkillGetDto left, SkillDto right)
        {
            if (left is null ^ right is null) return false;
            if (left.Name != right.Name) return false;
            if (left.Rate != right.Rate) return false;
            if (left.OwnerId != right.OwnerId) return false;
            return true;
        }

        public static bool operator ==(SkillGetDto left, SkillGetDto right)
        {
            return CompareGetDtos(left, right);
        }

        public static bool operator !=(SkillGetDto left, SkillGetDto right)
        {
            return !CompareGetDtos(left, right);
        }

        public static bool operator ==(SkillGetDto left, SkillDto right)
        {
            return CompareWithDto(left, right);
        }

        public static bool operator !=(SkillGetDto left, SkillDto right)
        {
            return !CompareWithDto(left, right);
        }
    }
}
