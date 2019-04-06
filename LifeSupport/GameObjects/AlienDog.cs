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

        List<Point> path = new List<Point>();
        Point dogPoint = new Point();
        float totalTime;


        public AlienDog(Player p, Vector2 position, Room room) : base(p, position, 32, 32, 0, Assets.Instance.alienDog, room, 10f)
        {
          
        }


        public override void UpdatePosition(GameTime gameTime)
        {

            Point startPoint = ToPoint(this.Position);
            Point calculatedStartPoint = new Point();
            calculatedStartPoint.Y = (startPoint.X / 30);
            calculatedStartPoint.X = (startPoint.Y / 30);

            Point endPoint = ToPoint(this.player.Position);
            Point calculatedEndPoint = new Point();
            calculatedEndPoint.Y = (endPoint.X / 30);
            calculatedEndPoint.X = (endPoint.Y / 30);


            path = AStarSearch(calculatedStartPoint, calculatedEndPoint);
            Point currentEnemyPosition = calculatedStartPoint;

            for(int i = 0; i < path.Count; i++)
            {
                Console.WriteLine(path[i] + " ");
                if (path[i].Y > currentEnemyPosition.Y)
                {
                    UpdateDirection(new Vector2(0, 1));
                    base.UpdatePosition(gameTime);
                }
                if (path[i].Y < currentEnemyPosition.Y) // If the player is above the enemy, the enenmy will move up
                {
                    UpdateDirection(new Vector2(0, -1));
                    base.UpdatePosition(gameTime);
                }

                if (path[i].X > currentEnemyPosition.X) // If the player is to the right, the enemy will move right
                {
                    UpdateDirection(new Vector2(1, 0));
                    base.UpdatePosition(gameTime);
                }
                if (path[i].X < currentEnemyPosition.X) // If the player is to the left, the enemy will move left
                {
                    UpdateDirection(new Vector2(-1, 0));
                    base.UpdatePosition(gameTime);
                }
            }

        }
            //   List<Point> pathTraveled = new List<Point>();
            //    pathTraveled.Add(calculatedStartPoint);

            /*
            for (int index = 0; index < path.Count; index++)
                 {
                     Console.WriteLine(path[index] + " ");
                    if (path[index].Y > this.Position.Y) // If the player is below the enemy, the enenmy will move down
                    {
                        UpdateDirection(new Vector2(0, 1));
                        base.UpdatePosition(gameTime);
                    }
                    if (path[index].Y < this.Position.Y) // If the player is above the enemy, the enenmy will move up
                    {
                        UpdateDirection(new Vector2(0, -1));
                        base.UpdatePosition(gameTime);
                    }

                    if (path[index].X > this.Position.X) // If the player is to the right, the enemy will move right
                    {
                        UpdateDirection(new Vector2(1, 0));
                        base.UpdatePosition(gameTime);
                    }
                    if (path[index].X < this.Position.X) // If the player is to the left, the enemy will move left
                    {
                        UpdateDirection(new Vector2(-1, 0));
                        base.UpdatePosition(gameTime);
                    }
                  }   
                */

        


        public Vector2 ToVector2(Point point)
        {
                return new Vector2(point.X, point.Y);
        }

        public Point ToPoint(Vector2 vector)
        {
            return new Point((int)vector.X, (int)vector.Y);
        }


        public void drawPath(SpriteBatch spriteBatch, GameTime gameTime)
        {

            //      totalTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            //      if (totalTime > 1)
            //      {
            //         path = Pathfind(startPoint, endPoint);

            Point startPoint = ToPoint(this.Position);
            Point calculatedStartPoint = new Point();
            calculatedStartPoint.Y = (startPoint.X / 30); //+ (startPoint.X % 30);
            calculatedStartPoint.X = (startPoint.Y / 30); //+ (startPoint.Y % 30);

            Point endPoint = ToPoint(this.player.Position);
            Point calculatedEndPoint = new Point();
            calculatedEndPoint.Y = (endPoint.X / 30); //+ (endPoint.X % 30);
            calculatedEndPoint.X = (endPoint.Y / 30); //+ (endPoint.Y % 30);


            path = AStarSearch(calculatedStartPoint, calculatedEndPoint);
           for (int index = 0; index < path.Count; index++)
            {
                spriteBatch.Draw(Assets.Instance.alienDog, ToVector2(currentRoom.gridPoints[path[index].X, path[index].Y]), new Rectangle(currentRoom.gridPoints[path[index].X, path[index].Y], new Point(32, 32)), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1);
              //    Console.WriteLine(path[index] + " ");
             }
                     
            //         totalTime = 0;
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
            predictedDistanceToEnd.Add(start, + Heuristic(start, end));
  
            while (newTiles.Count > 0)
            {
                
                Point current = (from i in newTiles orderby predictedDistanceToEnd[i] ascending select i).First();

                if (current.X == end.X && current.Y == end.Y){
                    return BuildOptimalPath(optimalPath, end);
                }

                newTiles.Remove(current);
                checkedTiles.Add(current);

                foreach (Point neighbor in GetNeighborTiles(current))
                {
                    int tempCurrentDistance = currentDistanceFromStart[current] + 1;

                    if (checkedTiles.Contains(neighbor) && tempCurrentDistance >= currentDistanceFromStart[neighbor]){
                        continue;
                    }

                    if (optimalPath.Keys.Contains(neighbor)){
                        optimalPath[neighbor] = current;
                    }
                    else{
                        optimalPath.Add(neighbor, current);
                    }

                    currentDistanceFromStart[neighbor] = tempCurrentDistance;
                    predictedDistanceToEnd[neighbor] = currentDistanceFromStart[neighbor]+ Math.Abs(neighbor.X - end.X) + Math.Abs(neighbor.Y - end.Y);

                    if (!newTiles.Contains(neighbor)) {
                        newTiles.Add(neighbor);
                     } 
                }
            }

            throw new Exception(string.Format("unable to find a path between {0},{1} and {2},{3}", start.X, start.Y, end.X, end.Y ));
        }


        public float Heuristic(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

     
        private List<Point> GetNeighborTiles(Point tile)
        {
            List<Point> tiles = new List<Point>();

            // up
            if (currentRoom.occupiedTilesGrid[tile.X, tile.Y - 1] ==  0){
                tiles.Add(new Point(tile.X, tile.Y - 1));
            }

            // right
            if (currentRoom.occupiedTilesGrid[tile.X + 1, tile.Y] == 0){
                tiles.Add(new Point(tile.X + 1, tile.Y));
            }

            // down
            if (currentRoom.occupiedTilesGrid[tile.X, tile.Y + 1] == 0){
                tiles.Add(new Point(tile.X, tile.Y + 1));
            }

            // left
            if (currentRoom.occupiedTilesGrid[tile.X - 1, tile.Y] == 0) {
                tiles.Add(new Point(tile.X - 1, tile.Y));
            }

            // up-right
            if (currentRoom.occupiedTilesGrid[tile.X + 1, tile.Y - 1] == 0) {
                tiles.Add(new Point(tile.X + 1, tile.Y - 1));
            }

            // up-left
            if (currentRoom.occupiedTilesGrid[tile.X - 1, tile.Y - 1] == 0){
                tiles.Add(new Point(tile.X - 1, tile.Y - 1));
            }

            // down-right
            if (currentRoom.occupiedTilesGrid[tile.X + 1, tile.Y + 1] == 0){
                tiles.Add(new Point(tile.X + 1, tile.Y + 1));
            }

            //down-left
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

    }
}
