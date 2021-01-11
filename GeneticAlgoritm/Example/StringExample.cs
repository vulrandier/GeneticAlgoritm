using System;
using GeneticAlgoritm.AI;
using GeneticAlgoritm.DNA;
using GeneticAlgoritm.Utilities;

namespace GeneticAlgoritm.Example {
    public class StringExample : Runnable {
        public void run() {
            GeneticAlgoritm<StringDNA> ga =
                new GeneticAlgoritm<StringDNA>(1000, OptimizationTarget.MINIMIZATION, 0.05, 0.8, 5);
            while (!ga.IsSolved) {
                ga.EvolveNextGeneration();
                Console.WriteLine($"{ga.GenerationsEvolved}: {ga.BestDNA}");
            }

            Console.WriteLine($"Genetic Algoritm has finished after {ga.GenerationsEvolved} generations");
            Console.WriteLine($"Solution: {ga.BestDNA}");
        }
    }
}