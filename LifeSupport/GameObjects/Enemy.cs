using LifeSupport.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSupport.GameObjects {
    abstract class Enemy : Actor {

        public Enemy(Rectangle rect, int rotation, Texture2D sprite,  Room room, float moveSpeed) : base(rect, rotation, sprite, room, moveSpeed) {

        }

    }
}
