using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LifeSupport.Augments;
using LifeSupport.Config;
using LifeSupport.Random;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;

namespace LifeSupport.GameObjects {
    class AugmentationStation : GameObject {

        private int UseRadius = 30 ;
        private Player player ;

        public delegate void OnUse() ;
        public static OnUse OnPlayerUse ;

        public AugmentationStation(Vector2 position, PenumbraComponent penumbra, Player player) : base(position, penumbra, 30, 30, 0, Assets.Instance.oxygenTank) {
            this.player = player ;
            
        }

        public override void UpdatePosition(GameTime gameTime) {

            //if the player is inside the use radius and hits the use button
            if (player.IsInside(Position.X-UseRadius, Position.Y-UseRadius, Position.X+UseRadius, Position.Y+UseRadius) 
                && Controller.Instance.IsKeyDown(Controller.Instance.Use)) {
                OnPlayerUse() ;
            }
            
        }

        public static Augmentation GenerateAugment(int money) {
            Augmentation aug = new Augmentation(0, 0, 0, 0, 0) ;

            //first generate random number for the number of stats on the augment
            int numStats = RandomGenerator.Instance.GetRandomIntRange(1, 5) ; //between 1 and 5 stats possible

            Console.WriteLine(numStats) ;

            //add the selected stats to stats list
            List<int> stats = new List<int>() ;

            for (int i = 0 ; i < numStats ; i++) {
                stats.Add(RandomGenerator.Instance.GetRandomIntRange(1, 5)) ; 
            }
            /*
             * 1 - damage
             * 2 - range
             * 3 - shot speed
             * 4 - rate of fire
             * 5 - move speed
             */

            //the pool we pull stats fromm
            int pool = money ;

            while (pool > 0) {
                //select a stat
                int selectedStat = RandomGenerator.Instance.GetRandomIntRange(0, stats.Count-1) ;
                //decide whether we will increment to it or decrement from it
                int roll = RandomGenerator.Instance.GetRandomIntRange(0, 3) ; //if it lands on 0 it is decrement otherwise inc
                Augmentation a = new Augmentation(0, 0, 0, 0, 0) ;
                switch (stats[selectedStat]) {
                    case 1:
                        a += new Augmentation(.1f, 0, 0, 0, 0) ;
                        pool-- ;
                        if (roll == 0) {
                            a = -1 * a ;
                            pool += 2 ;
                        }
                        break ;
                    case 2:
                        a += new Augmentation(0, .2f, 0, 0, 0) ;
                        pool-- ;
                        if (roll == 0) {
                            a = -1 * a ;
                            pool += 2 ;
                        }
                        break ;
                    case 3:
                        a += new Augmentation(0, 0, .2f, 0, 0) ;
                        pool-- ;
                        if (roll == 0) {
                            a = -1 * a ;
                            pool += 2 ;
                        }
                        break ;
                    case 4:
                        a += new Augmentation(0, 0, 0, .1f, 0) ;
                        pool-- ;
                        if (roll == 0) {
                            a = -1 * a ;
                        pool += 2 ;
                        }
                        break ;
                    case 5:
                        a += new Augmentation(0, 0, 0, 0, .05f) ;
                        pool-- ;
                        if (roll == 0) {
                            a = -1 * a ;
                        pool += 2 ;
                        }
                        break ;
                }
                aug += a ;
            }

            return aug ;

        }
    }
}
