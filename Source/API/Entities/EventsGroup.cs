using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget_Control.Source.API.Entities
{
    public class EventsGroup
    {
        public int Id { get; set; }

        [Column(TypeName="datetime2")]
        public DateTime TimeStamp { get; set; }

        public ICollection<Event> Events { get; set; }
    }
}
