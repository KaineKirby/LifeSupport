import org.newdawn.slick.AppGameContainer;
import org.newdawn.slick.BasicGame;
import org.newdawn.slick.GameContainer;
import org.newdawn.slick.Graphics;
import org.newdawn.slick.SlickException;

import gameObjects.GameObject;
import levels.Room;
import player.Player;
import settings.Assets;
import settings.Settings;

/*
 * Main Class
 * 
 * This is the class that runs the game and updates all the visuals from the lower level classes
 * 
 * comments explain each of the methods
 */

public class Main extends BasicGame {
	
	//this is a temporary 1 room for testing
	private Room testRoom ;

	public Main(String title) {
		super(title) ;
	}
	
	//this method is called when the game first opens, everything that has to do with the loading of the initial game should take place here
	//will likely just be the main menu being put into place
	@Override
	public void init(GameContainer cont) throws SlickException {
		GameObject[] testObjects = new GameObject[1] ;
		testObjects[0] = Player.getPlayer() ;
		testRoom = new Room(testObjects) ;
		
	}
	
	//this method is where the drawing of sprites should take place
	//generally, we will redraw graphics here based on the updated positions from the update method
	//most of this process will be abstracted away to lower classes however
	@Override
	public void render(GameContainer cont, Graphics g) throws SlickException {
		testRoom.renderObjects(g) ;
	}
	
	//this is the update method that is called every frame, this is where you do movement
	//the delta int passed is the time between the last frame (this is how we untie physics from framerate)
	//we may need to pass the delta to lower classes so they can update their positions properly
	@Override
	public void update(GameContainer cont, int delta) throws SlickException {
		testRoom.updateObjectPositions(delta) ; 
	}
	
	public static void main(String[] args) throws SlickException {
		//declare the app game container
		AppGameContainer game = new AppGameContainer(new Main(Assets.getAssets().gameTitle)) ;
		
		//set the app icon
		game.setIcon(Assets.getAssets().gameIcon) ;
		
		//set the video settings (resolution and fullscreen)
		game.setDisplayMode(Settings.getSettings().width, Settings.getSettings().height, Settings.getSettings().fullscreen) ;
		
		//set the target framerate cap (if it is capped)
		if (Settings.getSettings().framerateCapped) 
			game.setTargetFrameRate(Settings.getSettings().framerate) ;
		
		//set whether or not we are showing fps in top left
		game.setShowFPS(Settings.getSettings().showFramerate) ;
		
		//open the window
		game.start() ;

	}

}
