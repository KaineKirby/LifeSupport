using LifeSupport.Utilities;
using Microsoft.Xna.Framework.Graphics;

namespace LifeSupport.Utilities
{

    public class MainMenu : Menu
    {


        public MainMenu(int screenX, int screenY, GraphicsDevice game) : base(screenX, screenY,game)
        {

        }

        protected override void initializeUiObjects()
        {
            UI_Element whiteBorder = new UI_Element();

        }
    }
}