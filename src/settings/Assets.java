package settings ;

/*
 * Assets Class (Singleton)
 * 
 * This is the class where we get all game asset locations
 * If we centeralize them in this class then we can easily update the locations of everything just by changing this file
 */

public class Assets {
	
	public static Assets assets ;
	
	//all the assets (add as we go)
	public String gameTitle ;
	public String gameIcon ;
	public String playerIcon ;
	
	
	private Assets() {
		this.gameTitle = "LifeSupport" ;
		this.gameIcon = "res/icon/icon.png" ;
		
		this.playerIcon = "res/player/player.png" ;
		
		assets = this ;
	}
	
	public static Assets getAssets() {
		if (assets != null)
			return assets ;
		else
			return new Assets() ;
	}

}
