using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWorldTour.Model
{
    public class Trip
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string UserName { get; set; }

        public ICollection<Stop> Stops  { get; set; }
    }
}
