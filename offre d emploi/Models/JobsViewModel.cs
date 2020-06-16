using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace offre_d_emploi.Models
{
    public class JobsViewModel
    {
        public string JobTitle { get; set; }
        public IEnumerable<ApplyForJob> Items { get; set; }
    }
}