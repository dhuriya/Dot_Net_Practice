using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharp.Encapuslation
{
    public class CoffeeMachine
    {
        private int waterLevel;
        private int coffeeBeansLevel;
        private bool isHeated;
        public CoffeeMachine(int water, int coffee)
        {
            waterLevel = water;
            coffeeBeansLevel = coffee;
            isHeated = false;
        }
        private void HeatWater()
        {
            if(!isHeated)
            {
                Console.WriteLine("Heating water...");
                isHeated = true;
            }
        }
        private void GrindBeans(int amount)
        {
            if(coffeeBeansLevel < amount)
            {
                throw new InvalidOperationException("Not enough coffee beans.");
            }
            Console.WriteLine("Grinding coffee beans...");
            coffeeBeansLevel -= amount;
        }
        public void MakeLatte()
        {
            HeatWater();
            GrindBeans(25);
            Console.WriteLine("Making Latte...");
        }
        public void MakeEspresso()
        {
            HeatWater();
            GrindBeans(18);
            Console.WriteLine("Making Espresso...");
        }
        public int BeansLeft()
        {
            return coffeeBeansLevel;
        }
        public int WaterLeft()
        {
            return waterLevel;
        }
    }
}