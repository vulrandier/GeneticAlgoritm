using System;
using System.Collections.Generic;
using System.Linq;
using static GeneticAlgoritm.Utilities.RandomNumGen;
using static GeneticAlgoritm.Utilities.Extensions;

namespace GeneticAlgoritm.AI {
    /// <summary>
    /// Creates a new instance of a evolutionary algoritm based on survival of the fittest
    /// Requires a DNA which implements a fitness function    /// </summary>
    /// <typeparam name="T">A DNA implementation</typeparam>
    public class GeneticAlgoritm<T> where T : DNA, new() {

        private List<T> _population = new List<T>();
            
        public ulong GenerationsEvolved => _generationsEvolved;
        public ulong GenerationImprovements => _generationImprovements;
        public ulong FitnessEvaluations => _fitnessEvaluations;
        public OptimizationTarget Mode => _mode;

        public bool IsSolved {
            get {
                if (_bestDNA == null) return false;
                return _bestDNA.IsSolved;
            }
        }

        public DNA BestDNA => _bestDNA;

        private ulong _generationsEvolved = 0;
        private ulong _generationImprovements = 0;
        private ulong _fitnessEvaluations = 0;
        private OptimizationTarget _mode;

        private int _reproducable_count = 0;
        private double _reproducable_quotient = 0;
        private double _best_can_reproduce = 0;
        private double _mutation = 0;
        private DNA _bestDNA;
        private double _best_fitness;


        /// <summary>
        /// Creates a new instance of a evolutionary algoritm based on survival of the fittest.
        /// Requires a DNA which implements a fitness function
        /// </summary>
        /// <param name="population_size">Count of individuals per generation</param>
        /// <param name="mode">Is maximization or minimization the goal</param>
        /// <param name="mutation">Probability that mutations happen. More than 0.05 seens to be a bad choice</param>
        /// <param name="best_can_reproduce">propability that parents can recombine their genes</param>
        /// <param name="elitism_modifier">Multiplied with log_2(population_size), determines how many individuals may reproduce. Actually meant to be less than 1. But beeing greater than 1 could effect speed positivly</param>
        /// <exception cref="ArgumentOutOfRangeException">There must be at least 10 individuals in the population_size, less makes no sense and will also cause errors</exception>
        /// <exception cref="ArgumentException">There are less than 3 / not enough parents, because _reproducable_count is smaller than 3 which is a minimum defined by the developer</exception>
        public GeneticAlgoritm(int population_size, OptimizationTarget mode, double mutation,
            double best_can_reproduce = 0.9,
            double elitism_modifier = 1) {
            if (population_size < 10)
                throw new ArgumentOutOfRangeException($"{nameof(population_size)} must be greater than 9");

            for (int i = 0; i < population_size; i++) {
                _population.Add(new T());
            }
            
            _mode = mode;
            _reproducable_count = (int) ((elitism_modifier * Math.Log(population_size, 2)) % population_size);
            _reproducable_quotient = 1 / _reproducable_count; // used for selection, cannot divide by zero so need to multiply with quotient
            _best_can_reproduce = best_can_reproduce;
            _mutation = mutation;

            if (mode == OptimizationTarget.MAXIMIZATION) {
                _best_fitness = double.MinValue;
            }
            else {
                _best_fitness = double.MaxValue;
            }

            if (_reproducable_count < 3)
                throw new ArgumentException($"{nameof(_reproducable_count)} may not be smaller than 3");
        }

        /// <summary>
        /// Generates the next generation
        /// This involves selection and reproduction including mutation & crossover
        /// </summary>
        public void EvolveNextGeneration() {
            foreach (var individual in _population) {
                individual.calculateFitness();
                _fitnessEvaluations++;
            }

            _population.Sort();
            if (_mode == OptimizationTarget.MAXIMIZATION)
                _population.Reverse();

            // Check for improvements
            if ((_mode == OptimizationTarget.MAXIMIZATION && _population[0].Fitness > _best_fitness) ^
                (_mode == OptimizationTarget.MINIMIZATION && _population[0].Fitness < _best_fitness)) {
                _best_fitness = _population[0].Fitness;
                _bestDNA = _population[0].Clone() as DNA;
                _generationImprovements++;
            }

            
            // Selection seems to be a very imporant step
            // Having a bad selection algoritm can prevent the algoritm from ever succeeding...
            // 1st try: choosing random from best dna
            // 2nd try: make choosing the first one most probably and then decrease p
            //          the effect of this is really crazy...
            //          while guessing a string, we could easily improve the fitnes from 22 to 5 in 
            //          17 generations, while before the best performance stopped at 22
            //          But now we stop at 5
            //          It seems like the muational effect is still to strong
            //          Yes, this was a question of the config, how about creating a ga that trains ga settings lol
            List<DNA> parents = new List<DNA>();
            for (int iIndividual = 0; iIndividual < _reproducable_count; iIndividual++) {
                double prob_selection = 1 - iIndividual*_reproducable_quotient; // probability decreases on a linear level
                 prob_selection = Math.Pow(2, prob_selection); // even more boost
                if (getRandomTrue(prob_selection)) {
                    parents.Add(_population[iIndividual].Clone() as DNA);
                }
            }
            
            // a new population pool should be created
            for (int iIndividual = 0; iIndividual < _population.Count; iIndividual++) { 
                // Needed to avoid local maxima... some variety is good, too much is bad...
                if (getRandomTrue(_best_can_reproduce)) {
                    DNA parent1 = parents.selectRandomItem<DNA>();
                    DNA parent2 = parents.selectRandomItem<DNA>();
                    _population[iIndividual].Crossover(parent1, parent2);
                }

                _population[iIndividual].Mutate(_mutation);
            }

            _generationsEvolved++;
        }
    }
}