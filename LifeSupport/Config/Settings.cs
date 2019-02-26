using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LifeSupport.Utilities;
using Newtonsoft.Json;

/*
 * Settings Class (Singleton)
 * 
 * Central access point for settings
 * 
 */

namespace LifeSupport.Config {

    class Settings {

        /*
        * C# Has kind of a unique way of doing setters
        * each declared variable can have the "get" and "set" properties
        * This is just the equivalent to getSettings() in Java
        * 
        * Calling the field "Instance" for clarity
        */ 

        //settings object singleton
        private static Settings instance;


        public static Settings Instance {
            get {
                if (instance != null)
                    return instance ;
                else
                    return new Settings() ;
            }
            //out setter is private meaning it can only be accesed from within the class
            private set {
                instance = value ;
            }
        }

        //screen resolution
        public int Width ;
        public int Height ;

        //fullscreen, framerate settings
        public bool Fullscreen ;
        public bool ShowFps ;
        public int Fps ;

        //volume levels
        public double SfxVolume ;
        public double MusVolume ;

        //controller object
        public Controller Controller ;



        //private constructor for the singleton
        private Settings() {


            //just set all the fields
            this.Width = 1920 ;
            this.Height = 1080 ;
            this.Fullscreen = true ;
            this.ShowFps = false ;
            this.Fps = 300 ;
            this.SfxVolume = 100 ;
            this.MusVolume = 100 ;

            Instance = this;


            //get the controller instance since we may need to see it
            Controller = Controller.Instance;
            
    
        }

    }
}
