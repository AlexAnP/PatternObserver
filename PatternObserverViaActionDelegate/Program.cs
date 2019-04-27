using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatternObserverViaActionDelegate
{
    static class Program
    {
        static void Main(string[] args)
        {
            //coздать объект класса Thermostat
            Termostat termostat = new Termostat();

            //coздать объект класса Heater установив начальную температуру равную 30 градусов
            Heater heater = new Heater(20);

            //coздать объект класса Cooler установив начальную температуру равную 40 градусов
            Cooler cooler = new Cooler(30);

            //объект класса Heater - подписаться на событие изменения температуры класса Thermostat
            termostat.TemperatureChanged = heater.Update;

            //объект класса Cooler - подписаться на событие изменения температуры класса Thermostat
            termostat.TemperatureChanged += cooler.Update;

            //эмуляция изменения температуры объекта класса Thermostat
            termostat.EmulateTemperatureChange();

            //объект класса Cooler - отписаться от события изменения температуры класса Thermostat
            termostat.TemperatureChanged -= cooler.Update;

            //эмуляция изменения температуры объекта класса Thermostat на 45 градусов
            termostat.EmulateTemperatureChange();

            Console.Read();
        }
    }

    public class Cooler
    {
        public Cooler(int temperature) => Temperature = temperature;

        public int Temperature { get; private set; }

        public void Update(object sender, EventArgs args)
        {
            var newTemperature = args as TemperatureChangedEventArgs;

            Console.WriteLine(newTemperature.Temperature < Temperature
                        ? $"Cooler: On. Changed to:{Math.Abs(newTemperature.Temperature - Temperature)}. Cooler off."
                        : $"Cooler: Off. Temperature is:{Math.Abs(newTemperature.Temperature - Temperature)}");
        }
    }

    public class Heater
    {
        public Heater(int temperature) => Temperature = temperature;

        public int Temperature { get; private set; }

        public void Update(object sender, EventArgs args)
        {
            var newTemperature = args as TemperatureChangedEventArgs;

            Console.WriteLine(newTemperature.Temperature > Temperature
              ? $"Heater: On. Changed to:{Math.Abs(newTemperature.Temperature - Temperature)}. Heater off."
              : $"Heater: Off. Temperature is:{Math.Abs(newTemperature.Temperature - Temperature)}");
        }
    }

    public sealed class Termostat
    {
        private int currentTemperature;

        private Random random = new Random(Environment.TickCount);

        public Action<object, EventArgs> TemperatureChanged { get; set; }

        public Termostat()
        {
            currentTemperature = 5;
        }

        public int CurrentTemperature
        {
            get => currentTemperature;
            private set
            {
                if (value > currentTemperature)
                {
                    currentTemperature = value;
                    OnTemperatureChanged();
                }
            }
        }

        public void EmulateTemperatureChange()
        {
            this.CurrentTemperature = random.Next(0, 100);
        }

        private void OnTemperatureChanged()
        {
            var local = TemperatureChanged;
            local?.Invoke(this, new TemperatureChangedEventArgs(CurrentTemperature));
            //добавить возможность выполниться независимо от исключения
        }
    }

    public class TemperatureChangedEventArgs : EventArgs
    {
        public int Temperature { get; set; }
        public TemperatureChangedEventArgs(int temperature)
        {
            Temperature = temperature;
        }
    }
}