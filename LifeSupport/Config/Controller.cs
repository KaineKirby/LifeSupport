using LifeSupport.Utilities;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Controller Class (Singleton)
 * 
 * Central access point for controls
 * 
 */

namespace LifeSupport.Config {

    class Controller {

        //singleton reference
        public static Controller instance ;
        public static Controller Instance {
            get {
                if (instance != null)
                    return instance ;
                else 
                    return new Controller() ;
            }
            private set {
                instance = value ;
            }
        }

        //all of our key controls
        //system
        public Keys PauseGame ;
        public Keys OpenInventory;

        //movement
        public Keys MoveUp ;
        public Keys MoveDown ;
        public Keys MoveLeft ;
        public Keys MoveRight ;

        private Controller() {

            dynamic controllerData = JSONParser.ReadJsonFile("Content/Settings/Control_Settings.json");

            //this is temporary for now, explicitly set the keys rather then getting them from user configuration
            this.MoveUp = (Keys) controllerData.MoveUp ;
            this.MoveDown = (Keys)controllerData.MoveDown;
            this.MoveLeft = (Keys)controllerData.MoveLeft;
            this.MoveRight = (Keys)controllerData.MoveRight;
            this.OpenInventory = (Keys)controllerData.OpenInventory;
            this.PauseGame= (Keys)controllerData.PauseGame;


            Instance = this;

        }


        public void reloadControls()
        {
            dynamic controlData = JSONParser.ReadJsonFile("Content/Settings/Control_Settings.json");
            this.MoveUp = (Keys)controlData.MoveUp;
            this.MoveDown = (Keys)controlData.MoveDown;
            this.MoveLeft = (Keys)controlData.MoveLeft;
            this.MoveRight = (Keys)controlData.MoveRight;
            this.OpenInventory = (Keys)controlData.OpenInventory;
            this.PauseGame = (Keys)controlData.PauseGame;

        }

        //just for that central access point to get in our game context
        public bool IsKeyDown(Keys key) {
            return Keyboard.GetState().IsKeyDown(key) ;
        }

        public bool IsMovingUp()
        {
            return Keyboard.GetState().IsKeyDown(MoveUp);
        }
        public bool IsMovingDown()
        {
            return Keyboard.GetState().IsKeyDown(MoveDown);
        }
        public bool IsMovingLeft()
        {
            return Keyboard.GetState().IsKeyDown(MoveLeft);
        }
        public bool IsMovingRight()
        {
            return Keyboard.GetState().IsKeyDown(MoveRight);
        }
    }

}
