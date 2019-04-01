using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LifeSupport.Config;
using LifeSupport.GameObjects ;
using LifeSupport.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LifeSupport.Levels {

    /*
     * This class needs a lot of work currently
     * It needs to have a defined size so it can tell if the player is in it
     * It also needs to build the Barriers in the GenerateRoom method better (and dynamically with the size)
     * It also needs to check to see if the player is in the room or not
     *
     * And then stuff for spawning enemies later but we're nowhere near that
     */

    class Room {

        //an array of game objects
        public List<GameObject> Objects ;

        //the player (to know whether player is in room or not)
        private Player player ;

        //whether the room has been defeated or not
        private bool isBeaten ;

        //whether the room has the player in it or not
        private bool isActive ;

        //the width and height of the room in pixels
        public int Width ;
        public int Height ;

        //the starting point for room construction (top left)
        /*StartX and StartY both are currently set at 400 */
        public int StartX ;
        public int StartY ;

        /*Room Width and Height without walls 
          8x8, or 236 width with 131 height*/
        public int ROOMWIDTH = 1856;
        public int ROOMHEIGHT = 1016;


      public int SQUAREWIDTH = 232;
        public int SQUAREHEIGHT = 127;  


        public Room(Player player, int startX, int startY) {

            this.player = player ;
            this.isBeaten = false ;
            this.isActive = true ;

            this.StartX = startX ;
            this.StartY = startY ;

            //width and height of room in pixels
            this.Width = 1920 ;
            this.Height = 1080 ;

            this.Objects = new List<GameObject>() ;

            GenerateRoom() ;

        }

        //update all the objects in the room
        public void UpdateObjects(GameTime gameTime) {

            if (isActive) {
                for (int i = 0; i < Objects.Count; i++) {
                    Objects[i].UpdatePosition(gameTime) ;
                }
            }

        }

        public void RenderObjects(SpriteBatch spriteBatch) {
            //render the tile floor
            spriteBatch.Draw(Assets.Instance.floorTile, new Vector2(StartX, StartY), new Rectangle(0, 0, Width, Height), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1) ;

            if (isActive) {
                for (int i = 0; i < Objects.Count; i++)
                {
                    Objects[i].Draw(spriteBatch) ;
                }
            }
        }

        public void DestroyObject(GameObject obj)
        {
            Objects.Remove(obj);
        }

        public void AddObject(GameObject obj) {
            Objects.Add(obj) ;
        }




        //fills the room with game objects from our prefab set
        //TODO for now this just makes a box
        private void GenerateRoom() {

            //top no door
            Objects.Add(new Barrier(new Rectangle(StartX, StartY, Width, Barrier.WallThickness))) ;
            //bottom no door
            Objects.Add(new Barrier(new Rectangle(StartX, StartY+Height-Barrier.WallThickness, Width, Barrier.WallThickness))) ;
            //left no door
            //Objects.Add(new Barrier(new Rectangle(StartX, StartY+Barrier.WallThickness, Barrier.WallThickness, Height-2*Barrier.WallThickness))) ;
            //right no door
            //Objects.Add(new Barrier(new Rectangle(StartX+Width-Barrier.WallThickness, StartY+Barrier.WallThickness, Barrier.WallThickness, Height-2*Barrier.WallThickness))) ;

            //left with door
            Objects.Add(new Barrier(new Rectangle(StartX, StartY+Barrier.WallThickness, Barrier.WallThickness, (Height/2)-2*Barrier.WallThickness))) ;
            Objects.Add(new Door(new Vector2(StartX, StartY+(Height/2)-Barrier.WallThickness))) ;
            Objects.Add(new Barrier(new Rectangle(StartX, StartY+(Height/2)+Barrier.WallThickness, Barrier.WallThickness, (Height/2) - Barrier.WallThickness*2))) ;

            //right with door
            Objects.Add(new Barrier(new Rectangle(StartX+Width-Barrier.WallThickness, StartY+Barrier.WallThickness, Barrier.WallThickness, (Height/2)-2*Barrier.WallThickness))) ;
            Objects.Add(new Door(new Vector2(StartX+Width-Barrier.WallThickness, StartY+(Height/2)-Barrier.WallThickness))) ;
            Objects.Add(new Barrier(new Rectangle(StartX+Width-Barrier.WallThickness, StartY+(Height/2)+Barrier.WallThickness, Barrier.WallThickness, (Height/2) - Barrier.WallThickness*2))) ;

            Point[,] grid = new Point[8, 8];
            Point normalBarrierSize = new Point(32,32);
            Point doubleBarrierSize = new Point(64, 64);
            generateGrid(grid);

            Objects.Add(new Barrier(new Rectangle(grid[0, 0], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[0, 1], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[0, 2], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[0, 3], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[0, 4], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[0, 5], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[0, 6], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[0, 7], normalBarrierSize)));

            Objects.Add(new Barrier(new Rectangle(grid[1, 0], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[1, 1], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[1, 2], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[1, 3], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[1, 4], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[1, 5], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[1, 6], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[1, 7], normalBarrierSize)));

            Objects.Add(new Barrier(new Rectangle(grid[2, 0], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[2, 1], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[2, 2], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[2, 3], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[2, 4], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[2, 5], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[2, 6], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[2, 7], normalBarrierSize)));

            Objects.Add(new Barrier(new Rectangle(grid[3, 0], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[3, 1], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[3, 2], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[3, 3], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[3, 4], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[3, 5], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[3, 6], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[3, 7], normalBarrierSize)));

            Objects.Add(new Barrier(new Rectangle(grid[4, 0], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[4, 1], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[4, 2], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[4, 3], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[4, 4], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[4, 5], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[4, 6], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[4, 7], normalBarrierSize)));

            Objects.Add(new Barrier(new Rectangle(grid[5, 0], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[5, 1], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[5, 2], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[5, 3], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[5, 4], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[5, 5], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[5, 6], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[5, 7], normalBarrierSize)));

            Objects.Add(new Barrier(new Rectangle(grid[6, 0], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[6, 1], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[6, 2], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[6, 3], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[6, 4], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[6, 5], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[6, 6], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[6, 7], normalBarrierSize)));

            Objects.Add(new Barrier(new Rectangle(grid[7, 0], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[7, 1], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[7, 2], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[7, 3], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[7, 4], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[7, 5], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[7, 6], normalBarrierSize)));
            Objects.Add(new Barrier(new Rectangle(grid[7, 7], normalBarrierSize)));

            dynamic barrierData = JSONParser.ReadJsonFile("Content/JSONPrefab/barriers.json");

         //   make barrier width / height 36x36
        }

        /* Only works for the default room so far, quick implementation for testing  */
        public void generateGrid(Point[,] gridMap)
        {
            for(int row = 0; row < gridMap.GetLength(0); row++)
            {
                for(int col = 0; col < gridMap.GetLength(1); col++)
                {
                    // Top left corner
                    if (row == 0 && col == 0) {
                        gridMap[row, col] = new Point(Barrier.WallThickness,Barrier.WallThickness) ;
                    }
                    // Left side
                    else if(row != 0 && col == 0)
                    {
                        gridMap[row, col] = new Point(Barrier.WallThickness, Barrier.WallThickness + (SQUAREHEIGHT * row));
                    }
                    // Top Side
                    else if(row == 0 && col != 0)
                    {
                        gridMap[row, col] = new Point(Barrier.WallThickness + (SQUAREWIDTH * col), Barrier.WallThickness);
                    }
                    else 
                    {
                        gridMap[row, col] = new Point(Barrier.WallThickness + (SQUAREWIDTH * col),  Barrier.WallThickness + (SQUAREHEIGHT * row));
                    }
                    
                }
            }
        }


        /*
        public void generateGrid(Rectangle[,] gridMap)
        {
            int rowSpot = 200;
            int colSpot = 200;

            //Get the x and y grid rectangles
            for (int row = 0; row < gridMap.GetLength(0); row++)
            {
                for (int col = 0; col < gridMap.GetLength(1); col++)
                {
                    gridMap[row, col] = new Rectangle(rowSpot, colSpot, 120, 120);
                    colSpot += 120;
                }
                rowSpot+= 120;
            }
        }
        */


        /*
        {
        "Type":"Barrier",
        "X":4,
        "Y":5,
        }
        */
        /*
        if (json.Type.Equals("Barrier")) {
    Objects.add(new Barrier(new Rectangle(startX+json.X* gridWidth, startY+json.Y* gridHeight, gridWidth, gridHeight)));
}
*/
}

}
