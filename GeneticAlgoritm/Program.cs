using System;
using System.Reflection;
using   GeneticAlgoritm.Example;
namespace GeneticAlgoritm {
    internal class Program {
        
        public static void Main(string[] args) {
            Console.Title = "Genetic algoritm v" +Assembly.GetExecutingAssembly().GetName().Version;
            StringExample string_example = new StringExample();
            string_example.run();
        }
    }
}