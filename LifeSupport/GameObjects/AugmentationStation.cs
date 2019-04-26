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

        //the augment the station is holding
        public Augmentation Augment ;

        public delegate void OnUse(AugmentationStation station) ;
        public static OnUse OnPlayerUse ;

        public AugmentationStation(Vector2 position, PenumbraComponent penumbra, Player player) : base(position, penumbra, 30, 30, 0, Assets.Instance.augmentationStation) {
            this.player = player ;
            this.Augment = null ;
            
        }

        public override void UpdatePosition(GameTime gameTime) {

            //if the player is inside the use radius and hits the use button
            if (player.IsInside(Position.X-UseRadius, Position.Y-UseRadius, Position.X+UseRadius, Position.Y+UseRadius) 
                && Controller.Instance.IsKeyDown(Controller.Instance.Use)) {
                OnPlayerUse(this) ;
            }
            
        }

        public static Augmentation GenerateAugment(int money) {

            //first generate random number for the number of stats on the augment
            int numStats ;
            if (money >= 10)
                numStats = RandomGenerator.Instance.GetRandomIntRange(1, 6) ; //between 1 and 6 stats possible when money >10
            else 
                numStats = RandomGenerator.Instance.GetRandomIntRange(1, 5) ;

            //add the selected stats to stats list
            List<int> stats = new List<int>() ;
            //whether the stat is positive or negative
            //false for negative true for positive
            List<bool> posNeg = new List<bool>() ;

            for (int i = 0 ; i < numStats ; i++) {
                if (money >= 10)
                    stats.Add(RandomGenerator.Instance.GetRandomIntRange(1, 6)) ; 
                else
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

            float d = 0 ;
            float r = 0 ;
            float ss = 0 ;
            //rate of fire is handled in shots per second
            float rof = 0 ;
            float ms = 0 ;
            bool spr = false ;

            while (pool > 0 && stats.Count > 0) {
                //select a stat
                int selectedStat = RandomGenerator.Instance.GetRandomIntRange(0, stats.Count-1) ;

                int roll = RandomGenerator.Instance.GetRandomIntRange(1, pool) ;

                switch (stats[selectedStat]) {
                    case 1:
                        d += .1f*roll ;
                        pool -= roll ;
                        break ;
                    case 2:
                        r += .2f*roll ;
                        pool -= roll ;
                        break ;
                    case 3:
                        ss += .2f*roll ;
                        pool -= roll ;
                        break ;
                    case 4:
                        rof += 1*roll ;
                        //the fire rate is a disproprtionally good upgrade
                        pool -= roll;
                        break ;
                    case 5:
                        ms += .05f*roll ;
                        pool -= roll ;
                        break ;
                    case 6:
                        if (pool >= 10) {
                            spr = true ;
                            pool -= 10 ;
                            stats.Remove(6) ;
                        }
                        break ;
                }
            }

            return new Augmentation(d, r, ss, rof, ms, spr) ;

        }
    }
}
