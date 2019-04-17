using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LifeSupport.States;
using LifeSupport.Utilities;

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

            //use JSONParser to read the settings file
            dynamic videoData = JSONParser.ReadJsonFile("Content/Settings/Video_Settings.json") ;
            dynamic volumeData = JSONParser.ReadJsonFile("Content/Settings/Volume_Settings.json");
            dynamic FPSData = JSONParser.ReadJsonFile("Content/Settings/FPS_Settings.json");

            //just set all the fields
            this.Width = videoData.Width;
            this.Height = videoData.Height ;
            this.Fullscreen = videoData.Fullscreen ;
            this.ShowFps = FPSData.ShowFps ;
            this.Fps = FPSData.Fps ;
            this.SfxVolume = volumeData.SfxVolume;
            this.MusVolume = volumeData.MusVolume ;

            Instance = this ;


            //get the controller instance since we may need to see it
            Controller = Controller.Instance;

            
    
        }

        public void reloadVideoSettings()
        {
                dynamic videoData = JSONParser.ReadJsonFile("Content/Settings/Video_Settings.json");
                this.Width = videoData.Width;
                this.Height = videoData.Height;
                this.Fullscreen = videoData.Fullscreen;
            
        }

        public void reloadAudioSettings()
        {
            dynamic audioData = JSONParser.ReadJsonFile("Content/Settings/Volume_Settings.json");
            this.SfxVolume = audioData.SfxVolume;
            this.MusVolume = audioData.MusVolume;
        }



    }
}
