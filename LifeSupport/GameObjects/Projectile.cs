using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LifeSupport.Config;
using LifeSupport.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;

namespace LifeSupport.GameObjects {
    public class Projectile : GameObject {
        public Vector2 Source;
        public Vector2 Direction;
        public Room CurrentRoom;
        public float Damage;
        public float Velocity;
        public float Range;
        public float XPos, YPos;
        private float distanceTraveled;
        private bool isPlayer ;
        private Color color ;
        private PointLight light ;

        public Projectile(Vector2 source, Vector2 direction, float damage, float velocity, float range, bool isPlayer, Room room, PenumbraComponent penumbra) : base(source, penumbra, 8, 8, 0, Assets.Instance.projectile) {
            this.Source = source;
            this.Direction = direction;
            this.Damage = damage;
            this.Velocity = velocity;
            this.Range = range;
            this.distanceTraveled = 0;
            this.XPos = Source.X;
            this.YPos = Source.Y;
            this.CurrentRoom = room;
            this.HasCollision = false ;
            this.isPlayer = isPlayer ;

            //the color of the projectile should be different for the player vs the enemies
            if (isPlayer)
                this.color = Color.GreenYellow ;
            else
                this.color = Color.Red ;

            //the glow of the bullet in the air
            this.light = new PointLight {
                Position = this.Position,
                Scale = new Vector2(20f),
                Color = this.color
            } ;

            penumbra.Lights.Add(light) ;
        }

        public override void UpdatePosition(GameTime gameTime) {
            Position += (Direction * Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);

            distanceTraveled += (Direction * Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds).Length();

            if(distanceTraveled >= Range) {
                CurrentRoom.DestroyObject(this);
                penumbra.Lights.Remove(light) ;
            }

            //check to see if the projectile hit a game object or actor
            for (int i = 0 ; i < CurrentRoom.Objects.Count ; i++) {
                if (CurrentRoom.Objects[i].IsInside(this)) {
                    //we need to ignore both other projectiles, and the player/enemy depending on what team the projectile is on
                    //either hit the player or the enemy
                    if (isPlayer) {
                        if (i < CurrentRoom.Objects.Count && CurrentRoom.Objects[i] is Enemy) {
                            ((Actor)CurrentRoom.Objects[i]).OnHit(this) ;
                            penumbra.Lights.Remove(light) ;
                        }
             
                        if (i < CurrentRoom.Objects.Count && !(CurrentRoom.Objects[i] is Projectile) && !(CurrentRoom.Objects[i] is Player)) {
                            CurrentRoom.DestroyObject(this) ;
                            penumbra.Lights.Remove(light) ;
                        }
                    }
                    else {

                        if (i < CurrentRoom.Objects.Count && !(CurrentRoom.Objects[i] is Projectile) && !(CurrentRoom.Objects[i] is Enemy)) {
                            CurrentRoom.DestroyObject(this) ;
                            penumbra.Lights.Remove(light) ;
                        }
                    }

                }

                this.light.Position = this.Position ;
            }

            //when the player gets hit
            if (CurrentRoom.player.IsInside(this) && !isPlayer) {
                CurrentRoom.player.OnHit(this);
                CurrentRoom.DestroyObject(this) ;
                penumbra.Lights.Remove(light) ;
            }

        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(sprite, Position, spriteRectangle, color, Rotation, origin, 1f, SpriteEffects.None, 0);
        }

        //method to destroy the projectile
        public void Destroy() {
            CurrentRoom.DestroyObject(this) ;
            penumbra.Lights.Remove(light) ;
        }

    }
}
