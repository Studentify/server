using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Mime;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Linq;

using NUnit.Framework;
using Studentify.Models.HttpBody;
using Studentify.Models;
using Studentify.IntegrationTests.GetDto;
using Studentify.Models.Messages;

namespace Studentify.IntegrationTests.GetDto
{
    class MessageGetDto
    {
        public int Id { get; set; }
        public StudentifyAccount Author { get; set; }
        public DateTime Date { get; set; }
        public string Content{ get; set; }
        public bool IsViewed { get; set; }
        public int ThreadId { get; set; }
        
        private static bool CompareGetDtos(MessageGetDto left, MessageGetDto right)
        {
            if (left is null ^ right is null) return false;
            if (left.Id != right.Id) return false;
            if (left.Author.Id != right.Author.Id) return false;
            if (left.Date != right.Date) return false;
            if (left.Content != right.Content) return false;
            if (left.IsViewed != right.IsViewed) return false;
            if (left.ThreadId != right.ThreadId) return false;
            return true;
        }

        private static bool CompareWithDto(MessageGetDto left, MessageDto right)
        {
            if (left is null ^ right is null) return false;
            if (left.Content != right.Content) return false;
            if (left.ThreadId != right.ThreadId) return false;
            return true;
        }

        public static bool operator ==(MessageGetDto left, MessageGetDto right)
        {
            return CompareGetDtos(left, right);
        }

        public static bool operator !=(MessageGetDto left, MessageGetDto right)
        {
            return !CompareGetDtos(left, right);
        }

        public static bool operator ==(MessageGetDto left, MessageDto right)
        {
            return CompareWithDto(left, right);
        }

        public static bool operator !=(MessageGetDto left, MessageDto right)
        {
            return !CompareWithDto(left, right);
        }
    }
}
