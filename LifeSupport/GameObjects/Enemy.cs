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

        public Enemy(Vector2 position, int width, int height, int rotation, Texture2D sprite,  Room room, float moveSpeed) : base(position, width, height, rotation, sprite, room, moveSpeed) {

        }

    }
}
