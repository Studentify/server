using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Studentify.Models.DTO
{
    public class SkillDto
    {
        public string Name { get; set; }
        public int Rate { get; set; }
        public int OwnerId { get; set; }
    }
}
