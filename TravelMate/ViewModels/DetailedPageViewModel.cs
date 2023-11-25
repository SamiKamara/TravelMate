using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microcharts;
using Newtonsoft.Json.Linq;
using TravelMate.Services;
using TravelMate.ViewModels;

namespace TravelMate
{
    public class DetailedPageViewModel : ViewModelBase
    {
        public ChartEntry[] entries = new[]
        {
            new ChartEntry(212)
            {
                Label = "Test1",
                ValueLabel = "212",
                Color = SkiaSharp.SKColor.Parse("#FF1493")
            },
            new ChartEntry(248)
            {
                Label = "Test2",
                ValueLabel = "248",
                Color = SkiaSharp.SKColor.Parse("#FF1493")
            },
            new ChartEntry(129)
            {
                Label = "Test3",
                ValueLabel = "129",
                Color = SkiaSharp.SKColor.Parse("#FF1493")
            },
            new ChartEntry(69)
            {
                Label = "Test4",
                ValueLabel = "69",
                Color = SkiaSharp.SKColor.Parse("#FF1493")
            }
        };
    }
}