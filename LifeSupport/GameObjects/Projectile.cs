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


        public Projectile(Point source, Vector2 direction, float damage, float velocity, float range, Room room) : base(new Rectangle(source.X, source.Y, 8, 8), 0, Assets.Instance.projectile)
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


        }

    }
}
