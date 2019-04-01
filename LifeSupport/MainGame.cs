using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using LifeSupport.Config ;
using LifeSupport.GameObjects ;
using LifeSupport.Levels;
using LifeSupport.Utilities;
using System;

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LifeSupport {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MainGame : Game {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteBatch hud ;
        SpriteBatch bg ;
        FrameCounter frames ;

        Player player ;
        Room testRoom ;
        
        

        public MainGame() {

            graphics = new GraphicsDeviceManager(this) ;
            graphics.PreferredBackBufferHeight = Settings.Instance.Height;
            graphics.PreferredBackBufferWidth = Settings.Instance.Width;
            graphics.IsFullScreen = Settings.Instance.Fullscreen;
            graphics.SynchronizeWithVerticalRetrace = false ;
            this.IsFixedTimeStep = false ;

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            Assets.Instance.LoadContent(this);
            spriteBatch = new SpriteBatch(GraphicsDevice) ;
            hud = new SpriteBatch(GraphicsDevice) ;
            bg = new SpriteBatch(GraphicsDevice) ;


            testRoom = new Room(player, 0, 0) ;
            player = new Player(testRoom) ;        
            if (Settings.Instance.ShowFps)
                frames = new FrameCounter(this) ;


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            if (Controller.Instance.IsKeyDown(Controller.Instance.PauseGame))
                Exit();

            player.UpdatePosition(gameTime) ;
            testRoom.UpdateObjects(gameTime) ;

            Cursor.Instance.Update(gameTime);
            
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            //two separate sprite batches for HUD/UI/background elements and then game content itself
            //the coordinate systems should be different for these two

            //draw the background
            bg.Begin() ;
            bg.Draw(Assets.Instance.background, new Rectangle(0, 0, 1920, 1080), Color.White) ;
            bg.End() ;

            //draw the game objects
            spriteBatch.Begin(SpriteSortMode.BackToFront, null, SamplerState.PointWrap, null, null, null, Matrix.CreateTranslation(-player.Position.X+960, -player.Position.Y+540, 0)) ; // a transformation matrix is applied to keep the player centered on screen
            //render the player and the objects in the room
            testRoom.RenderObjects(spriteBatch) ;
            player.Draw(spriteBatch) ;
            
            spriteBatch.End() ;

            //draw HUD elements
            hud.Begin() ;
            Cursor.Instance.Draw(hud);
            //render the FPS counter if it is enabled
            if (Settings.Instance.ShowFps)
                frames.Draw(hud, gameTime) ;
            hud.End() ;

            base.Draw(gameTime);
        }
    }
}
