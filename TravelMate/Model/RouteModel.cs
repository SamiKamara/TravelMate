using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private string startLocation;
        private string endLocation;

        public string Mode { get => mode; set => mode = value; }
        public string StartTime { get => startTime; set => startTime = value; }
        public string Duration { get => duration; set => duration = value; }
        public string StartLocation 
        { 
            get => startLocation; 
            set => startLocation = ValidateLocation(value) ? value : "Unknown"; 
        }
        public string EndLocation 
        { 
            get => endLocation; 
            set => endLocation = ValidateLocation(value) ? value : "Unknown";
        }
        private bool ValidateLocation(string address)
        {
            return !string.IsNullOrEmpty(address) && address.Length > 1;
        }
    }
    public class RouteModel
    {
        private string startTime;
        private string arrivalTime;
        private string from;
        private string to;
        private double routeMatchpercentage;
        private int inputTemperature;
        private int resultTemperature;
        private int inputRainChance;
        private int resultRainChance;
        private int inputCloudiness;
        private int resultCloudiness;
        private double inputWindSpeed;
        private double resultWindSpeed;
        private string formattedDate;
        private TimeSpan totalTravelTime;
        private string formattedTotalTravelTime;
        private DateTime date;

        public string StartTime { get => startTime; set => startTime = value; }
        public string ArrivalTime { get => arrivalTime; set => arrivalTime = value; }
        public string From { get => from; set => from = value; }
        public string To { get => to; set => to = value; }
        public double RouteMatchpercentage { get => routeMatchpercentage; set => routeMatchpercentage = value; }
        public int InputTemperature { get => inputTemperature; set => inputTemperature = value; }
        public int ResultTemperature { get => resultTemperature; set => resultTemperature = value; }
        public int InputRainChance { get => inputRainChance; set => inputRainChance = value; }
        public int ResultRainChance { get => resultRainChance; set => resultRainChance = value; }
        public int InputCloudiness { get => inputCloudiness; set => inputCloudiness = value; }
        public int ResultCloudiness { get => resultCloudiness; set => resultCloudiness = value; }
        public double InputWindSpeed { get => inputWindSpeed; set => inputWindSpeed = value; }
        public double ResultWindSpeed { get => resultWindSpeed; set => resultWindSpeed = value; }
        public string FormattedDate { get => formattedDate;  set => formattedDate = value; }
        public TimeSpan TotalTravelTime { get => totalTravelTime;  set => totalTravelTime = value; }
        public string FormattedTotalTravelTime { get => formattedTotalTravelTime; private set => formattedTotalTravelTime = value; }

        public int RainChanceMatchPercentage
        {
            get
            {
                return CalculatePercentageDifference(InputRainChance, ResultRainChance);
            }
        }

        public int CloudinessMatchPercentage
        {
            get
            {
                return CalculatePercentageDifference(InputCloudiness, ResultCloudiness);
            }
        }

        private readonly double toleranceForWindSpeed = 10.0; // If over 10 m/s, the match percentage is 0
        private readonly double toleranceForTemperature = 20.0; // If over 20 degrees, the match percentage is 0

        public int WindSpeedMatchPercentage
        {
            get
            {
                return CalculatePercentageCloseness(InputWindSpeed, ResultWindSpeed, toleranceForWindSpeed);
            }
        }

        public int TemperatureMatchPercentage
        {
            get
            {
                return CalculatePercentageCloseness(InputTemperature, ResultTemperature, toleranceForTemperature);
            }
        }

        static int CalculatePercentageCloseness(double inputValue, double resultValue, double threshold)
        {
            // Calculate the absolute difference between the temperatures
            double difference = Math.Abs(inputValue - resultValue);

            // Calculate the percentage closeness based on the difference and threshold
            double percentageCloseness = 100.0 - (difference / threshold) * 100.0;

            // Ensure the result is within the range [0, 100] and convert to integer
            int result = (int)Math.Max(0, Math.Min(100, percentageCloseness));

            return result;
        }

        static int CalculatePercentageDifference(int percentage1, int percentage2)
        {
            // Ensure the percentages are within the valid range (0-100)
            percentage1 = Math.Max(0, Math.Min(100, percentage1));
            percentage2 = Math.Max(0, Math.Min(100, percentage2));

            // Calculate the absolute difference
            int absoluteDifference = Math.Abs(percentage1 - percentage2);

            // Calculate the adjusted percentage difference
            int adjustedPercentageDifference = 100 - absoluteDifference;

            return adjustedPercentageDifference;
        }

        public void CalculateTotalTravelTime(DateTimeOffset? startTime, DateTimeOffset? endTime)
        {
            TotalTravelTime = endTime.Value - startTime.Value;
            FormattedTotalTravelTime = TotalTravelTime.ToString(@"hh\:mm");
        }
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
                return "Tomorrow, " + date.ToString("dd MMM yyyy");
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

