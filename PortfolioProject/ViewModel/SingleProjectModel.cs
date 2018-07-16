using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PortfolioProject.Models;

namespace PortfolioProject.ViewModel
{
    public class SingleProjectModel
    {
        public Project _single_project { get; set; }
        public List<Project> _project { get; set; }
        public List<ProjectImage> _project_img { get; set; }
        public List<Contact> _contact { get; set; }

    }
}