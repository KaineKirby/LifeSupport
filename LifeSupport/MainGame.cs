using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using LifeSupport.Config ;
using LifeSupport.GameObject ;

namespace LifeSupport {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MainGame : Game {

        private static MainGame instance ;
        public static MainGame Instance {
            get {
                if (instance != null)
                    return instance ;
                else
                    return new MainGame() ;
            }
            private set {
                instance = value ;
            }
        }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Player player ;

        private MainGame() {
            graphics = new GraphicsDeviceManager(this) ;
            graphics.PreferredBackBufferHeight = Settings.Instance.Height ;
            graphics.PreferredBackBufferWidth = Settings.Instance.Width ;
            graphics.IsFullScreen = Settings.Instance.Fullscreen ;
            graphics.SynchronizeWithVerticalRetrace = Settings.Instance.FpsCapped ;
            this.IsFixedTimeStep = Settings.Instance.FpsCapped ;
           

            Content.RootDirectory = "Content";

            instance = this ;
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

            player = Player.Instance ;

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
            player.Render(spriteBatch) ;
            spriteBatch.End() ;

            base.Draw(gameTime);
        }
    }
}
