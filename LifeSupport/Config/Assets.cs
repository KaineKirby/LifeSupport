using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
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

        private bool loaded ;

        public Texture2D background ;
        public Texture2D mainMenuBackground;
        public Texture2D optionsBackground;
        public Texture2D videoSettingsBackground;
        public Texture2D audioSettingsBackground;
        public Texture2D controlSettingsBackground;
        public Texture2D playerPageBackground;
        public Texture2D pauseScreen;
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
        public Texture2D btnTextureMedium;
        public Texture2D infoBox;
        public Texture2D btnTextureSmall;
        public Texture2D leftArrowButton;
        public Texture2D rightArrowButton;
        public SpriteFont btnText;
        public SpriteFont smallText;
        public SpriteFont mediumText ;
        public SpriteFont largeText;
        public Texture2D healthIcon ;
        public Texture2D oxygenIcon ;
        public Texture2D speedIcon ;
        public Texture2D moneyIcon ;
        public Texture2D activeRoom ;
        public Texture2D beatenRoom ;
        public Texture2D nonBeatenRoom ;
        public Texture2D challengeRoom ;
     

        //Enemies
        public Texture2D alienDog;
        public Texture2D alienTurret;
        public Texture2D alienInfantry;
        public Texture2D alienInfantryLegs;

        public Texture2D oxygenTank ;
        public Texture2D money ;
        public Texture2D keycard ;

        //sound effects
        public SoundEffect playerShot ;
        public SoundEffect alienShot ;
        public SoundEffect menuClick ;
        public SoundEffect healthPickup ;
        public SoundEffect moneyPickup ;
        public SoundEffect keycardPickup ;
        public SoundEffect doorOpen ;
        public SoundEffect enemyHit ;
        public SoundEffect playerHit ;


        //music
        public Song menuMusic ;
        public Song level1 ;
        public Song level2 ;
        public Song level3 ;


        private Assets() {
            instance = this ;
            loaded = false ;
        }

        public void LoadContent(Game game) {
            if (loaded)
                return ;

            loaded = true ;
            background = game.Content.Load<Texture2D>("img/background") ;
            mainMenuBackground = game.Content.Load<Texture2D>("img/menus/main_menu");
            optionsBackground = game.Content.Load<Texture2D>("img/menus/optionsMenu");
            videoSettingsBackground = game.Content.Load<Texture2D>("img/menus/videoMenu");
            audioSettingsBackground = game.Content.Load<Texture2D>("img/menus/audioMenu");
            controlSettingsBackground = game.Content.Load<Texture2D>("img/menus/controlsMenu");
            pauseScreen = game.Content.Load<Texture2D>("img/menus/pauseMenu");
            playerPageBackground = game.Content.Load<Texture2D>("img/menus/playerMenu");
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
            btnTextureMedium = game.Content.Load<Texture2D>("img/controls/mediumButton");
            infoBox = game.Content.Load<Texture2D>("img/controls/hoverBox");
            btnTextureSmall = game.Content.Load<Texture2D>("img/controls/smallBlackButton");
            btnText = game.Content.Load<SpriteFont>("fonts/default_ui_18");
            smallText = game.Content.Load<SpriteFont>("fonts/small_Font");
            mediumText = game.Content.Load<SpriteFont>("fonts/medium_font");
            largeText = game.Content.Load<SpriteFont>("fonts/large_font");

            alienDog = game.Content.Load<Texture2D>("img/enemies/alien_dog");
            alienTurret = game.Content.Load<Texture2D>("img/enemies/alien_turret");
            alienInfantry = game.Content.Load<Texture2D>("img/enemies/alien_infantry");
            alienInfantryLegs = game.Content.Load<Texture2D>("img/enemies/alien_infantry_legs");

            leftArrowButton = game.Content.Load<Texture2D>("img/controls/arrowLeft");
            rightArrowButton = game.Content.Load<Texture2D>("img/controls/arrowRight");

            //HUD
            healthIcon = game.Content.Load<Texture2D>("img/hud/health") ;
            speedIcon = game.Content.Load<Texture2D>("img/hud/speed") ;
            moneyIcon = game.Content.Load<Texture2D>("img/hud/money_icon") ;
            oxygenIcon = game.Content.Load<Texture2D>("img/hud/oxygen_icon") ;
            activeRoom = game.Content.Load<Texture2D>("img/hud/active_room") ;
            beatenRoom = game.Content.Load<Texture2D>("img/hud/beaten_room") ;
            nonBeatenRoom = game.Content.Load<Texture2D>("img/hud/non_beaten_room") ;
            challengeRoom = game.Content.Load<Texture2D>("img/hud/challenge_room") ;


            //aug machine and oxygen tank (usables)
            oxygenTank = game.Content.Load<Texture2D>("img/objects/oxygen_tank") ;

            //drops
            money = game.Content.Load<Texture2D>("img/drops/money") ;
            keycard = game.Content.Load<Texture2D>("img/drops/keycard") ;

            //sounds

            //misc
            menuClick = game.Content.Load<SoundEffect>("sound/effects/menu_click") ;
            healthPickup = game.Content.Load<SoundEffect>("sound/effects/health_pickup") ;
            moneyPickup = game.Content.Load<SoundEffect>("sound/effects/money_pickup") ;
            keycardPickup = game.Content.Load<SoundEffect>("sound/effects/keycard_pickup") ;
            doorOpen = game.Content.Load<SoundEffect>("sound/effects/door_open") ;
            enemyHit = game.Content.Load<SoundEffect>("sound/effects/enemy_hit") ;
            playerHit = game.Content.Load<SoundEffect>("sound/effects/player_hit") ;

            //gun shots
            playerShot = game.Content.Load<SoundEffect>("sound/effects/player_shot") ;
            alienShot = game.Content.Load<SoundEffect>("sound/effects/alien_shot") ;

            //music
            menuMusic = game.Content.Load<Song>("sound/music/menu_music") ;
            level1 = game.Content.Load<Song>("sound/music/level_1_music") ;
            level2 = game.Content.Load<Song>("sound/music/level_2_music") ;
            level3 = game.Content.Load<Song>("sound/music/level_3_music") ;

        }

    }
}
