using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using LifeSupport.Config ;
using LifeSupport.GameObjects ;
using LifeSupport.Levels;
using LifeSupport.Utilities;

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
        Settings instance;

       //  readJSON(Settings, "Utilities/JSONsettings.json");

        public MainGame() {
            
            instance = JSONParser.readJSON(instance, "JSON/JSONsettings.json");
            graphics = new GraphicsDeviceManager(this) ;
            graphics.PreferredBackBufferHeight = instance.Height;//Settings.Instance.Height ;
            graphics.PreferredBackBufferWidth = instance.Width;//Settings.Instance.Width ;
            graphics.IsFullScreen = instance.Fullscreen;//Settings.Instance.Fullscreen ;
   //         graphics.SynchronizeWithVerticalRetrace = Settings.Instance.FpsCapped ;
    //        this.IsFixedTimeStep = Settings.Instance.FpsCapped ;
           

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

            player = new Player(this) ;
            testRoom = new Room(player, this) ;
            frames = new FrameCounter() ;
            

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

            player.UpdatePosition(gameTime, testRoom.Objects) ;
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

            frames.Update((float)gameTime.ElapsedGameTime.TotalSeconds) ;
            string fps = string.Format("FPS: {0}", (int)frames.AverageFramesPerSecond) ;

            spriteBatch.Begin() ;
            //render the player and the objects in the room
            player.Render(spriteBatch) ;
            testRoom.RenderObjects(spriteBatch) ;
            //render the FPS counter
            spriteBatch.DrawString(Content.Load<SpriteFont>("fonts/default_ui"), fps, new Vector2(0, 0), Color.White) ;
            spriteBatch.End() ;

            base.Draw(gameTime);
        }
    }
}
