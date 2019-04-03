using LifeSupport.Config;
using LifeSupport.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSupport.GameObjects
{
    class AlienDog : Enemy
    {



        // Rectangle alienBox = new Rectangle(600, 600, 16, 16);

        public AlienDog(Player p, Vector2 position, Room room) : base(p, position, 32, 32, 0, Assets.Instance.alienDog, room, 300f)
        {
          
        }


        /*
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Assets.Instance.alienDog, Position, alienBox, Color.White, Rotation, new Vector2(0,0), 1f, SpriteEffects.None, 0);
            base.Draw(spriteBatch);
        }
        */
    }
}
