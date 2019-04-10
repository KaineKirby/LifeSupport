using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSupport.Config {
    class Assets {

        //singleton reference
        public static Assets instance ;
        public static Assets Instance {
            get {
                if (instance != null)
                    return instance ;
                else 
                    return new Assets() ;
            }
            private set {
                instance = value ;
            }
        }

        public Texture2D background ;
        public Texture2D mainMenuBackground;
        public Texture2D player ;
        public Texture2D playerLegs ;
        public Texture2D barrier ;
        public Texture2D floorTile ;
        public Texture2D openDoor ;
        public Texture2D openDoorH ;
        public Texture2D closeDoor ;
        public Texture2D closeDoorH ;
        public Texture2D cursor;
        public Texture2D projectile;
        public Texture2D btnTexture;
        public Texture2D btnTextureLarge;
        public SpriteFont btnText;
        public SpriteFont defText;

        // Alien Dog
        public Texture2D alienDog;
        
        

        private Assets() {
            instance = this ;
        }

        public void LoadContent(Game game) {
            background = game.Content.Load<Texture2D>("img/background") ;
            mainMenuBackground = game.Content.Load<Texture2D>("img/menus/main_menu");
            player = game.Content.Load<Texture2D>("img/player/player") ;
            playerLegs = game.Content.Load<Texture2D>("img/player/player_legs") ;
            barrier = game.Content.Load<Texture2D>("img/objects/barrier") ;
            floorTile = game.Content.Load<Texture2D>("img/objects/floor_tile") ;
            openDoor = game.Content.Load<Texture2D>("img/objects/open_door") ;
            openDoorH = game.Content.Load<Texture2D>("img/objects/open_door_h") ;
            closeDoor = game.Content.Load<Texture2D>("img/objects/closed_door") ;
            closeDoorH = game.Content.Load<Texture2D>("img/objects/closed_door_H") ;
            cursor = game.Content.Load<Texture2D>("img/cursor");
            projectile = game.Content.Load<Texture2D>("img/objects/projectile");
            btnTexture = game.Content.Load<Texture2D>("img/controls/blk_button");
            btnTextureLarge = game.Content.Load<Texture2D>("img/controls/blk_button_large");
            btnText = game.Content.Load<SpriteFont>("fonts/default_ui_18");
            alienDog = game.Content.Load<Texture2D>("img/enemies/circle");
        }

    }
}
