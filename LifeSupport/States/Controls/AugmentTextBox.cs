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
        private Texture2D whiteBox = Assets.Instance.infoBox;
        private SpriteFont font = Assets.Instance.mediumText;

        public Vector2 position;
        public int boxWidth;
        public int boxHeight;
        public Rectangle Rect;

        public String text { get; set; }


        public AugmentTextBox(Augmentation augment)
        {
            this.boxHeight = whiteBox.Height;
            this.boxWidth = whiteBox.Width;
            this.Rect = CreateTextBox(augment);
        }


        public void DrawBox(SpriteBatch spriteBatch)
        {
            if (boxHeight > 0 && boxWidth > 0)
            {
                spriteBatch.Draw(whiteBox, Rect, Color.White);
                spriteBatch.DrawString(font, text, new Vector2(Rect.X, Rect.Y), Color.Red);
                spriteBatch.DrawString(font, text, new Vector2(Rect.X, Rect.Y), Color.Red);
                spriteBatch.DrawString(font, text, new Vector2(Rect.X, Rect.Y), Color.Red);
                spriteBatch.DrawString(font, text, new Vector2(Rect.X, Rect.Y), Color.Red);
                spriteBatch.DrawString(font, text, new Vector2(Rect.X, Rect.Y), Color.Red);
                spriteBatch.DrawString(font, text, new Vector2(Rect.X, Rect.Y), Color.Red);
                spriteBatch.DrawString(font, text, new Vector2(Rect.X, Rect.Y), Color.Red);
            }
        }


        public Rectangle CreateTextBox(Augmentation augment)
        {
            Rectangle Box;
            if (augment == null)
            {
                text = "";
                return Box = new Rectangle(0, 0, 0, 0);
            }
            int horizontalBoxSections = 5;
            int oneHorizontalBoxSection = boxHeight / horizontalBoxSections;
            int rowsRemoved = 0;

            string damage = augment.Damage.ToString();
            string range = augment.Range.ToString();
            string shotSpeed = augment.ShotSpeed.ToString();
            string rateOfFire = augment.RateOfFire.ToString();
            string moveSpeed = augment.MoveSpeed.ToString();

            if (damage.Equals("0"))
            {
                boxHeight -= oneHorizontalBoxSection;
                rowsRemoved++;
            }
            else if (!damage.Equals("0"))
            {
                text += "+" + damage + " Damage \n";
            }

            if (range.Equals("0"))
            {
                boxHeight -= oneHorizontalBoxSection;
                rowsRemoved++;
            }
            else if (!range.Equals("0"))
            {
                text += "+" + range + " Range \n";
            }

            if (shotSpeed.Equals("0"))
            {
                boxHeight -= oneHorizontalBoxSection;
                rowsRemoved++;
            }
            else if (!shotSpeed.Equals("0"))
            {
                text += "+" + shotSpeed + " Shot Speed \n";
            }

            if (rateOfFire.Equals("0"))
            {
                boxHeight -= oneHorizontalBoxSection;
                rowsRemoved++;
            }
            else if (!rateOfFire.Equals("0"))
            {
                text += "+" + rateOfFire + " Rate of Fire \n";
            }

            if (moveSpeed.Equals("0"))
            {
                boxHeight -= oneHorizontalBoxSection;
                rowsRemoved++;
            }
            else if (!moveSpeed.Equals("0"))
            {
                text += "+" + moveSpeed + " Movement Speed \n";
            }

            if (rowsRemoved == 5)
            {
                boxHeight = 0;
                boxWidth = 0;
                Box = new Rectangle((int)augment.position.X + 100, (int)augment.position.Y + 100, boxWidth, boxHeight);
                return Box;
            }
            else
            {
                Box = new Rectangle((int)augment.position.X + 100, (int)augment.position.Y + 100, boxWidth, boxHeight);
                Console.WriteLine(Box);
                return Box;
            }
        }




    }
}
