using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LifeSupport.Config;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        public static readonly int WallThickness = 30 ;

        private Rectangle rect ;

        public Barrier(Rectangle rect) : base(Vector2.Zero, rect.Width, rect.Height, rotation, Assets.Instance.barrier) {
            this.rect = rect ;
            this.Position = rect.Center.ToVector2() ;
        }

        public override void UpdatePosition(GameTime gameTime) {
            //also do nothing because it does not move
        }


    }
}
