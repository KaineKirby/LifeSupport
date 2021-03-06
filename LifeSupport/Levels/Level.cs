﻿using LifeSupport.GameObjects;
using LifeSupport.Random;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LifeSupport.Levels.Room;

namespace LifeSupport.Levels {

    public class Level {

        //Level class contains all the rooms

        //constants for the min/max number of rooms
        private static int minRooms = 10 ;
        private static int maxRooms = 20 ;
        private int CurrentLevel ;
        public bool gameComplete = false;

        public int CurLevel {
            get {
                return CurrentLevel ;
            }
            private set {
                CurrentLevel = value ;
            }
        }
        
        //all the rooms in the level and the player
        public List<Room> Rooms ;
        public Room ChallengeRoom ;
        public Player player ;

        public Room activeRoom ;

        public PenumbraComponent penumbra ;

        public Level(PenumbraComponent penumbra) {

            CurrentLevel = 0 ;

            player = new Player(penumbra) ;
            this.Rooms = new List<Room>() ;

            this.penumbra = penumbra ;

            NextLevel() ;

        }
        
        //generate a level and place the player in the starting room
        public void NextLevel() {

            player.OxygenTime = Player.FloorTimer ;

            //clear all of the lights from the room
            foreach (Room room in Rooms) {
                room.RemoveLights() ;
            }

            //clear all additional hulls
            penumbra.Hulls.Clear() ;

            this.Rooms = new List<Room>() ;

            //current level increment
            if(CurrentLevel == 3)
            {
                gameComplete = true;
            } else
            {
                CurrentLevel++;
            }
            
            //first we place all of the rooms down with no doors
            
            //start with beginning room
            Room start = new Room(player, this, penumbra, 0, 0, "Content/RoomPrefabs/Level"+CurrentLevel+"/Room0.json", new Point(0, 0)) ; //id of 0 is empty room
            activeRoom = start ;
            start.IsBeaten = true ;
            player.CurrentRoom = start ;
            player.HasCard = false ;
            player.Position = new Vector2(start.StartX + Room.Width/2, start.StartY + Room.Height/2) ;
            Rooms.Add(start) ; //add start to the list

            int numRooms = RandomGenerator.Instance.GetRandomIntRange(minRooms, maxRooms) ;

            //number of rooms in pool
            string[] files = Directory.GetFiles("Content/RoomPrefabs/Level"+CurrentLevel) ;
            int pool = files.Length-1 ;

            foreach (string file in files) {
                Console.WriteLine("File: " + file) ;
            }

            Console.WriteLine("Choosing from a room pool of " + pool) ;
            
            //till we have reach the number of rooms we have generated for this level and add the "boss" room at the last step
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
                if (GetRoomAtCoordinate(curCoord) == null && i < numRooms-1) {
                    Console.WriteLine("Using room prefab from " + files[chosenId]) ;
                    Rooms.Add(new Room(player, this, penumbra, curCoord.X*Room.Width, curCoord.Y*Room.Height, "Content/RoomPrefabs/Level"+CurrentLevel+"/Room"+chosenId+".json", curCoord)) ;
                    i++ ;
                }
                else if (GetRoomAtCoordinate(curCoord) == null && i == numRooms-1) {

                    //number of rooms in challenge pool
                    string[] challengeFiles = Directory.GetFiles("Content/RoomPrefabs/Level"+CurrentLevel+"/Challenge") ;
                    int cPool = challengeFiles.Length-1 ;
                    chosenId = RandomGenerator.Instance.GetRandomIntRange(0, cPool) ;
                    Console.WriteLine("Using room prefab from " + challengeFiles[chosenId]) ;
                    Rooms.Add(new Room(player, this, penumbra, curCoord.X*Room.Width, curCoord.Y*Room.Height, "Content/RoomPrefabs/Level"+CurrentLevel+"/Challenge/Room"+chosenId+".json", curCoord)) ;
                    ChallengeRoom = Rooms[Rooms.Count-1] ;
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

            //but close the doors on the challenge room
            ChallengeRoom.CloseAllDoors() ;

            //find a room to drop the keycard in
            int keycardRoom = RandomGenerator.Instance.GetRandomIntRange(1, numRooms-1) ;
            Rooms[keycardRoom].DropsCard = true ;

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
        public Room GetRoomAtCoordinate(Point point) {
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
                activeRoom.OnPlayerLeave() ;
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
