using System.ComponentModel.DataAnnotations.Schema;

namespace Budget_Control.Source.API.Entities
{
    public class UserTask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
        public string ImagePath { get; set; }

        public bool IsCompleted { get; set; }

        [NotMapped] // field for ProgressBar "Balance" property.
        public int CurrentAmount { get; set; }
    }
}
