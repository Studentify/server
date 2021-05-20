﻿using System;
using NetTopologySuite.Geometries;

namespace Studentify.Models.HttpBody
{
    public class EventDTO
    {
        public string Name { get; set; }
        public DateTime ExpiryDate { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Description { get; set; }
    }
}