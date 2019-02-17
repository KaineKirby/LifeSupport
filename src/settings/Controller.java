package settings ;

import org.newdawn.slick.Input;

/*
 * Controller Class (Singleton)
 * 
 * Much like settings, this is a central access point for controls.
 * 
 * NOTE: At the moment this is hardcoded, but should be updated to load the controls from the file
 */

public class Controller {
	
	public static Controller controller ;
	
	private Input input ;
	
	//all game controls and their binds
	public int moveUp ;
	public int moveDown ;
	public int moveLeft ;
	public int moveRight ;
	
	
	private Controller() {
		controller = this ;
		
		input = new Input(Settings.getSettings().height) ;
		
		//TODO these are hardcoded for now, they should be read from settings file later
		this.moveUp = Input.KEY_W ;
		this.moveDown = Input.KEY_S ;
		this.moveLeft = Input.KEY_A ;
		this.moveRight = Input.KEY_D ;
		
	}
	
	public static Controller getController() {
		if (controller != null)
			return controller ;
		else 
			return new Controller() ;
	}
	
	public boolean isKeyDown(int key) {
		if (input.isKeyDown(key))
			return true ;
		else
			return false ;
	}

}
