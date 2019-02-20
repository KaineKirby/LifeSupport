using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LifeSupport.Config ;
using Microsoft.Xna.Framework;

namespace LifeSupport.GameObject {
    abstract class Actor : GameObject {

        public float MoveSpeed ;
        private Vector2 MoveDirection ; //enemies may need to see the player direction and this has to change

        public Actor(int xPos, int yPos, int width, int height, int rotation, String spritePath, float moveSpeed) : base(xPos, yPos, width, height, rotation, spritePath) {
            this.MoveSpeed = moveSpeed ;
        }

        protected void UpdateDirection(Vector2 vector) {
            this.MoveDirection = vector ;
            this.MoveDirection.Normalize() ;
        }

        public override void UpdatePosition(GameTime gameTime) {
            this.XPos += (MoveDirection * MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds).X ;
            this.YPos += (MoveDirection * MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds).Y ;

        }

    }
}
