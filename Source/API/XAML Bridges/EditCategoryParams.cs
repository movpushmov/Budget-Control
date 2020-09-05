using Salary_Control.Source.API.Entities;
using Salary_Control.Source.API.XAML_Bridges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salary_Control.Source.API
{
    public class EditCategoryParams
    {
        public CategoriesList List { get; set; }
        public EventCategory Category { get; set; }
    }
}
