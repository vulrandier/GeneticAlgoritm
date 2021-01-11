using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using GeneticAlgoritm.AI;
using static GeneticAlgoritm.Utilities.RandomNumGen;

namespace GeneticAlgoritm.DNA {
    public class StringDNA : AI.DNA {
        private static readonly string _target_string = "Programming with Rider is awesome";
        private string _dna_string = "";
        private static readonly string _alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ ";

        public StringDNA() {
            StringBuilder builder = new StringBuilder(_target_string.Length);
            _fitness = double.MaxValue;
            for (int i = 0; i < _target_string.Length; i++) {
                builder.Append(_alphabet[getRandomIntInRange(0, _alphabet.Length - 1)]);
            }

            _dna_string = builder.ToString();
        }

        public StringDNA(in StringDNA copyFrom) {
            _dna_string = copyFrom._dna_string;
            _fitness = copyFrom._fitness;
        }

        public override bool IsSolved {
            get { return _fitness <1; }
        }

        public override double calculateFitness() {
#if DEBUG
            // if code works in debug this check do not need to be executed in release...
            if (_target_string.Length != _dna_string.Length) {
                throw new ApplicationException(
                    $"length of {nameof(_target_string)} and {nameof(_dna_string)} do not match. Check if {nameof(_dna_string)} gets properly initialized.");
            }
#endif

            double local_fitness = 0.0;

            for (int iChar = 0; iChar < _target_string.Length; iChar++) {
                if (_dna_string[iChar] != _target_string[iChar]) {
                    local_fitness++;
                }
            }

            _fitness = local_fitness;
            return local_fitness;
        }

        public override void Crossover(in AI.DNA parent_A, in AI.DNA parent_B) {
            var dna_1 = parent_A as StringDNA;
            var dna_2 = parent_B as StringDNA;

            if (getRandomTrue()) {
                CrossOverAtPoint(dna_1, dna_2);
            }
            else {
                CrossOver_50_50(dna_1, dna_2);
            }
        }

        private void CrossOverAtPoint(in StringDNA parent_A, in StringDNA parent_B) {
            int point = (int) (parent_A._dna_string.Length * getRandomDouble_0_to_1());

            _dna_string = parent_A._dna_string.Substring(0, point) +
                          parent_B._dna_string.Substring(point);


        }


        private void CrossOver_50_50(in StringDNA parent_A, in StringDNA parent_B) {

            StringBuilder builder = new StringBuilder(_dna_string);
            for (int iChar = 0; iChar < _dna_string.Length; iChar++) {
                if (getRandomTrue()) {
                    builder[iChar] = parent_A._dna_string[iChar];
                }
                else {
                    builder[iChar] = parent_B._dna_string[iChar];
                }
            }

        }

        public override void Mutate(double mutation) {
            StringBuilder builder = new StringBuilder(_dna_string);
            for (int iChar = 0; iChar < _dna_string.Length; iChar++) {
                if (getRandomTrue(mutation)) {
                    builder[iChar] = _alphabet[getRandomIntInRange(0, _alphabet.Length - 1)];
                }
            }

            _dna_string = builder.ToString();
        }

        public override object Clone() {
            return new StringDNA(this);
        }

        public override string ToString() {
            return _dna_string;
        }

        public override bool Equals(object obj) {
            // CODE INSPIRED FROM: https://docs.microsoft.com/de-de/dotnet/api/system.icomparable?view=net-5.0
            if (obj == null) return false;

            StringDNA other_DNA = obj as StringDNA;
            if (other_DNA != null)
                return this._dna_string == other_DNA._dna_string;
            else
                throw new ArgumentException($"Object is not a {nameof(StringDNA)}");
            return _dna_string.Equals(obj);
        }

        public override int GetHashCode() {
            return _dna_string.GetHashCode();
        }
    }
}