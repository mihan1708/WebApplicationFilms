﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationFilms.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Film Film { get; set; }
    }
}