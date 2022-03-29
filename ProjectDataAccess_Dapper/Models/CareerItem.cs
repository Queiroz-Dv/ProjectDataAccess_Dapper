﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDataAccess_Dapper.Models
{
    public class CareerItem
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        // Tipo complexo
        public Course Course { get; set; }
    }
}
