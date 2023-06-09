using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentSpan.Models
{
    public class Comment
    {
        public int ID { get; set; }
        //public int StudentID { get; set; }
        //public int ApplicationUserID { get; set; }
        public DateTime EnteredOn { get; set; }
        public string Text { get; set; }

        public string EnteredBy
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
