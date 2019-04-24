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
    public class AugmentTextBox
    {
        private Texture2D boxTexture ;
        private SpriteFont font = Assets.Instance.mediumText;
        private int BoxWidth = 310 ;

        public Vector2 position;
        public Rectangle Rect;
        public Augmentation augment ;
        private GraphicsDevice graphicsDevice ;

        public String text { get; set; }


        public AugmentTextBox(Augmentation augment, GraphicsDevice graphicsDevice) {
            this.augment = augment ;
            this.graphicsDevice = graphicsDevice ;

            UpdateText() ;

        }

        public void UpdateText() {
            this.Rect = GetDrawingRectangle();
            this.boxTexture = GenerateTexture() ;

            text = "" ;

            if (augment != null) {
                if (augment.Damage > 0f)
                    text += "+" + augment.Damage*100 + "% Damage\n" ;
                if (augment.Range > 0f)
                    text += "+" + augment.Range*100 + "% Range\n" ;
                if (augment.ShotSpeed > 0f)
                    text += "+" + augment.ShotSpeed*100 + "% Bullet Speed\n" ;
                if (augment.RateOfFire > 0f)
                    text += "+" + augment.RateOfFire*100 + "% Rate Of Fire\n" ;
                if (augment.MoveSpeed > 0f)
                    text += "+" + augment.MoveSpeed*100 + "% Movement Speed\n" ;
            }
        }


        public void DrawBox(SpriteBatch spriteBatch) {

            if (boxTexture == null)
                return ;
            spriteBatch.Draw(boxTexture, position, null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, .1f) ;
            spriteBatch.DrawString(font, text, position +  new Vector2(4, 4), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, .2f);

        }

        private Rectangle GetDrawingRectangle() {

            if (augment == null)
                return new Rectangle() ;

            int rows = 0 ;

            //increase the height of the rectangle depending on the augment
            if (augment.Damage > 0f)
                rows++ ;
            if (augment.Range > 0f)
                rows++ ;
            if (augment.ShotSpeed > 0f)
                rows++ ;
            if (augment.RateOfFire > 0f)
                rows++ ;
            if (augment.MoveSpeed > 0f)
                rows++ ;

            return new Rectangle((int)position.X + 100, (int)position.Y + 100, 310, rows*30) ;

        }

        private Texture2D GenerateTexture() {

            if (augment == null)
                return null ;

            int rows = 0 ;

            //increase the height of the rectangle depending on the augment
            if (augment.Damage > 0f)
                rows++ ;
            if (augment.Range > 0f)
                rows++ ;
            if (augment.ShotSpeed > 0f)
                rows++ ;
            if (augment.RateOfFire > 0f)
                rows++ ;
            if (augment.MoveSpeed > 0f)
                rows++ ;

            if (rows == 0)
                return null ;

            Texture2D bg = new Texture2D(graphicsDevice, 310, rows*30) ;
            Color[] data = new Color[BoxWidth*rows*30] ;
            for(int i = 0 ; i < data.Length ; i++) {
                data[i] = new Color(0, 99, 151) ;
                if (i < BoxWidth)
                    data[i] = Color.White ;
                else if (i % BoxWidth == 0)
                    data[i] = Color.White ;
                else if (i % BoxWidth == BoxWidth-1)
                    data[i] = Color.White ;
                else if (i > (BoxWidth*rows*30)-BoxWidth)
                    data[i] = Color.White ;
            }
            bg.SetData(data) ;

            return bg ;
            
        }


    }
}
