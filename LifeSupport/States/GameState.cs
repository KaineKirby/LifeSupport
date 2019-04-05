﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LifeSupport.Config;
using LifeSupport.GameObjects;
using LifeSupport.Levels;
using LifeSupport.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LifeSupport.States
{
    public class GameState : State {

        private FrameCounter frames;

        private Player player;
        private Room testRoom;

        public GameState(MainGame game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content) {

        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteBatch bg, SpriteBatch hud) {
            //two separate sprite batches for HUD/UI/background elements and then game content itself
            //the coordinate systems should be different for these two

            //draw the background
            bg.Begin();
            bg.Draw(Assets.Instance.background, new Rectangle(0, 0, 1920, 1080), Color.White);
            bg.End();

            //draw the game objects
            spriteBatch.Begin(SpriteSortMode.BackToFront, null, SamplerState.PointWrap, null, null, null, Matrix.CreateTranslation(-player.Position.X + 960, -player.Position.Y + 540, 0)); // a transformation matrix is applied to keep the player centered on screen
            //render the player and the objects in the room
            testRoom.RenderObjects(spriteBatch);
            player.Draw(spriteBatch);

            spriteBatch.End();

            //draw HUD elements
            hud.Begin();
            Cursor.Instance.Draw(hud);
            //render the FPS counter if it is enabled
            if (Settings.Instance.ShowFps)
                frames.Draw(hud, gameTime);
            hud.End();
        }

        public override void PostUpdate(GameTime gameTime) {

        }

        public override void Update(GameTime gameTime) {
            if (Controller.Instance.IsKeyDown(Controller.Instance.PauseGame))
                game.Exit();

            player.UpdatePosition(gameTime);
            testRoom.UpdateObjects(gameTime);

            Cursor.Instance.Update(gameTime);
        }

        public override void Load()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            Assets.Instance.LoadContent(game);
    
            player = new Player(testRoom);
            testRoom = new Room(player, 0, 0);
            player.CurrentRoom = testRoom;

            if (Settings.Instance.ShowFps)
                frames = new FrameCounter(game);
        }
       
    }
}