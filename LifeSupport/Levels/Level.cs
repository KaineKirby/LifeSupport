using LifeSupport.GameObjects;
using LifeSupport.Random;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LifeSupport.Levels.Room;

namespace LifeSupport.Levels {

    class Level {

        //constants for the min/max number of rooms
        private static int minRooms = 10 ;
        private static int maxRooms = 20 ;
        
        //all the rooms in the level and the player
        public List<Room> Rooms ;
        public Player player ;

        public Room activeRoom ;

        public Level() {

            this.Rooms = new List<Room>() ;

            GenerateLevel() ;

        }
        
        //generate a level and place the player in the starting room
        private void GenerateLevel() {

            //first we place all of the rooms down with no doors
            
            //start with beginning room
            player = new Player() ;
            Room start = new Room(player, 0, 0, 0, new Point(0, 0)) ; //id of 0 is empty room
            activeRoom = start ;
            player.CurrentRoom = start ;
            player.Position = new Vector2(start.StartX + Room.Width/2, start.StartY + Room.Height/2) ;
            Rooms.Add(start) ; //add start to the list

            int numRooms = RandomGenerator.Instance.GetRandomIntRange(minRooms, maxRooms) ;

            //number of rooms in pool
            string[] files = Directory.GetFiles("Content/RoomPrefabs/") ;
            int pool = files.Length-1 ;

            foreach (string file in files) {
                Console.WriteLine("File: " + file) ;
            }

            Console.WriteLine("Choosing from a room pool of " + pool + files[2]) ;
            
            //till we have reach the number of rooms we have generated for this level
            for (int i = 0 ; i < numRooms ;) {
                //select a random room from the room list
                Room curRoom = Rooms[RandomGenerator.Instance.GetRandomIntRange(0, Rooms.Count-1)] ;
                Point curCoord = curRoom.coordinate ;
                int chosenId = RandomGenerator.Instance.GetRandomIntRange(1, pool) ;

                //pick a random side
                int side = RandomGenerator.Instance.GetRandomIntRange(0, 3) ;

                switch (side) {
                    case 0:
                        curCoord.Y-- ;
                    break  ;
                    case 1:
                        curCoord.Y++ ;
                    break ;
                    case 2:
                        curCoord.X++ ;
                    break ;
                    case 3:
                        curCoord.X-- ;
                    break ;
                    default:
                        continue ;
                }

                //if that coordinate is not chosen then we can place a room there
                if (GetRoomAtCoordinate(curCoord) == null) {
                    Rooms.Add(new Room(player, curCoord.X*Room.Width, curCoord.Y*Room.Height, chosenId, curCoord)) ;
                    Console.WriteLine("Using room prefab from " + files[chosenId]) ;
                    i++ ;
                }

            }

            //next we traverse the list and place the doors where they need to be
            foreach (Room room in Rooms) {
                Point coord = room.coordinate ;
                Point above = new Point (coord.X, coord.Y-1) ;
                Point below = new Point (coord.X, coord.Y+1) ;
                Point left = new Point (coord.X-1, coord.Y) ;
                Point right = new Point (coord.X+1, coord.Y) ;

                if (GetRoomAtCoordinate(above) != null) {
                    GetRoomAtCoordinate(coord).AddDoor(DoorSpot.Top) ;
                }
                if (GetRoomAtCoordinate(below) != null) {
                    GetRoomAtCoordinate(coord).AddDoor(DoorSpot.Bottom) ;
                }
                if (GetRoomAtCoordinate(left) != null) {
                    GetRoomAtCoordinate(coord).AddDoor(DoorSpot.Left) ;
                }
                if (GetRoomAtCoordinate(right) != null) {
                    GetRoomAtCoordinate(coord).AddDoor(DoorSpot.Right) ;
                }

                //open all the doors by default
                GetRoomAtCoordinate(coord).OpenAllDoors() ;
            }

        }

        //get the respective room where the player is located
        private Point GetPlayerRoomPosition() {
            int x = (int) player.Position.X ;
            int y = (int) player.Position.Y ;
            if (x < 0)
                x = x-Room.Width ;
            if (y < 0)
                y = y-Room.Height ;
            return new Point(x / Room.Width, y / Room.Height) ;
        }

        //get the room at a particular coordinate
        private Room GetRoomAtCoordinate(Point point) {
            foreach (Room room in Rooms) {
                if (room.coordinate.Equals(point))
                    return room ;
            }
            return null ;
        }

        public void UpdateRooms(GameTime gameTime) {
            //check to see if the player has moved into another room
            Room canidate = GetRoomAtCoordinate(GetPlayerRoomPosition()) ;
            if (player.CurrentRoom != canidate) {
                player.CurrentRoom = canidate ;
                activeRoom = canidate ;
            }
            if (activeRoom.PlayerInside())
                activeRoom.OnPlayerEntered() ;

            //update the active room and the player
            activeRoom.UpdateObjects(gameTime) ;
            player.UpdatePosition(gameTime) ;
        }

        public void DrawRooms(SpriteBatch spriteBatch) {
            foreach (Room room in Rooms) {
                room.RenderObjects(spriteBatch) ;
            }
            player.Draw(spriteBatch) ;
        }

    }
}
