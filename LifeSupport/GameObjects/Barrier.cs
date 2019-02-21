using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace LifeSupport.GameObjects {

    class Barrier : GameObject {

        //these are configurable but should be the same for all barriers
        private static readonly int width = 32 ;
        private static readonly int height = 32 ;
        private static readonly int rotation = 0 ;

        public Barrier(int xPos, int yPos, Game game) : base(xPos, yPos, width, height, rotation, "img/objects/barrier", game) {
            //do nothing
        }

        public override void UpdatePosition(GameTime gameTime, GameObject[] objects) {
            //also do nothing because it does not move
        }


    }
}
