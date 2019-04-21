using LifeSupport.Config;
using LifeSupport.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSupport.GameObjects
{
    class AlienInfantry : Enemy {

        private Player player;

        //leg animation
        private Texture2D alienLegs ;
        private int animFrame ; //the current frame of animation
        private float timer ; //the time between animation frames
        private float time ; //the current time since last animation frame
        private float legRotation ;
        private Vector2 legOrigin ;

        public AlienInfantry(Player p, Vector2 position, PenumbraComponent penumbra, Room room,
            float speed, float health, float damage, float range, float shotSpeed, float rateOfFire) : 
            base(p, position, penumbra, 30, 30, 0, Assets.Instance.alienInfantry, room, speed, health, damage, range, shotSpeed, rateOfFire) {

            this.player = p;

            this.animFrame = 0 ;
            this.timer = .05f ;
            this.time = 0;
            this.alienLegs = Assets.Instance.alienInfantryLegs ;
            this.legOrigin = new Vector2(16, 16) ;
            this.legRotation = 0 ;

        }


        public override void UpdatePosition(GameTime gameTime) {
            base.UpdatePosition(gameTime);

            if (this.HasLineOfSight()) {
                Vector2 dir = player.Position - this.Position;
                dir.Normalize();
                this.Rotation = (float)(Math.Atan(dir.Y/dir.X)) ;
                if (dir.X < 0f)
                    this.Rotation -= (float)Math.PI ;
                Shoot(dir, Assets.Instance.alienShot);
            }
            else {
                this.Rotation = (float)(Math.Atan(MoveDirection.Y/MoveDirection.X)) ;
                if (MoveDirection.X < 0f)
                    this.Rotation -= (float)Math.PI ;
            }


            //the OnHit requires a projectile so generate a dummy one
            if (this.IsInside(player))
                player.OnHit(new Projectile(Vector2.Zero, Vector2.Zero, Damage, 0, 0, false, CurrentRoom, penumbra));

            //for animation
            this.time += (float)gameTime.ElapsedGameTime.TotalSeconds ;
            this.legRotation = (float)(Math.Atan(MoveDirection.Y/MoveDirection.X)) ;
                
            //timer between frames
            if (time >= timer) {
                time = 0 ;
                animFrame = (animFrame+1)%(alienLegs.Width/(32)) ;
            }
        }

        protected override void InfluenceDirection(Vector2 direction, GameTime gameTime) {
            this.MoveDirection = ((MoveDirection * (.6f) / (float)gameTime.ElapsedGameTime.TotalSeconds) + direction)/30 ;
            if (!MoveDirection.Equals(Vector2.Zero))
                MoveDirection.Normalize() ;
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(alienLegs, Position, new Rectangle(animFrame*32, 0, 32, 32), Color.White, legRotation, legOrigin, 1f, SpriteEffects.None, .1f);
            base.Draw(spriteBatch) ;
        }
    }
}
