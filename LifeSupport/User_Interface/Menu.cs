using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSupport.Utilities
{

    public abstract class Menu
    {
        protected ArrayList UiObjects = new ArrayList();
        protected int screenX;
        protected int screenY;

        public Menu(int screenX, int screenY)
        {
            this.screenX = screenX;
            this.screenY = screenY;
            this.initializeUiObjects();
        }

        protected abstract void initializeUiObjects();

        public virtual void Draw(SpriteBatch spriteBatch){
            for(int i = 0; i < UiObjects.Count; i++){
                UI_Element element = (UI_Element)UiObjects[i];
                element.Draw(spriteBatch);
            }
        }
    }
}
