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
    // This class is used to interact with and view augments 
    public class AugmentSlot : Button
    {

        /*Attributes*/

        // Used to Retrieve augment information and draw it onto the screen
        private AugmentTextBox hoverBox;

        private Texture2D augmentImage ;

        // The actual augment that will be stored within the slot
        private Augmentation augment ;

        // Draw an X over the augment if the player wants to delete it (from PlayerPageState.cs)
        Texture2D X = Assets.Instance.crossOut;

        /*Constructor*/
        public AugmentSlot(Texture2D AugmentButtonTexture, AugmentTextBox box, Augmentation augment) {
            texture = AugmentButtonTexture;
            ThisColor = Color.White;
            this.hoverBox = box;
            this.augmentImage = Assets.Instance.augmentationLarge ;
            this.augment = augment ;
            
        }

        // Draw the augmentation slot
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Outline the slot with a white color
            var currColor = Color.White;

            // If hovered over, turn the outline color to gray
            if (!PlayerPageState.removeAugmentActive)
            {
                if (hover)
                {
                    currColor = Color.Gray;
                }
            }

            // If player presses destroy augment from the gear screen, outline augments with red
            // If an augment is hovered over, draw the x
            else if (PlayerPageState.removeAugmentActive)
            {
                currColor = Color.Red;
                if (hover && augment != null)
                {
                    spriteBatch.Draw(X, new Vector2(80, 26) + CurrPosition, null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, .08f);
                }
            }

            if (augment != null)
                spriteBatch.Draw(augmentImage, new Vector2(80, 26) + CurrPosition, null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, .05f) ;
            spriteBatch.Draw(texture, Rect, null, currColor, 0, Vector2.Zero, SpriteEffects.None, 0f);
            if (hover) {
                hoverBox.position = this.CurrPosition + new Vector2(100, 100) ;
                hoverBox.DrawBox(spriteBatch);
            }
        }

        // Update the augment slot (get the correct information from the text box)
        public void UpdateAugment(Augmentation augment) {
            this.augment = augment ;
            this.hoverBox.augment = augment ;
            this.hoverBox.UpdateText() ;
        }


    }
}