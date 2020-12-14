using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace giSelleRemastered.Models
{
    public class UserRole
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual IdentityRole Role { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}