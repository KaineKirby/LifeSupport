using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LifeSupport.Config;
using LifeSupport.Levels;
using Microsoft.Xna.Framework;

namespace LifeSupport.GameObjects
{
    class Projectile : GameObject
    {
        public Point Source;
        public Vector2 Direction;
        public Room CurrentRoom;
        public float Damage;
        public float Velocity;
        public float Range;
        public float XPos, YPos;
        private float distanceTraveled;
        private bool isPlayer ;


        public Projectile(Point source, Vector2 direction, float damage, float velocity, float range, bool isPlayer, Room room) : base(new Rectangle(source.X, source.Y, 16, 16), 0, Assets.Instance.projectile)
        {
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
        }

        public override void UpdatePosition(GameTime gameTime)
        {
            XPos += (Direction * Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds).X;
            YPos += (Direction * Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds).Y;

            this.Rect.X = (int)XPos;
            this.Rect.Y = (int)YPos;

            distanceTraveled += (Direction * Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds).Length();

            if(distanceTraveled >= Range)
            {
                CurrentRoom.DestroyObject(this);
            }

            //check to see if the projectile hit a game object or actor
            for (int i = 0 ; i < CurrentRoom.Objects.Count ; i++) {
                if (CurrentRoom.Objects[i].Rect.Intersects(this.Rect)) {
                    //we need to ignore both other projectiles, and the player/enemy depending on what team the projectile is on
                    //either hit the player or the enemy
                    if (isPlayer) {
                        if (CurrentRoom.Objects[i] is Enemy)
                            ((Actor)CurrentRoom.Objects[i]).Hit(this) ;

                        if (!(CurrentRoom.Objects[i] is Projectile) && !(CurrentRoom.Objects[i] is Player))
                            CurrentRoom.DestroyObject(this) ;
                    }
                    else {
                        if (CurrentRoom.Objects[i] is Player)
                            ((Actor)CurrentRoom.Objects[i]).Hit(this) ;

                        if (!(CurrentRoom.Objects[i] is Projectile) && !(CurrentRoom.Objects[i] is Enemy))
                            CurrentRoom.DestroyObject(this) ;
                    }

                }
            }

        }

    }
}
