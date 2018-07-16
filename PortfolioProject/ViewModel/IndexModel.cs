using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PortfolioProject.Models;

namespace PortfolioProject.ViewModel
{
    public class IndexModel
    {
        public List<Slider> _slider { get; set; }
        public Slider _first_img { get; set; }

        public List<AnimeImage> _anime { get; set; }

        public List<Project> _project { get; set; }
        public List<Workshop> _workshop { get; set; }
        public List<About> _about { get; set; }
        public List<Press> _press { get; set; }
        public List<Contact> _contact { get; set; }

    }
}