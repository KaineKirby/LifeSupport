using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LifeSupport.Utilities
{
    public abstract class UI_Element
    {

        //Screen Position
        public float xPos;
        public float yPos;
        public float scale;
        public GraphicsDevice game;

        public UI_Element(float xPos, float yPos, GraphicsDevice game)
        {
            this.xPos = xPos;
            this.yPos = yPos;
            this.game = game;
        }

        protected Texture2D loadRectangle()
        {

            Color[] data = new Color[100 * 20];
            Texture2D rectTexture = new Texture2D(this.game, 100, 20);

            for (int i = 0; i < data.Length; ++i)
                data[i] = Color.White;

            rectTexture.SetData(data);
            return rectTexture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D rectTexture = loadRectangle();

            Vector2 position = new Vector2(100, 200);
            spriteBatch.Draw(rectTexture, position, Color.White);
        }
    }
}
