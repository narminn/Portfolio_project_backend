using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PortfolioProject.Models;

namespace PortfolioProject.ViewModel
{
    public class SinglePressModel
    {
        public Press _single_press { get; set; }
        public List<Press> _press { get; set; }
        public List<Contact> _contact { get; set; }

    }
}