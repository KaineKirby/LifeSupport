using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSupport.Random {

    class RandomGenerator {
        
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
        }

        public int GetRandomIntRange(int min, int max) {
            System.Random rand = new System.Random() ;
            return rand.Next(min, max+1) ;
        }

    }
}
