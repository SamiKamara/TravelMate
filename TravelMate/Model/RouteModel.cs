using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelMate.Model
{

    public class TransportMode
    {
        private string mode;
        private string startTime;
        private string duration; 

        public string Mode { get => mode; set => mode = value; }
        public string StartTime { get => startTime; set => startTime = value; }
        public string Duration { get => duration; set => duration = value; }
    }
    public class RouteModel
    {
        
        public string StartTime { get; set; }
        public string ArrivalTime { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public double RouteMatchpercentage { get; set; }
        public int InputTemperature { get; set; }
        public int ResultTemperature { get; set; }
        public int InputRainChance { get; set; }
        public int ResultRainChance { get; set; }
        public int InputCloudiness { get; set; }
        public int ResultCloudiness { get; set; }
        public double InputWindSpeed { get; set; }
        public double ResultWindSpeed { get; set; }
        public string FormattedDate { get; private set; }
        public TimeSpan TotalTravelTime { get; private set; }
        public string FormattedTotalTravelTime { get; private set; }  

        public void CalculateTotalTravelTime(DateTimeOffset? startTime, DateTimeOffset? endTime)
        {
            TotalTravelTime = endTime.Value - startTime.Value;
            FormattedTotalTravelTime = TotalTravelTime.ToString(@"hh\:mm");
        }

        private DateTime date;
        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                FormattedDate = GetFormattedDate();
            }
        }
        private string GetFormattedDate()
        {
            DateTime today = DateTime.Today;
            DateTime tomorrow = DateTime.Today.AddDays(1);

            if (Date.Date == today.Date)
            {
                return "Today, " + date.ToString("dd MMM yyyy");
            }
            else 
            {
                return "Tomorrow " + date.ToString("dd MM yyyy");
            }
        }
        public List<TransportMode> TransportModes { get; set; }

        public RouteModel()
        {

            TransportModes = new List<TransportMode>(); 
        }
        public void AssignWeatherData(string[] weatherValues)
        {
            
           
            int.TryParse(weatherValues[0], out int temp);
            int.TryParse(weatherValues[1], out int rainChance);
            int.TryParse(weatherValues[2], out int cloudiness);
            double.TryParse(weatherValues[3], out double windSpeed);

            ResultTemperature = temp - 50;
            ResultRainChance = rainChance;
            ResultCloudiness = cloudiness;
            ResultWindSpeed = windSpeed;
            
        }
    }

}

