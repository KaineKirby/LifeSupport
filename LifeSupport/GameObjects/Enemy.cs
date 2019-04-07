using LifeSupport.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LifeSupport.GameObjects
{
    public abstract class Enemy : Actor {

        protected Player player;
        protected Room currentRoom;
        private int health;
        List<Point> path = new List<Point>();
        List<Point> nextPoint = new List<Point>();
        List<Vector2> positions = new List<Vector2>();

        float totalTime;

        float timer = 1;         //Initialize a 10 second timer
        const float TIMER = 1;

        public Enemy(Player p, Vector2 position, int width, int height, int rotation, Texture2D sprite,  Room room, float moveSpeed) : base(position, width, height, rotation, sprite, room, moveSpeed) {
            this.player = p;
            this.currentRoom = room;
        }





        public override void UpdatePosition(GameTime gameTime)
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
                    this.MoveDirection = vectDirection;
                    // Vector2 newPosition = this.Position + (vectDirection * MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    base.UpdatePosition(gameTime);
                }
                else { continue; }
                }   
            }



            public Vector2 ToVector2(Point point)
        {
            return new Vector2(point.X, point.Y);
        }

        public Point ToPoint(Vector2 vector)
        {
            return new Point((int)vector.X, (int)vector.Y);
        }




        public List<Point> AStarSearch(Point start, Point end)
        {

            List<Point> checkedTiles = new List<Point>();
            List<Point> newTiles = new List<Point>();
            newTiles.Add(start);

            Dictionary<Point, Point> optimalPath = new Dictionary<Point, Point>();
            Dictionary<Point, int> currentDistanceFromStart = new Dictionary<Point, int>();
            Dictionary<Point, float> predictedDistanceToEnd = new Dictionary<Point, float>();

            currentDistanceFromStart.Add(start, 0);
            predictedDistanceToEnd.Add(start, +Heuristic(start, end));

            while (newTiles.Count > 0)
            {

                Point current = (from i in newTiles orderby predictedDistanceToEnd[i] ascending select i).First();

                if (current.X == end.X && current.Y == end.Y)
                {
                    return BuildOptimalPath(optimalPath, end);
                }

                newTiles.Remove(current);
                checkedTiles.Add(current);

                foreach (Point neighbor in GetNeighborTiles(current))
                {
                    int tempCurrentDistance = currentDistanceFromStart[current] + 1;

                    if (checkedTiles.Contains(neighbor) && tempCurrentDistance >= currentDistanceFromStart[neighbor])
                    {
                        continue;
                    }

                    if (optimalPath.Keys.Contains(neighbor))
                    {
                        optimalPath[neighbor] = current;
                    }
                    else
                    {
                        optimalPath.Add(neighbor, current);
                    }

                    currentDistanceFromStart[neighbor] = tempCurrentDistance;
                    predictedDistanceToEnd[neighbor] = currentDistanceFromStart[neighbor] + Math.Abs(neighbor.X - end.X) + Math.Abs(neighbor.Y - end.Y);

                    if (!newTiles.Contains(neighbor))
                    {
                        newTiles.Add(neighbor);
                    }
                }
            }

            throw new Exception(string.Format("unable to find a path between {0},{1} and {2},{3}", start.X, start.Y, end.X, end.Y));
        }


        public float Heuristic(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }


        private List<Point> GetNeighborTiles(Point tile)
        {
            List<Point> tiles = new List<Point>();

            // Left
            if (currentRoom.occupiedTilesGrid[tile.X, tile.Y - 1] == 0)
            {
                tiles.Add(new Point(tile.X, tile.Y - 1));
            }

            // Down
            if (currentRoom.occupiedTilesGrid[tile.X + 1, tile.Y] == 0)
            {
                tiles.Add(new Point(tile.X + 1, tile.Y));
            } 

            // Right
            if (currentRoom.occupiedTilesGrid[tile.X, tile.Y + 1] == 0)
            {
                tiles.Add(new Point(tile.X, tile.Y + 1));
            }

            // Up
            if (currentRoom.occupiedTilesGrid[tile.X - 1, tile.Y] == 0)
            {
                tiles.Add(new Point(tile.X - 1, tile.Y));
            }
            
               
            // Down-Left
            if (currentRoom.occupiedTilesGrid[tile.X + 1, tile.Y - 1] == 0)
            {
                tiles.Add(new Point(tile.X + 1, tile.Y - 1));
            }

            // Up-Left
            if (currentRoom.occupiedTilesGrid[tile.X - 1, tile.Y - 1] == 0)
            {
                tiles.Add(new Point(tile.X - 1, tile.Y - 1));
            }

            // down-Right
            if (currentRoom.occupiedTilesGrid[tile.X + 1, tile.Y + 1] == 0)
            {
                tiles.Add(new Point(tile.X + 1, tile.Y + 1));
            }

            //Up-Right
            if (currentRoom.occupiedTilesGrid[tile.X - 1, tile.Y + 1] == 0)
            {
                tiles.Add(new Point(tile.X - 1, tile.Y + 1));
            }
            
            return tiles;
        }



        private List<Point> BuildOptimalPath(Dictionary<Point, Point> optimalPath, Point current)
        {
            if (!optimalPath.Keys.Contains(current))
            {
                return new List<Point> { current };
            }

            List<Point> completePath = BuildOptimalPath(optimalPath, optimalPath[current]);
            completePath.Add(current);
            return completePath;
        }





        /*
        public override void UpdatePosition(GameTime gameTime)
        {

            if (player.Position.Y > this.Position.Y) // If the player is below the enemy, the enenmy will move down
            {
                UpdateDirection(new Vector2(0, 1));
                base.UpdatePosition(gameTime);
            }
            if(player.Position.Y < this.Position.Y) // If the player is above the enemy, the enenmy will move up
            {
                UpdateDirection(new Vector2(0, -1));
                base.UpdatePosition(gameTime);
            }

            if (player.Position.X > this.Position.X) // If the player is to the right, the enemy will move right
            {
                UpdateDirection(new Vector2(1, 0));
                base.UpdatePosition(gameTime);
            }
            if (player.Position.X < this.Position.X) // If the player is to the left, the enemy will move left
            {
                UpdateDirection(new Vector2(-1, 0));
                base.UpdatePosition(gameTime);
            }
            
        }*/



        public Player setFollowPlayer(Player p)
        {
            this.player = p;
            return player;
        }


        public void OnHit(int damgage)
        {

        }

    }
}
