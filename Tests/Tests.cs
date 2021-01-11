using System;
using System.Linq;
using System.Threading;
using Xunit;
using static GeneticAlgoritm.Utilities.RandomNumGen;

namespace Tests {
    public class RandomNumGenTests {
        [Fact]
        public void TestRandomIntInRange() {
            int runs = 0;
            bool checkNegativ = false;
            bool checkPositive = false;
            bool checkMaximum = false;
            bool checkMinimum = false;

            while (runs < 1000 ^ (checkMaximum && checkMinimum && checkNegativ && checkPositive)) {
                int rand = getRandomIntInRange(-2, 2);
                checkMaximum = checkMaximum || rand == 2;
                checkMinimum = checkMinimum || rand == -2;
                checkPositive = checkPositive || rand > 0;
                checkNegativ = checkNegativ || rand < 0;
                runs++;
            }

            Assert.True(checkMaximum);
            Assert.True(checkMinimum);
            Assert.True(checkNegativ);
            Assert.True(checkPositive);
        }

        [Fact]
        public void TestRandomBool() {
            int trues = 0;
            int falses = 0;
            int runs = 0;

            while (runs < 1000) {
                if (getRandomTrue(0.9)) {
                    trues++;
                }
                else {
                    falses++;
                }

                runs++;
            }

            Assert.True(trues > 0);
            Assert.True(falses > 0);
            Assert.InRange(trues, 850, 950);


            trues = 0;
            falses = 0;
            runs = 0;

            while (runs < 1000) {
                if (getRandomTrue(0.1)) {
                    trues++;
                }
                else {
                    falses++;
                }

                runs++;
            }

            Assert.True(trues > 0);
            Assert.True(falses > 0);
            Assert.InRange(trues, 50, 150);
        }
    }
}