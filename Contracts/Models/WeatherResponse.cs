﻿using System.ComponentModel.DataAnnotations;

namespace Contracts.Models
{
    public class WeatherResponse
    {
        [Key]
        public string? City { get; set; }

        public int Temperature { get; set; }

        public int Precipitation { get; set; }

        public double WindSpeed { get; set; }

        public string? Summary { get; set; }
    }
}