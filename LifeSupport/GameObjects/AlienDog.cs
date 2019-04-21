using LifeSupport.Config;
using LifeSupport.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoyT.AStar;

namespace LifeSupport.GameObjects
{
  public  class AlienDog : Enemy {

        private Player player;

        //animation stuff
        private int animFrame ; //the current frame of animation
        private float timer ; //the time between animation frames
        private float time ; //the current time since last animation frame

        public AlienDog(Player p, Vector2 position, Room room, 
            float speed, float health, float damage) : base(p, position, 30, 30, 0, Assets.Instance.alienDog, room, speed, health, damage, 0, 0, 0) {
            this.player = p;

            this.animFrame = 0 ;
            this.timer = .05f ;
            this.time = 0f ;

        }

        public override void UpdatePosition(GameTime gameTime) {

            //the OnHit requires a projectile so generate a dummy one
            if (this.IsInside(player))
                player.OnHit(new Projectile(Vector2.Zero, Vector2.Zero, Damage, 0, 0, false, CurrentRoom));

            this.time += (float)gameTime.ElapsedGameTime.TotalSeconds ;

            this.Rotation = (float)(Math.Atan(MoveDirection.Y/MoveDirection.X)) ;
            if (MoveDirection.X < 0f)
                this.Rotation -= (float)Math.PI ;

            if (this.time >= this.timer) {
                this.time = 0f ;
                animFrame = (animFrame+1)%(sprite.Width/(Width)) ;
            }

            base.UpdatePosition(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(sprite, Position, new Rectangle(animFrame*Width, 0, Width, Height), Color.White, Rotation, origin, 1f, SpriteEffects.None, 0);
        }

    }
}
