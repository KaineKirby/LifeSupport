using LifeSupport.Augments;
using LifeSupport.Config;
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
        private AugmentTextBox hoverBox;
        private Texture2D augmentImage ;
        private Augmentation augment ;

        public AugmentSlot(Texture2D AugmentButtonTexture, AugmentTextBox box, Augmentation augment) {
            texture = AugmentButtonTexture;
            ThisColor = Color.White;
            this.hoverBox = box;
            this.augmentImage = Assets.Instance.augmentationLarge ;
            this.augment = augment ;
            
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var currColor = Color.White;
            if (hover)
                currColor = Color.Gray;
            if (augment != null)
                spriteBatch.Draw(augmentImage, new Vector2(80, 26) + CurrPosition, null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, .05f) ;
            spriteBatch.Draw(texture, Rect, null, currColor, 0, Vector2.Zero, SpriteEffects.None, 0f);
            if (hover) {
                hoverBox.position = this.CurrPosition + new Vector2(100, 100) ;
                hoverBox.DrawBox(spriteBatch);
            }
        }

        public void UpdateAugment(Augmentation augment) {
            this.augment = augment ;
            this.hoverBox.augment = augment ;
            this.hoverBox.UpdateText() ;
        }


    }
}