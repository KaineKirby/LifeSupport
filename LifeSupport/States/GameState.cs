using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LifeSupport.Config;
using LifeSupport.GameObjects;
using LifeSupport.HUD;
using LifeSupport.Levels;
using LifeSupport.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LifeSupport.States
{
    public class GameState : State {

        private FrameCounter frames;


        private Player player ;
        private Level level ;

        private float scale ;

        private PlayerStatsHUD pHud ;
        //private PlayerWeaponHUD wHud ;


        public GameState(MainGame game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content) {

        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteBatch bg, SpriteBatch hud) {
            //two separate sprite batches for HUD/UI/background elements and then game content itself
            //the coordinate systems should be different for these two

            //draw the background
            bg.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width/1920));
            bg.Draw(Assets.Instance.background, new Rectangle(0, 0, 1920, 1080), Color.White);
            bg.End();

            //draw the game objects
            spriteBatch.Begin(SpriteSortMode.BackToFront, null, SamplerState.PointWrap, null, null, null, Matrix.CreateTranslation(-player.Position.X, -player.Position.Y, 0)*Matrix.CreateScale(scale)*Matrix.CreateTranslation(Settings.Instance.Width*.5f, Settings.Instance.Height*.5f, 0)); // a transformation matrix is applied to keep the player centered on screen
            //render the player and the objects in the room
            level.DrawRooms(spriteBatch) ;

            spriteBatch.End();

            //draw HUD elements
            hud.Begin();
            Cursor.Instance.Draw(hud);
            //render the FPS counter if it is enabled
            if (Settings.Instance.ShowFps)
                frames.Draw(hud, gameTime);

            pHud.Draw(hud) ;
            //wHud.Draw(hud) ;

            hud.End();
        }

        public override void PostUpdate(GameTime gameTime) {

        }

        public override void Update(GameTime gameTime) {
            if (Controller.Instance.IsKeyDown(Controller.Instance.PauseGame))
                game.Exit();

            level.UpdateRooms(gameTime) ;

            Cursor.Instance.Update(gameTime);

            pHud.Update() ;
            //wHud.Update() ;
        }

        public override void Load()
        {

            game.IsMouseVisible = false ;
            // Create a new SpriteBatch, which can be used to draw textures.
            Assets.Instance.LoadContent(game);
    
            level = new Level() ;
            player = level.player ;

            if (Settings.Instance.ShowFps)
                frames = new FrameCounter(game);

            scale = (float)Settings.Instance.Width/1920 ;

            pHud = new PlayerStatsHUD(new Vector2(0, 900), player) ;
            //wHud = new PlayerWeaponHUD(new Vector2(1650, 800), player) ;
        }
       
    }
}
