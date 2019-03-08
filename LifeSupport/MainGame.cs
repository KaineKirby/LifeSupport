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
        FrameCounter frames ;

        Player player ;
        Room testRoom ;
        MouseControl mouse;

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
            spriteBatch = new SpriteBatch(GraphicsDevice) ;

            testRoom = new Room(player, this) ;
            player = new Player(this, testRoom) ;
            mouse = new MouseControl(this);
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

            mouse.Update(gameTime);
            player.UpdatePosition(gameTime) ;
            testRoom.UpdateObjects(gameTime) ;

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin() ;

            mouse.Draw(spriteBatch);
            //render the player and the objects in the room
            player.Render(spriteBatch) ;
            player.DrawPlayerProjectiles(spriteBatch);
            testRoom.RenderObjects(spriteBatch) ;
            //render the FPS counter if it is enabled
            if (Settings.Instance.ShowFps)
                frames.Draw(spriteBatch, gameTime) ;
            spriteBatch.End() ;

            base.Draw(gameTime);
        }
    }
}
