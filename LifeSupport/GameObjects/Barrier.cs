using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LifeSupport.Config;
using Microsoft.Xna.Framework;

namespace LifeSupport.GameObjects {

    /*
    * New vision for this class:
    * Barriers should no longer be 32x32 squares, but dynamically sized.
    * This will make map creation easier, and rooms less computationally expensive
    * (less objects, less collision detection)
    * 
    * New constructor will ask for starting point (top left) and ending point (bottom right)
    */

    class Barrier : GameObject {

        private static readonly int rotation = 0 ;

        public static readonly int wallThickness = 32 ;

        public Barrier(int xPos, int yPos, Game game, int endX, int endY) : base(xPos, yPos, endX-xPos, endY-yPos, rotation, Assets.Instance.barrier, game) {
            //do nothing
        }

        public override void UpdatePosition(GameTime gameTime) {
            //also do nothing because it does not move
        }


    }
}
