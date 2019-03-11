using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LifeSupport.Config;
using LifeSupport.GameObjects ;
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
        public ArrayList Objects ;

        //the player (to know whether player is in room or not)
        private Player player ;

        //the game the room is in
        private Game game ;

        //whether the room has been defeated or not
        private bool isBeaten ;

        //whether the room has the player in it or not
        private bool isActive ;

        //the width and height of the room in barrier walls
        public int Width ;
        public int Height ;

        //the starting point for room construction
        public int StartX ;
        public int StartY ;

        public Room(Player player, Game game, int startX, int startY) {

            this.player = player ;
            this.isBeaten = false ;
            this.isActive = true ;
            this.game = game ;

            this.StartX = startX ;
            this.StartY = startY ;

            //width and height of room in pixels
            this.Width = 1920 ;
            this.Height = 1080 ;

            this.Objects = new ArrayList() ;

            GenerateRoom() ;

        }

        //update all the objects in the room
        public void UpdateObjects(GameTime gameTime) {

            if (isActive) {
                foreach (GameObject obj in Objects) {
                    obj.UpdatePosition(gameTime) ;
                }
            }

        }

        public void RenderObjects(SpriteBatch spriteBatch) {
            //render the tile floor
            spriteBatch.Draw(Assets.Instance.floorTile, new Vector2(StartX, StartY), new Rectangle(0, 0, Width, Height), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1) ;

            if (isActive) {
                foreach (GameObject obj in Objects) {
                    obj.Render(spriteBatch) ;
                }
            }
        }

        //fills the room with game objects from our prefab set
        //TODO for now this just makes a box
        private void GenerateRoom() {

            //build the walls for the room

            //top no door
            Objects.Add(new Barrier(StartX, StartY, game, StartX+Width, StartY+Barrier.wallThickness)) ;
            //bottom no door
            Objects.Add(new Barrier(StartX, StartY+Height, game, StartX+Width, StartY+Barrier.wallThickness+Height)) ;

            //left no door
            //Objects.Add(new Barrier(StartX, StartY+Barrier.wallThickness, game, StartX+Barrier.wallThickness, StartY+Height)) ;

            //left with door 
            Objects.Add(new Barrier(StartX, StartY+Barrier.wallThickness, game, StartX+Barrier.wallThickness, StartY+((Height/2)-32))) ;
            Objects.Add(new Door(StartX, StartY+(Height/2)-32, game)) ;
            Objects.Add(new Barrier(StartX, StartY+(Height/2)+32, game, StartX+Barrier.wallThickness, StartY+Height)) ;
            
            //right no door
            //Objects.Add(new Barrier(StartX+Width-Barrier.wallThickness, StartY+Barrier.wallThickness, game, StartX+Width, StartY+Height)) ;

            //right with door
            Objects.Add(new Barrier(StartX+Width-Barrier.wallThickness, StartY+Barrier.wallThickness, game, StartX+Width, StartY+(Height/2)-32)) ;
            Objects.Add(new Door(StartX+Width-Barrier.wallThickness, StartY+(Height/2)-32, game)) ;
            Objects.Add(new Barrier(StartX+Width-Barrier.wallThickness, StartY+(Height/2)+32, game, StartX+Width, StartY+Height)) ;
          

        }

    }

}
