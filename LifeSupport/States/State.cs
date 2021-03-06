﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSupport.States
{
    // This class serves as the parent class for all states (screens) within the game
    public abstract class State
    {
        #region Fields
        protected ContentManager content;
        protected MainGame game;
        protected GraphicsDevice graphDevice;
        #endregion

        #region Methods
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteBatch bg, SpriteBatch hud, SpriteBatch fg);
        public abstract void PostUpdate(GameTime gameTime);
        public abstract void Load();
        public State(MainGame state_game, GraphicsDevice state_graphDevice, ContentManager state_content)
        {
            game = state_game;
            graphDevice = state_graphDevice;
            content = state_content;
        }
        public abstract void Update(GameTime gameTime);

        #endregion
    }
}
