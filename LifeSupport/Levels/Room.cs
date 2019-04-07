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
using RoyT.AStar;

namespace LifeSupport.Levels {

    /*
     * This class needs a lot of work currently
     * It needs to have a defined size so it can tell if the player is in it
     * It also needs to build the Barriers in the GenerateRoom method better (and dynamically with the size)
     * It also needs to check to see if the player is in the room or not
     *
     * And then stuff for spawning enemies later but we're nowhere near that
     */

    public class Room {

        //an array of game objects
        public List<GameObject> Objects ;

        // Every room contains a grid
        // This grid is a 2D array that stores the top left x and y coordinates of every tile on the grid
        public Point[,] gridPoints;

        // Assign a 0 (false) or 1 (true) to each point on the grid. 0 for not occupied, 1 for occupied.
        public int[,] occupiedTilesGrid;

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
        /*StartX and StartY both are currently set at 0 */
        public int StartX ;
        public int StartY ;

        public const int TileWidth = 30;
        public const int TileHeight = 30;  


        public Room(Player player, int startX, int startY) {

            this.player = player ;
            this.isBeaten = false ;
            this.isActive = true ;
            
            this.StartX = startX ;
            this.StartY = startY ;

            //width and height of room in pixels
            this.Width = 1950 ;
            this.Height = 1100 ;

            this.Objects = new List<GameObject>() ;

            // Instantiate a 2D array of points with 36 rows and 64 Columns
            this.gridPoints = new Point[37, 65];
            this.gridPoints = new Point[37, 65];

            // Instantiate a 2D array of bools for the grid to check to see if each tile is occupied by an object
            this.occupiedTilesGrid= new int[gridPoints.GetLength(0), gridPoints.GetLength(1)];

            // Set each point in the 2D array to a (x,y) coordinate within the room(each tile on the grid is 30x30)
            generateGrids(gridPoints, occupiedTilesGrid);

            FillRoom();
        }

        //update all the objects in the room
        public void UpdateObjects(GameTime gameTime) {

            if (isActive) {
                for (int i = 0; i < Objects.Count; i++) {
                    Objects[i].UpdatePosition(gameTime) ;
                }
                
            }

        }

        public void RenderObjects(SpriteBatch spriteBatch, GameTime gameTime) {
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


        /* Fill the 2D grid arrays with default values.
         * gridPoints is filled with points, and 
         * occupiedTilesGrid is filled with 0's (not obstructued) */
        public void generateGrids(Point[,] gridMap, int[,] gridOccupationMap)
        {
            for (int row = 0; row < gridMap.GetLength(0); row++)
            {
                for (int col = 0; col < gridMap.GetLength(1); col++)
                {
                    gridMap[row, col] = new Point(StartX + (TileWidth * col), StartY + (TileHeight * row));
                    gridOccupationMap[row, col] = 0;
                }
            }
        }


        private void CreateOuterWalls() {

            //top no door
            Objects.Add(new Barrier(new Rectangle(StartX, StartY, Width, Barrier.WallThickness)));
            //bottom no door
            Objects.Add(new Barrier(new Rectangle(StartX, StartY + Height - Barrier.WallThickness, Width, Barrier.WallThickness)));
            //left no door
            //Objects.Add(new Barrier(new Rectangle(StartX, StartY+Barrier.WallThickness, Barrier.WallThickness, Height-2*Barrier.WallThickness))) ;
            //right no door
            //Objects.Add(new Barrier(new Rectangle(StartX+Width-Barrier.WallThickness, StartY+Barrier.WallThickness, Barrier.WallThickness, Height-2*Barrier.WallThickness))) ;

            //left with door
            Objects.Add(new Barrier(new Rectangle(StartX, StartY + Barrier.WallThickness, Barrier.WallThickness, (Height / 2) - 2 * Barrier.WallThickness)));
            Objects.Add(new Door(new Vector2(StartX, StartY + (Height / 2) - Barrier.WallThickness)));
            Objects.Add(new Barrier(new Rectangle(StartX, StartY + (Height / 2) + Barrier.WallThickness, Barrier.WallThickness, (Height / 2) - Barrier.WallThickness * 2)));

            //right with door
            Objects.Add(new Barrier(new Rectangle(StartX + Width - Barrier.WallThickness, StartY + Barrier.WallThickness, Barrier.WallThickness, (Height / 2) - 2 * Barrier.WallThickness)));
            Objects.Add(new Door(new Vector2(StartX + Width - Barrier.WallThickness, StartY + (Height / 2) - Barrier.WallThickness)));
            Objects.Add(new Barrier(new Rectangle(StartX + Width - Barrier.WallThickness, StartY + (Height / 2) + Barrier.WallThickness, Barrier.WallThickness, (Height / 2) - Barrier.WallThickness * 2)));
        }


        private void FillRoom()
        {
            CreateOuterWalls();

            // Read in a json file with a barrier object
            dynamic jsonData = JSONParser.ReadJsonFile("Content/RoomPrefabs/RoomObjects.json");
            int count = 1;
            for (int i = 0; i < jsonData.Barrier.Count; i++)
            {
                if ((jsonData.Barrier[i].BarrierWidth > 30 && jsonData.Barrier[i].BarrierHeight > 30) ||
                    jsonData.Barrier[i].BarrierWidth < 30 || jsonData.Barrier[i].BarrierHeight < 30 ||
                    (int)jsonData.Barrier[i].BarrierWidth % 30 != 0 || (int)jsonData.Barrier[i].BarrierHeight % 30 != 0 ||
                    jsonData.Barrier[i].BarrierWidth > Width - 120 || jsonData.Barrier[i].BarrierHeight > Height - 120 ||
                    jsonData.Barrier[i].Row >= gridPoints.GetLength(0) || jsonData.Barrier[i].Column >= gridPoints.GetLength(1) ||
                    jsonData.Barrier[i].Row < 0 || jsonData.Barrier[i].Column < 0)  {

                    continue;
                }
                else  {
                    Point jsonBarrierSize = new Point((int)jsonData.Barrier[i].BarrierWidth, (int)jsonData.Barrier[i].BarrierHeight);
                    Objects.Add(new Barrier(new Rectangle(gridPoints[jsonData.Barrier[i].Row, jsonData.Barrier[i].Column], jsonBarrierSize)));

                    occupiedTilesGrid[jsonData.Barrier[i].Row, jsonData.Barrier[i].Column] = 1;

                    if (jsonBarrierSize.X > 30)
                    {
                        jsonBarrierSize.X -= 30;
                        while (jsonBarrierSize.X > 0)
                        {
                            occupiedTilesGrid[jsonData.Barrier[i].Row , jsonData.Barrier[i].Column + count] = 1;
                            jsonBarrierSize.X -= 30;
                            count++;
                        }
                        count = 1;
                    }
                    else if (jsonBarrierSize.Y > 30)
                    {
                        jsonBarrierSize.Y -= 30;
                        while (jsonBarrierSize.Y > 0)
                        {
                            occupiedTilesGrid[jsonData.Barrier[i].Row + count, jsonData.Barrier[i].Column] = 1;
                            jsonBarrierSize.Y -= 30;
                            count++;
                        }
                        count = 1;
                    }
                }
            }

            for(int i = 0; i < jsonData.AlienDog.Count;i++)
            {
                Objects.Add(new AlienDog(player, new Vector2(StartX + (TileWidth * (int)jsonData.AlienDog[i].Column), StartY + (TileHeight * (int)jsonData.AlienDog[i].Row)), this));
            }

        }




        private void checkTiles()
        {
            
        }


    }

}
