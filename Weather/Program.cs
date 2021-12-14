using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Weather
{
    class Program
    {
        static void Main(string[] args)
        {
            float min = 0;
            int max = 0;
            int datebase;
            DateTime DateNull = new DateTime(1970, 1, 1);
            string url = "https://api.openweathermap.org/data/2.5/onecall?lat=58.0174&lon=56.2855&exclude=hourly,minutely&units=metric&appid=91df9e1c13ae5ab6bd45b1fe53d8f28f";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string response;

            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
            }
            Weather weather = JsonConvert.DeserializeObject<Weather>(response);
            weather.daily.RemoveAt(weather.daily.Count - 1);
            weather.daily.RemoveAt(weather.daily.Count - 1);
            weather.daily.RemoveAt(weather.daily.Count - 1);
            foreach (var a in weather.daily)
            {
                Console.WriteLine(DateNull.AddSeconds(a.dt) + " В этот день в Перми температура ночью фактическая " + a.temp.night 
                    + " ощущаемая " + a.feels_like.night);
                Console.WriteLine(" рассвет в " + DateNull.AddSeconds(a.sunrise) + " закат " + DateNull.AddSeconds(a.sunset));
                Console.WriteLine();
            }
            
            min = weather.daily.Select(x => x.temp.night - x.feels_like.night).Min();
            Console.WriteLine("Минимальная разница между температурой фактической и ощущаемой ночью " + min + "°C");
            Console.WriteLine();
            max = weather.daily.Select(x => x.sunset - x.sunrise).Max();
            datebase = weather.daily.Where(x => x.sunset - x.sunrise == max).Select(x => x.dt).Max();
            DateTime date = DateNull.AddSeconds(datebase);
            Console.WriteLine("Максимальная продолжительность светового дня было в " + date + " и составляла " + TimeSpan.FromSeconds(max));
        }
    }
    public class Temperature
    {
        public float night { get; set; }
    }
    public class Feels
    {
        public float night { get; set; }
    }
    public class Day
    {
        public int dt { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
        public Temperature temp { get; set; }
        public Feels feels_like { get; set; }
    }
    public class Weather
    {
        public virtual List<Day> daily { get; set; }
    }
}
