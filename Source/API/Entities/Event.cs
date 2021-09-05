using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget_Control.Source.API.Entities
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }

        public int CategoryId { get; set; }
        public EventCategory Category { get; set; }

        public int EventGroupId { get; set; }
        public EventsGroup EventsGroup { get; set; }
    }
}
