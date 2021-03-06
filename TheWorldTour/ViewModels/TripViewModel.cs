﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWorldTour.ViewModels
{
    public class TripViewModel
    {
        [Required]
        [StringLength(100, MinimumLength =3)]
        public string Name { get; set; }
        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
