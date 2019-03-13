using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public Room(Player player, Game game) {

            this.player = player ;
            this.isBeaten = false ;
            this.isActive = true ;
            this.game = game ;

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
            if (isActive) {
                foreach (GameObject obj in Objects) {
                    obj.Render(spriteBatch) ;
                }
            }
        }

        //fills the room with game objects from our prefab set
        //TODO for now this just makes a box
        private void GenerateRoom() {

            for (int i = 0 ; i < 60 ; i++) {
                for (int j = 0 ; j < 34 ; j++) {
                    if (i == 0 || i == 59 || j == 0 || j == 33) {
                        Objects.Add(new Barrier(i*32, j*32, this.game)) ;
                    }
                }
            }
            Objects.Add(new Barrier(600, 600, this.game));

        }

    }

}
