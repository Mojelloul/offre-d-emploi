using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace offre_d_emploi.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [Display(Name="type d'emploi")]
        public string CategoryName { get; set; }
        [Required]
        [Display(Name="Description type d'emploi")]
        public string CategoryDescription { get; set; }

        public virtual ICollection<Job> Jobs { get; set; }
    }
}