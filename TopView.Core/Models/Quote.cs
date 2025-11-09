using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopView.Core.Models
{
    public class Quote
    {
        public decimal c { get; set; } // Current price
        public decimal d { get; set; } // Day change
        public decimal dp { get; set; } // Day change percent
        public decimal pc { get; set; } // Previous close price
        public long t { get; set; } // Timestamp (UNIX)
    }

}
