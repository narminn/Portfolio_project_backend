using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PortfolioProject.Models;

namespace PortfolioProject.ViewModel
{
    public class AboutModel
    {
        public List<About> _about { get; set; }
        public List<AboutImage> _about_img { get; set; }
        public List<Contact> _contact { get; set; }

    }
}