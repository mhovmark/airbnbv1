using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirBnbProject
{
    class City
    {
        private string cityName;
        private int population;
        private int avgIncome;
        private int touristsPerYear;
        private List<Accommodation> accommodations;
        private int count;
        private double avgRoomPrice;

        public City()
        {

        }

        public string CityName { get => cityName; set => cityName = value; }
        public int Population { get => population; set => population = value; }
        public int AvgIncome { get => avgIncome; set => avgIncome = value; }
        public int TouristsPerYear { get => touristsPerYear; set => touristsPerYear = value; }
        internal List<Accommodation> Accommodations { get => accommodations; set => accommodations = value; }
    }
}
