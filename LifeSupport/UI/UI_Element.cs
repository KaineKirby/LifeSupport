using System;

namespace LifeSupport.Utilities
{
    abstract class UI_Element
    {

        //Screen Position
        public float xPos;
        public float yPos;
        public float scale;

        public UI_Element(float xPos, float yPos)
        {
            this.xPos = xPos;
            this.yPos = yPos;
            this.scale = scale;
        }

        protected Texture2D loadRectangle()
        {
            vertexColors = new[]
            {
            new VertextPositionColor(new Vector3(0,0,1), Color.White),
            new VertextPositionColor(new Vector3(10,0,1), Color.White),
            new VertextPositionColor(new Vector3(10,10,1), Color.White),
            new VertextPositionColor(new Vector3(0,10,1), Color.White)
        };

            basicEffect = new BasicEffect(GraphicsDevice);

            Color[] data = new Color[rectangle.Width * rectangle.Height];
            Texture2D rectTexture = new Texture2D(GraphicsDevice, 100, 20);

            for (int i = 0; i < data.Length; ++i)
                data[i] = Color.White;

            rectTexture.SetData(data);
            return rectTexture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D rectTexture = loadRectangle();

            Vector2 position = new Vector2(100, 200);
            spriteBatch.draw(rectTexture, position, Color.white);
        }


    }
}
