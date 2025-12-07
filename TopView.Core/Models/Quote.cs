using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopView.Core.Models
{
    public record Quote
    (
        decimal c, // Current price
        decimal d, // Day change
        decimal dp, // Day change percent
        decimal pc, // Previous close price
        long t // Timestamp (UNIX)
    );
}
