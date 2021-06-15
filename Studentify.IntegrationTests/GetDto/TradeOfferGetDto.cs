using System;
using System.Collections.Generic;
using System.Linq;
using Studentify.Models.HttpBody;
using Studentify.Models;
using Studentify.Models.DTO;

namespace Studentify.IntegrationTests.GetDto
{
    class TradeOfferGetDto
    {
        public int Id { get; set; }
        public String EventType { get; set; }
        public String Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public String Description { get; set; }
        public int AuthorId { get; set; }
        public string Price { get; set; }
        public string Offer { get; set; }
        public int? BuyerId { get; set; }

        private static bool CompareWithDto(TradeOfferGetDto left, TradeOfferDto right)
        {
            if (left is null ^ right is null) return false;
            if (left.Name != right.Name) return false;
            if (left.ExpiryDate != right.ExpiryDate) return false;
            if (left.Description != right.Description) return false;
            if (left.Price != right.Price) return false;
            if (left.Offer != right.Offer) return false;
            if (left.BuyerId != right.BuyerId) return false;
            return true;

        }

        private static bool CompareDtos(TradeOfferGetDto left, TradeOfferGetDto right)
        {
            if (left is null ^ right is null) return false;
            if (left.Id != right.Id) return false;
            if (left.EventType != right.EventType) return false;
            if (left.Name != right.Name) return false;
            if (left.CreationDate != right.CreationDate) return false;
            if (left.ExpiryDate != right.ExpiryDate) return false;
            if (left.Description != right.Description) return false;
            if (left.AuthorId != right.AuthorId) return false;
            if (left.Price != right.Price) return false;
            if (left.Offer != right.Offer) return false;
            if (left.BuyerId != right.BuyerId) return false;
            return true;
        }

        public static bool operator ==(TradeOfferGetDto left, TradeOfferDto right)
        {
            return CompareWithDto(left, right);
        }

        public static bool operator !=(TradeOfferGetDto left, TradeOfferDto right)
        {
            return !CompareWithDto(left, right);
        }

        public static bool operator ==(TradeOfferGetDto m1, TradeOfferGetDto m2)
        {
            return CompareDtos(m1, m2);
        }

        public static bool operator !=(TradeOfferGetDto m1, TradeOfferGetDto m2)
        {
            return !CompareDtos(m1, m2);
        }

        public override bool Equals(object o)
        {
            TradeOfferDto dto = o as TradeOfferDto;
            if (dto is null) return false;
            return CompareWithDto(this, dto);
        }
    }
}
