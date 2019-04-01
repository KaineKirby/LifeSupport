using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LifeSupport.Config;
using LifeSupport.GameObjects ;
using LifeSupport.Random;
using LifeSupport.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;

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


        public const int GridSquareWidth = 30;
        public const int GridSquareHeight = 30;  


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


            Point normalBarrierSize = new Point(GridSquareWidth, GridSquareHeight);

            // Instantiate a 2D array of points with 64 rows and 36 Columns
            Point[,] grid = new Point[64, 36];

            // Set each point in the 2D array to a (x,y) coordinate within the room(each tile on the grid is 30x30)
            generateGrid(grid);

            // Read in a json file with a barrier object
            dynamic jsonData = JSONParser.ReadJsonFile("Content/RoomPrefabs/roomObjects.json");

            // Set the size of the barrier using the values set in the json file
            Point jsonBarrierSize = new Point((int)jsonData.XSize, (int)jsonData.YSize);

            // Create a barrier 
            if (jsonData.Type == "Barrier"){
                Objects.Add(new Barrier(new Rectangle(grid[StartX + (int)jsonData.BeginX, StartY + (int)jsonData.BeginY], jsonBarrierSize)));
            }

                /*
                if (jsonData.Type == "Barrier"){
                    for(int count = 0; count < jsonData.X.Count || count < jsonData.Y.Count; count++)
                    {   
                        Objects.Add(new Barrier(new Rectangle(grid[StartX + (int)jsonData.X[count], StartY + (int)jsonData.Y[count]], normalBarrierSize)));
                    }
                }
                */


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

        }

        /* Split the room up into grid squares */
        public void generateGrid(Point[,] gridMap)
        {
            for (int row = 0; row < gridMap.GetLength(0); row++)
            {
                for (int col = 0; col < gridMap.GetLength(1); col++)
                {
                    gridMap[row, col] = new Point(GridSquareWidth * row, GridSquareHeight * col);
                }
            }
        }

    }

}
