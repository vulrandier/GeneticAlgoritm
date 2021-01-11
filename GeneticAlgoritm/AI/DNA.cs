using System;
using System.Data.Common;

namespace GeneticAlgoritm.AI {
    public abstract class DNA : IComparable, ICloneable {
        /// <summary>
        /// The fitness represents how well the DNA has operated
        /// whether the minimum or maximum is better depends on the
        /// OptimizationTarget
        /// <seealso cref="OptimizationTarget"/>
        /// </summary>
        public double Fitness {
            get => _fitness;
        }
        
        /// <summary>
        /// Checks if the problem is solved
        /// </summary>
        public abstract bool IsSolved {
            get;
        }
        
        protected double _fitness = 0;

        /// <summary>
        /// This function should implement all action the individual should do
        /// Finnaly it needs to save and return the fitness of the operation
        /// </summary>
        /// <returns>new fitness value</returns>
        public abstract double calculateFitness();


        /// <summary>
        /// Compares DNA by its fitness in ascending order
        /// </summary>
        /// <param name="obj">the other DNA to be compared</param>
        /// <returns>result of CompareTo()</returns>
        /// <exception cref="ArgumentException">obj is not a DNA</exception>
        public int CompareTo(object obj) {
            // CODE INSPIRED FROM: https://docs.microsoft.com/de-de/dotnet/api/system.icomparable?view=net-5.0
            if (obj == null) return 1;

            DNA other_DNA = obj as DNA;
            if (other_DNA != null)
                return this._fitness.CompareTo(other_DNA._fitness);
            else
                throw new ArgumentException($"Object is not a {nameof(DNA)}");
        }

        /// <summary>
        /// Recombine the genes of parent_A and parent_B
        /// </summary>
        /// <param name="parent_A">Parts of the new DNA should be taken from both parents</param>
        /// <param name="parent_B">Parts of the new DNA should be taken from both parents</param>
        public abstract void Crossover(in DNA parent_A,in DNA parent_B);

        /// <summary>
        /// Change random parts of the DNA
        /// </summary>
        /// <param name="mutation">Probability that a mutation happens</param>
        public abstract void Mutate(double mutation);

        public abstract object Clone();
    }
}