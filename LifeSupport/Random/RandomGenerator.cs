using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
* RandomGenerator Class (Singleton)
* 
* This is the class that we use for RandomNumber generation across the whole game.
* 
* In addition to having some nicer built in methods with this, we can eventually introduce new RNG algorithms that have the ability to seed
* meaning that we can introduce the concept of game seeds.
* 
* It must be an object so we can control the seed, otherwise we could just make it a static class. In its current implementation it could be,
* but just future-proofing.
* 
* NOTE: TODO: This is a very basic class for now, just calling upon the java built in random number generation, we can change this later
*/

namespace LifeSupport.Random {

    class RandomGenerator {

        System.Random random ;
        
        private static RandomGenerator instance ;
        public static RandomGenerator Instance {
            get {
                if (instance != null)
                    return instance ;
                else 
                    return new RandomGenerator() ;
            }
            private set {
                instance = value ;
            }
        }

        private RandomGenerator() {
            instance = this ;
            random = new System.Random() ;
        }

        public int GetRandomIntRange(int min, int max) {
            return random.Next(min, max+1) ;
        }

    }
}
