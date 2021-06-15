using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Studentify.Models.HttpBody;
using Studentify.Models;

namespace Studentify.IntegrationTests.GetDto
{
    class ThreadGetDto
    {
        public int Id { get; set; }
        public int ReferencedEventId { get; set; }
        public StudentifyAccount UserAccount { get; set; }

        private static bool CompareGetDtos(ThreadGetDto t1, ThreadGetDto t2)
        {
            if (t1 is null ^ t2 is null) return false;
            if (t1.Id != t2.Id) return false;
            if (t1.ReferencedEventId != t2.ReferencedEventId) return false;
            if (t1.UserAccount.Id != t2.UserAccount.Id) return false;
            return true;
        }

        public static bool operator==(ThreadGetDto t1, ThreadGetDto t2)
        {
            return CompareGetDtos(t1, t2);
        }

        public static bool operator !=(ThreadGetDto t1, ThreadGetDto t2)
        {
            return !CompareGetDtos(t1, t2);
        }
    }
}
