using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication5.Models;

namespace offre_d_emploi.Models
{
    public class Job
    {
        public int Id { get; set; }
        [DisplayName("Nom d'emploi")]
        public string JobTitle { get; set; }
        [DisplayName("Discription d'emploi")]
        [AllowHtml]
        public string JobContent { get; set; }
        [DisplayName("Photo d'emploi")]
        public string JobImage { get; set; }
        [DisplayName("Type d'emploi")]
        public int CategoryId { get; set; }
        public string UserID { get; set; }
        public virtual Category Category { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}