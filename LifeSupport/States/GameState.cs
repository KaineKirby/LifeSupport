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
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Penumbra;

namespace LifeSupport.States
{
    public class GameState : State {

        private FrameCounter frames;


        private Player player ;
        private Level level ;

        private float scale ;

        private PlayerStatsHUD pHud ;
        private MiniMap mMap ;

        private int difficulty ;


        public GameState(MainGame game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content) {

        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteBatch bg, SpriteBatch hud, SpriteBatch fg) {

            //two separate sprite batches for HUD/UI/background elements and then game content itself
            //the coordinate systems should be different for these two


            //draw the background
            bg.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width/1920));
            bg.Draw(Assets.Instance.background, new Rectangle(0, 0, 1920, 1080), Color.White);
            bg.End();
            
            game.penumbra.BeginDraw() ;

            game.penumbra.Transform = Matrix.CreateTranslation(-player.Position.X, -player.Position.Y, 0)*Matrix.CreateScale(scale)*Matrix.CreateTranslation(Settings.Instance.Width*.5f, Settings.Instance.Height*.5f, 0) ;

            //draw the game objects
            spriteBatch.Begin(SpriteSortMode.BackToFront, null, SamplerState.PointWrap, null, null, null, Matrix.CreateTranslation(-player.Position.X, -player.Position.Y, 0)*Matrix.CreateScale(scale)*Matrix.CreateTranslation(Settings.Instance.Width*.5f, Settings.Instance.Height*.5f, 0)); // a transformation matrix is applied to keep the player centered on screen
            //render the player and the objects in the room
            level.DrawRooms(spriteBatch) ;

            spriteBatch.End();

            game.penumbra.Draw(gameTime) ;

            //draw HUD elements
            hud.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Matrix.CreateScale((float)Settings.Instance.Width/1920));
            //render the FPS counter if it is enabled
            if (Settings.Instance.ShowFps)
                frames.Draw(hud, gameTime);

            pHud.Draw(hud) ;
            mMap.Draw(hud) ;

            hud.End();

            fg.Begin() ;
            Cursor.Instance.Draw(fg);
            fg.End() ;

        }

        public override void PostUpdate(GameTime gameTime) {

        }

        public override void Update(GameTime gameTime) {
            if (Controller.Instance.IsKeyDown(Controller.Instance.PauseGame)) {
                game.ChangeState(new PauseState(game, graphDevice, content));

            }
            if (Controller.Instance.IsKeyDown(Controller.Instance.OpenPlayerPage))
            {
                game.ChangeState(new PlayerPageState(player, game, graphDevice, content));
            }

            level.UpdateRooms(gameTime) ;
            
            //change the song if the level changed
            if (difficulty != level.CurLevel) {
                MediaPlayer.Stop() ;
                MediaPlayer.Play(GetSong()) ;
                MediaPlayer.Volume = (float)Settings.Instance.MusVolume/100 ;
                difficulty = level.CurLevel ;
            }
            
            Cursor.Instance.Update(gameTime);

            pHud.Update() ;
            mMap.Update() ;
        }

        public override void Load() {

            game.penumbra.Lights.Clear() ;
            game.penumbra.Hulls.Clear() ;

            game.IsMouseVisible = false ;
            // Create a new SpriteBatch, which can be used to draw textures.
            Assets.Instance.LoadContent(game);
    
            level = new Level(game.penumbra) ;
            player = level.player ;

            if (Settings.Instance.ShowFps)
                frames = new FrameCounter(game);

            scale = (float)Settings.Instance.Width/1920 * 1.5f ;

            pHud = new PlayerStatsHUD(new Vector2(0, 850), player) ;
            mMap = new MiniMap(new Vector2(1650, 100), new Rectangle(0, 0, 50, 50), level) ;
            difficulty = 1 ;
        }

        public Song GetSong() {
            switch (level.CurLevel) {
                case 1:
                    return Assets.Instance.level1 ;
                case 2:
                    return Assets.Instance.level2 ; 
                case 3:
                    return Assets.Instance.level3 ;
            }

            return null ;

        }

        public void RecaculateScale() {
            scale = (float)Settings.Instance.Width/1920 * 1.5f ;
        }
       
    }
}
