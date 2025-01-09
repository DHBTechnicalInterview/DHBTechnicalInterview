using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHBTestApplication.Domain
{
    public class Country
    {
        public string Name { get; set; }
        public int Population { get; set; }
        public double Area { get; set; }

        public double CalculateDensity()
        {
            return Area / Population;
        }
    }
}
