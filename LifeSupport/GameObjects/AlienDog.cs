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
  public  class AlienDog : Enemy
    {


        private int health;
        List<Point> path = new List<Point>();
        List<Point> nextPoint = new List<Point>();
        List<Vector2> positions = new List<Vector2>();


        public AlienDog(Player p, Vector2 position, Room room) : base(p, position, 32, 32, 0, Assets.Instance.alienDog, room, 100f)
        {
          
        }

        public void drawPath(SpriteBatch spriteBatch, GameTime gameTime)
        {


            Point startPoint = ToPoint(this.Position);
            Point calculatedStartPoint = new Point();
            calculatedStartPoint.X = (startPoint.Y / 30);
            calculatedStartPoint.Y = (startPoint.X / 30);

            Point endPoint = ToPoint(this.player.Position);
            Point calculatedEndPoint = new Point();

            // X is the row, Y is the column
            // X has a max of 35, Y has a max of 63
            calculatedEndPoint.X = (endPoint.Y / 30);
            calculatedEndPoint.Y = (endPoint.X / 30);
            Console.WriteLine(calculatedEndPoint);


            path = AStarSearch(calculatedStartPoint, calculatedEndPoint);
            //     positions.Clear();
            //      nextPoint.Clear();
            for (int i = 0; i < path.Count; i++)
            {
                if (i > 0)
                {

                    Point holdPoint = new Point();
                    holdPoint = path[i];
                    holdPoint.X = path[i].Y * 30;
                    holdPoint.Y = path[i].X * 30;
                    nextPoint.Add(holdPoint);
                    Point vectDistance = holdPoint - ToPoint(this.Position);
                    Vector2 vectDirection = ToVector2(vectDistance);
                    vectDirection.Normalize();

                    Vector2 newPosition = this.Position + (vectDirection * MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    this.Position = newPosition;

                    Vector2 test = ToVector2(holdPoint);
                    //spriteBatch.Draw(Assets.Instance.alienDog, ToVector2(currentRoom.gridPoints[path[i].X, path[i].Y]), new Rectangle(currentRoom.gridPoints[path[i].X, path[i].Y], new Point(32, 32)), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1);
                    spriteBatch.Draw(Assets.Instance.alienDog, test, new Rectangle(holdPoint, new Point(32, 32)), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1);
                }
                else { continue; }
            }
        }


    }
}
