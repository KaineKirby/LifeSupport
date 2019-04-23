﻿using LifeSupport.Config;
using LifeSupport.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace LifeSupport.States.Controls
{
    public class AugmentSlot : Button
    {
        AugmentTextBox hoverBox;

        public AugmentSlot(Texture2D AugmentButtonTexture, AugmentTextBox box)
        {
            texture = AugmentButtonTexture;
            ThisColor = Color.White;
            this.hoverBox = box;
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var currColor = Color.White;
            if (hover)
                currColor = Color.Gray;
            spriteBatch.Draw(texture, Rect, null, currColor, 0, Vector2.Zero, SpriteEffects.None, 0f);
            if (hover) {
                hoverBox.position = this.CurrPosition;
                hoverBox.DrawBox(spriteBatch);
                hoverBox.DrawBox(spriteBatch);
                hoverBox.DrawBox(spriteBatch);
                hoverBox.DrawBox(spriteBatch);
                hoverBox.DrawBox(spriteBatch);
            }
        }


    }
}