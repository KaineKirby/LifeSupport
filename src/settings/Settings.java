package settings;

/*
 * Settings Class (Singleton)
 * 
 * Central access point for settings.
 * 
 * NOTE: This is currently limited in functionality. Eventually settings will be pulled from a file rather than hard coded
 * TODO: hard coding is located in the getSettings() method
 */

public class Settings {
	
	//settings object
	public static Settings settings ;
	
	//screen resolution
	public int width ;
	public int height ;
	
	//whether or not game is fullscreen
	public boolean fullscreen ;
	public boolean framerateCapped ;
	public boolean showFramerate ;
	public int framerate ;
	
	//the volume levels for both sfx and music
	public double sfxVolume ;
	public double musVolume ;
	
	//controller object (similar to settings but for controls) TODO: controller object stub ATM
	public Controller controller ;
	
	//private constructor
	private Settings(int width, int height, boolean fullscreen, boolean framerateCapped, boolean showFramerate, int framerate, double sfxVolume, double musVolume) {
		this.width = width ;
		this.height = height ;
		this.fullscreen = fullscreen ;
		this.sfxVolume = sfxVolume ;
		this.musVolume = musVolume ;
		this.framerateCapped = framerateCapped ;
		this.showFramerate = showFramerate ;
		this.framerate = framerate ;
		
		controller = Controller.getController() ;
		settings = this ;
	}
	
	//get the settings object
	public static Settings getSettings() {
		if (settings != null)
			return settings ;
		else //NOTE: TODO: Eventually this will have to read settings in from the file (game started not changing active settings)
			return new Settings(1920, 1080, true, false, true, 300, 1, 1) ;
		
	}

}
