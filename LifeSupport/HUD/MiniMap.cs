using LifeSupport.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSupport.HUD {

    class MiniMap {

        private Rectangle bounds ;
        private Vector2 position ;
        private Level level ;

        public MiniMap(Vector2 position, Rectangle boundaries, Level level) {
            this.position = position ;
            this.bounds = boundaries ;
            this.level = level ;
        }

        public void Update() {

        }

        public void Draw(SpriteBatch spriteBatch) {

        }

    }
}
