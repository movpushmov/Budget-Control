﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Salary_Control.Source.API.Entities
{
    public class EventCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }

        public ICollection<Event> Events { get; set; }
    }
}