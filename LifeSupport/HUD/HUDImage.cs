using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSupport.HUD {

    class HUDImage {

        private Texture2D image ;
        private Vector2 position ;

        public Rectangle rect ;

        public HUDImage(Texture2D image, Vector2 position) {
            this.image = image ;
            this.position = position ;
            this.rect = new Rectangle(0, 0, image.Width, image.Height) ;
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(image, position, rect, Color.White, 0, new Vector2(0,0) , 1f, SpriteEffects.None, 0);
        }

    }
}
