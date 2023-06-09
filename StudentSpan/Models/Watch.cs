using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentSpan.Models
{
    public class Watch
    {
        public int ID { get; set; }
        
        public DateTime WatchedOn { get; set; }
        public string Text { get; set; }

        public string WatchedBy
        {
            get
            {
                return string.Concat(ApplicationUser.FirstName,
              " ", ApplicationUser.LastName);
            }
        }

        public virtual Student Student { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}

