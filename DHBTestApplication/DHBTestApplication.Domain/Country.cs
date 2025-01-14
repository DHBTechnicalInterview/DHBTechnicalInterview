using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHBTestApplication.Domain
{
    public class Country
    {
        public string Name { get; set; }
        //fix bug: do validation, but optional
        [Range(0, int.MaxValue, ErrorMessage = "population should be non-negative integer.")]
        public int Population { get; set;}

        [Range(0.0001, double.MaxValue, ErrorMessage = "area should be greater than zero.")]
        public double Area { get; set; }

        public double CalculateDensity()
        {
            //fix bug: before it is Area / Poputaion, the formular is wong
            if (Area <=0 || Area < 0.0001) throw new InvalidOperationException("area should be greater than zero.");
            if(Population < 0) throw new InvalidOperationException("population should be non-negative integer.");
            return  (double)Population / Area;
        }


    }
}
