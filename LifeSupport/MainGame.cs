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
using LifeSupport.States;

namespace LifeSupport {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MainGame : Game {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteBatch bg;
        SpriteBatch hud;

        private State currState;
        private State nextState;

        public void ChangeState(State state) {
            nextState = state;
            nextState.Load();
        }


        public MainGame() {

            graphics = new GraphicsDeviceManager(this) ;
            graphics.PreferredBackBufferHeight = Settings.Instance.Height;
            graphics.PreferredBackBufferWidth = Settings.Instance.Width;
            graphics.IsFullScreen = Settings.Instance.Fullscreen;
            graphics.SynchronizeWithVerticalRetrace = false ;
            this.IsFixedTimeStep = true ;

            this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 62d);

            Content.RootDirectory = "Content";
        }


        protected override void Initialize() {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            bg = new SpriteBatch(GraphicsDevice);
            hud = new SpriteBatch(GraphicsDevice);
            currState = new MenuState(this, graphics.GraphicsDevice, Content);
            currState.Load();

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

            if(nextState != null) {
               currState = nextState;
               nextState = null;
            }
             currState.Update(gameTime);
             currState.PostUpdate(gameTime);

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            currState.Draw(gameTime, spriteBatch, bg, hud);

            base.Draw(gameTime);
        }
    }
}
