using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PortfolioProject.Models;

namespace PortfolioProject.ViewModel
{
    public class SingleWorkshopModel
    {
        public Workshop _single_workshop { get; set; }
        public List<Workshop> _workshop { get; set; }
        public List<WorkshopImage> _workshop_img { get; set; }
        public List<Contact> _contact { get; set; }

    }
}