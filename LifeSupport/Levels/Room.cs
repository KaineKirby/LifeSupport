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
using Penumbra;
using RoyT.AStar;

namespace LifeSupport.Levels {

    public class Room {


        //an array of game objects
        public List<GameObject> Objects ;

        /* 
         * Every room contains a grid
         * This grid is a 2D array that stores the top left x and y coordinates of every tile on the grid
         * The purpose of this grid is to place objects into the room with json files
         */
        public Point[,] gridPoints;

        /* This grid is created using the RoyT.AStar Library
         * This grid is used to implement pathfinding for enemies.
         */
        public Grid gridTiles;

        //the player (to know whether player is in room or not)
        public Player player ;


        //whether the room has been defeated or not
        public bool IsBeaten ;

        //the width and height of the room in pixels
        public static int Width = 1920 ;
        public static int Height = 1080 ;

        //the starting point for room construction (top left)
        /*StartX and StartY both are currently set at 0 */
        public int StartX ;
        public int StartY ;

        // Every tile on the grid is 30x30. There are a total of 36 rows and 64 columns
        // This means the room is 1920x1080
        public const int SquareTileLength = 30 ;

        //the prefab file path
        private string filepath ;
        public enum DoorSpot {Top, Bottom, Left, Right} ;
        //the coordinate of the room on the level grid
        public Point coordinate ;

        //the current level
        private Level level ;

        //whether or not the room will drop a keycard
        public bool DropsCard ;

        //penumbra component
        PenumbraComponent penumbra ;

        //the slight light in the room
        private PointLight light1 ;
        private PointLight light2 ;
        private PointLight light3 ;
        private PointLight light4 ;
        private PointLight light5 ;
       

        public Room(Player player, Level level, PenumbraComponent penumbra, int startX, int startY, string filepath, Point coordinate, bool dropsCard = false) {

            this.player = player ;
            this.IsBeaten = false ;
            this.coordinate = coordinate ; 
            this.filepath = filepath ;
            this.level = level ;
            
            this.StartX = startX ;
            this.StartY = startY ;


            this.Objects = new List<GameObject>() ;

            // Grid points is a 2D array to placed objects into the room
            this.gridPoints = new Point[36, 64];

            // Set each point in the 2D array to a (x,y) coordinate within the room(each tile on the grid is 30x30)
            generatePointGrid(gridPoints);

            // Grid is used for pathfinding
            this.gridTiles = new Grid(36, 64);

            this.DropsCard = dropsCard ;

            this.penumbra = penumbra ;


            this.light1 = new PointLight {
                Position = new Vector2(StartX + Width/4, StartY + Height/4),
                Intensity = 1f,
                Scale = new Vector2(1000f),
            } ;
            this.light2 = new PointLight {
                Position = new Vector2(StartX + Width/4, StartY + 3*Height/4),
                Intensity = 1f,
                Scale = new Vector2(1000f),
            } ;
            this.light3 = new PointLight {
                Position = new Vector2(StartX + 3*Width/4, StartY + 3*Height/4),
                Intensity = 1f,
                Scale = new Vector2(1000f)
            } ;
            this.light4 = new PointLight {
                Position = new Vector2(StartX + 3*Width/4, StartY + Height/4),
                Intensity = 1f,
                Scale = new Vector2(1000f)
            } ;
            this.light5 = new PointLight {
                Position = new Vector2(StartX + Width/2, StartY + Height/2),
                Intensity = 1f,
                Scale = new Vector2(500f),
                Color = Color.Red
            } ;
            
            penumbra.Lights.Add(light1) ;
            penumbra.Lights.Add(light2) ;
            penumbra.Lights.Add(light3) ;
            penumbra.Lights.Add(light4) ;
            penumbra.Lights.Add(light5) ;

            FillRoom();
        }

        //update all the objects in the room
        public void UpdateObjects(GameTime gameTime) {
            for (int i = 0; i < Objects.Count; i++) {
                Objects[i].UpdatePosition(gameTime) ;
            }

        }

        public void RenderObjects(SpriteBatch spriteBatch) {

            //render the tile floor
            spriteBatch.Draw(Assets.Instance.floorTile, new Vector2(StartX, StartY), new Rectangle(0, 0, Width, Height), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, .1f) ;

            for (int i = 0; i < Objects.Count; i++) {
                Objects[i].Draw(spriteBatch) ;
            }
        }

        public void DestroyObject(GameObject obj) {
            Objects.Remove(obj);
            //if all the enemies are gone we can open all the doors in the room
            if (!HasEnemies() && !IsBeaten) {
                OnRoomComplete() ;
            }
        }

        public void AddObject(GameObject obj) {
            Objects.Add(obj) ;
        }

        public void RemoveLights() {
            penumbra.Lights.Remove(light1) ;
            penumbra.Lights.Remove(light2) ;
            penumbra.Lights.Remove(light3) ;
            penumbra.Lights.Remove(light4) ;
            penumbra.Lights.Remove(light5) ;

            //remove lights from any alive actors
            foreach (GameObject obj in Objects) {
                if (obj is Actor)
                    ((Actor)obj).RemoveLight() ;
            }
        }

        //called when the room is beaten
        private void OnRoomComplete() {
            //roll for a chance to drop an item

            int roll = RandomGenerator.Instance.GetRandomIntRange(0, 7) ;

            //25 percent chance to drop some money
            if (roll == 0) {
                Console.WriteLine("Dropped money") ;
                int amount = RandomGenerator.Instance.GetRandomIntRange((level.CurLevel-1)*10 + 1, (level.CurLevel-1)*10 + 10) ;
                AddObject(new Money(new Vector2(StartX + 960, StartY + 540), player, this, amount)) ;
            }
            else if (roll == 1) {
                Console.WriteLine("Dropped health") ;
                AddObject(new Health(new Vector2(StartX + 960, StartY + 540), player, this)) ;
            }

            //drop the keycard if the room is designated to do so
            if (DropsCard) {
                Console.WriteLine("Dropped keycard") ;
                AddObject(new Keycard(new Vector2(StartX + 960, StartY + 600), player, this, level)) ;
            }
            
            Assets.Instance.doorOpen.Play((float)Settings.Instance.SfxVolume/100, 0f, 0f) ;

            this.light5.Color = Color.Green ;

            OpenAllDoors() ;
            IsBeaten = true ;
        }


        /* Fill the 2D grid arrays with default values.
         * gridPoints is filled with points. The primary purpose
         * of gridPoints is to place objects in specific rows and columns
         within the room*/
        public void generatePointGrid(Point[,] gridMap)
        {
            for (int row = 0; row < gridMap.GetLength(0); row++)
            {
                for (int col = 0; col < gridMap.GetLength(1); col++)
                {
                    gridMap[row, col] = new Point(StartX + (SquareTileLength * col), StartY + (SquareTileLength * row));
             
                }
            }
        }

        //adds a door to the given side
        public void AddDoor(DoorSpot side) {
                        
            switch (side) {
                case DoorSpot.Top:
                    //top
                    Objects.RemoveAt(1) ;
                    Objects.Insert(1, new DoorH(new Vector2(StartX + (Width/2) - Barrier.WallThickness, StartY), penumbra)) ;
                    break ;
                case DoorSpot.Bottom:
                    //bottom
                    Objects.RemoveAt(4) ;
                    Objects.Insert(4, new DoorH(new Vector2(StartX + (Width/2) - Barrier.WallThickness, StartY + Height - Barrier.WallThickness), penumbra)) ;
                    break ;
                case DoorSpot.Left:
                    //left
                    Objects.RemoveAt(7) ;
                    Objects.Insert(7, new DoorV(new Vector2(StartX, StartY + (Height / 2) - Barrier.WallThickness))) ;
                    break ;
                case DoorSpot.Right:
                    //right
                    Objects.RemoveAt(10) ;
                    Objects.Insert(10, new DoorV(new Vector2(StartX + Width - Barrier.WallThickness, StartY + (Height / 2) - Barrier.WallThickness))) ;
                    break ;
                //this shouldn't happen
                default:
                    return ;
            }

        }

        //a check to see whether or not the room still has enemies
        //we open the doors when there are no more enemies remaining
        private bool HasEnemies() {
            foreach (GameObject obj in Objects) {
                if (obj is Enemy)
                    return true ;
            }
            return false ;
        }

        //called when the player enters the room
        //close the doors if enemies exist in the room
        public void OnPlayerEntered() {
            if (HasEnemies() && PlayerInside())
                CloseAllDoors() ;
        }

        //check whether or not the player lies within the boundaries of the room
        //so the doors can be closed
        public bool PlayerInside() {
            int padding = 16 ;
            if (player.Position.X > StartX+Barrier.WallThickness+padding && player.Position.X < StartX + Width - Barrier.WallThickness-padding &&
                player.Position.Y > StartY+Barrier.WallThickness+padding && player.Position.Y < StartY + Height - Barrier.WallThickness-padding)
                return true ;

            return false ;
        }

        //close all the doors in the room
        public void CloseAllDoors() {
            foreach (GameObject obj in Objects) {
                if (obj is Door)
                    ((Door)obj).CloseDoor() ;
            }
        }

        //open all the doors in the room
        public void OpenAllDoors() {
            if ((Objects[1] is Door) && (player.HasCard || level.ChallengeRoom.coordinate != new Point(coordinate.X, coordinate.Y-1))) {
                ((Door)Objects[1]).OpenDoor() ;
            }
            if ((Objects[4] is Door) && (player.HasCard || level.ChallengeRoom.coordinate != new Point(coordinate.X, coordinate.Y+1))) {
                ((Door)Objects[4]).OpenDoor() ;
            }
            if ((Objects[7] is Door) && (player.HasCard || level.ChallengeRoom.coordinate != new Point(coordinate.X-1, coordinate.Y))) {
                ((Door)Objects[7]).OpenDoor() ;
            }
            if ((Objects[10] is Door) && (player.HasCard || level.ChallengeRoom.coordinate != new Point(coordinate.X+1, coordinate.Y))) {
                ((Door)Objects[10]).OpenDoor() ;
            }
        }

        private void CreateOuterWalls() {

            //redoing this system slightly due to consistency
            //the index of the walls within the game object list will always be the same regardless of whether or not a door is present w/ this system

            //top
            Objects.Add(new Barrier(new Rectangle(StartX, StartY, (Width/2) - Barrier.WallThickness, Barrier.WallThickness), penumbra));
            Objects.Add(new Barrier(new Rectangle(StartX + (Width/2) - Barrier.WallThickness, StartY, Barrier.WallThickness*2, Barrier.WallThickness), penumbra, false)) ;
            Objects.Add(new Barrier(new Rectangle(StartX + (Width/2) + Barrier.WallThickness, StartY, (Width/2) - Barrier.WallThickness, Barrier.WallThickness), penumbra));

            //bottom
            Objects.Add(new Barrier(new Rectangle(StartX, StartY + Height - Barrier.WallThickness, (Width/2) - Barrier.WallThickness, Barrier.WallThickness), penumbra));
            Objects.Add(new Barrier(new Rectangle(StartX + (Width/2) - Barrier.WallThickness, StartY + Height - Barrier.WallThickness, Barrier.WallThickness*2, Barrier.WallThickness), penumbra, false)) ;
            Objects.Add(new Barrier(new Rectangle(StartX + (Width/2) + Barrier.WallThickness, StartY + Height - Barrier.WallThickness, (Width/2) - Barrier.WallThickness, Barrier.WallThickness), penumbra));

            //left
            Objects.Add(new Barrier(new Rectangle(StartX, StartY + Barrier.WallThickness, Barrier.WallThickness, (Height / 2) - 2 * Barrier.WallThickness), penumbra));
            Objects.Add(new Barrier(new Rectangle(StartX, StartY + (Height/2) - Barrier.WallThickness, Barrier.WallThickness, Barrier.WallThickness*2), penumbra, false)) ;
            Objects.Add(new Barrier(new Rectangle(StartX, StartY + (Height / 2) + Barrier.WallThickness, Barrier.WallThickness, (Height / 2) - Barrier.WallThickness * 2), penumbra));

            //right
            Objects.Add(new Barrier(new Rectangle(StartX + Width - Barrier.WallThickness, StartY + Barrier.WallThickness, Barrier.WallThickness, (Height / 2) - 2 * Barrier.WallThickness), penumbra));
            Objects.Add(new Barrier(new Rectangle(StartX + Width - Barrier.WallThickness, StartY + (Height/2) - Barrier.WallThickness, Barrier.WallThickness, Barrier.WallThickness*2), penumbra, false)) ;
            Objects.Add(new Barrier(new Rectangle(StartX + Width - Barrier.WallThickness, StartY + (Height / 2) + Barrier.WallThickness, Barrier.WallThickness, (Height / 2) - Barrier.WallThickness * 2), penumbra));
        }


        private void FillRoom()
        {
            CreateOuterWalls();

            //block off the outer edges
            for (int row = 0 ; row < Height/30 ; row++) {
                for (int col = 0 ; col < Width/30 ; col++) {
                    if (row == 0 || col == 0 || row == (Height/30) - 1 || col == (Width/30) - 1)
                        gridTiles.BlockCell(new Position(row, col)) ;
                }
            }


            // Read in a json file with a barrier object
            dynamic jsonData = JSONParser.ReadJsonFile(filepath);

            int count = 1;
            for (int i = 0; jsonData.Barrier != null && i < jsonData.Barrier.Count; i++) {

                Point jsonBarrierSize = new Point((int)(jsonData.Barrier[i].BarrierWidth*30), ((int)jsonData.Barrier[i].BarrierHeight*30));
                Objects.Add(new Barrier(new Rectangle(gridPoints[jsonData.Barrier[i].Row, jsonData.Barrier[i].Column], jsonBarrierSize), penumbra));

                gridTiles.BlockCell(new Position((int)jsonData.Barrier[i].Row, (int)jsonData.Barrier[i].Column));

                if (jsonBarrierSize.X > 30)
                {
                    jsonBarrierSize.X -= 30;
                    while (jsonBarrierSize.X > 0)
                    {
                        gridTiles.BlockCell(new Position((int)jsonData.Barrier[i].Row, (int)jsonData.Barrier[i].Column + count));
                        jsonBarrierSize.X -= 30;
                        count++;
                    }
                    count = 1;
                }
                if (jsonBarrierSize.Y > 30)
                {
                    jsonBarrierSize.Y -= 30;
                    while (jsonBarrierSize.Y > 0)
                    {
                        gridTiles.BlockCell(new Position((int)jsonData.Barrier[i].Row + count, (int)jsonData.Barrier[i].Column)); 
                        jsonBarrierSize.Y -= 30;
                        count++;
                    }
                    count = 1;
                }

            }

            for(int i = 0; jsonData.AlienDog != null && i < jsonData.AlienDog.Count;i++) {
                Objects.Add(new AlienDog(player, 
                    new Vector2(StartX + (15) + (SquareTileLength * (int)jsonData.AlienDog[i].Column), 
                    StartY + (15) + (SquareTileLength * (int)jsonData.AlienDog[i].Row)), 
                    penumbra,
                    this, (float)jsonData.AlienDog[i].Speed, 
                    (float)jsonData.AlienDog[i].Health, 
                    (float)jsonData.AlienDog[i].Damage));
            }
            for (int i = 0; jsonData.AlienTurret != null && i < jsonData.AlienTurret.Count; i++) {
                Objects.Add(new AlienTurret(player, 
                    new Vector2(StartX + (15) + (SquareTileLength * (int)jsonData.AlienTurret[i].Column), StartY + (15) + (SquareTileLength * (int)jsonData.AlienTurret[i].Row)), 
                    penumbra,
                    this, 
                    (float)jsonData.AlienTurret[i].Speed, 
                    (float)jsonData.AlienTurret[i].Health, 
                    (float)jsonData.AlienTurret[i].Damage, 
                    (float)jsonData.AlienTurret[i].Range, 
                    (float)jsonData.AlienTurret[i].ShotSpeed, 
                    (float)jsonData.AlienTurret[i].RateOfFire));
            }
            for (int i = 0; jsonData.AlienInfantry != null && i < jsonData.AlienInfantry.Count; i++)
            {
                Objects.Add(new AlienInfantry(player,
                    new Vector2(StartX + (15) + (SquareTileLength * (int)jsonData.AlienInfantry[i].Column), StartY + (15) + (SquareTileLength * (int)jsonData.AlienInfantry[i].Row)),
                    penumbra,
                    this,
                    (float)jsonData.AlienInfantry[i].Speed,
                    (float)jsonData.AlienInfantry[i].Health,
                    (float)jsonData.AlienInfantry[i].Damage,
                    (float)jsonData.AlienInfantry[i].Range,
                    (float)jsonData.AlienInfantry[i].ShotSpeed,
                    (float)jsonData.AlienInfantry[i].RateOfFire));
            }
            for (int i = 0 ; jsonData.OxygenTank != null && i < jsonData.OxygenTank.Count ; i++) {
                Objects.Add(new OxygenTank(new Vector2(StartX + (15) + (SquareTileLength * (int)jsonData.OxygenTank[i].Column), StartY + (15) + (SquareTileLength * (int)jsonData.OxygenTank[i].Row)),
                    level, this, player)) ;
            }

        }

    }

}
