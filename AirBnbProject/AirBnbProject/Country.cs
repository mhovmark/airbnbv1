using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirBnbProject
{
    class Country
    {
        private string countryName;
        private int population;
        private int GDPPerCap;
        private List<City> city;

        public Country()
        {

        }

        public int Population { get => population; set => population = value; }
        public int GDPPerCap1 { get => GDPPerCap; set => GDPPerCap = value; }
        public string CountryName { get => countryName; set => countryName = value; }
        internal List<City> City { get => city; set => city = value; }
    }
}
