package player;

import org.newdawn.slick.Image;
import org.newdawn.slick.SlickException;

import gameObjects.Actor;
import settings.Assets;
import settings.Controller;

/*
 * Player Class (Singleton)
 * This is the class that represents the player
 */

public class Player extends Actor {
	
	public static Player player ;
	
	//input controller
	Controller controller ;

	private Player(float xPos, float yPos, int width, int height, int rotation, Image sprite, boolean isStatic) {
		super(xPos, yPos, width, height, rotation, .3f, sprite) ;
		
		//not sure why this needs the height of the screen
		this.controller = Controller.getController() ;
	
		player = this ;
		
	}

	@Override
	public void updatePosition(int delta) {
		
		//for the player we update their position based on keyboard presses
		//TODO we need to move slower on diagonals, need to think about the math
		
		//the time delta and the scale constants are included
		//scale constants for letting the screen scale and the delta for different framerates
		if (controller.isKeyDown(controller.moveUp) && controller.isKeyDown(controller.moveRight)) {
			this.xPos = this.xPos + moveSpeedX*delta ;
			this.yPos = this.yPos - moveSpeedY*delta ;
		}
		else if (controller.isKeyDown(controller.moveUp) && controller.isKeyDown(controller.moveLeft)) {
			this.xPos = this.xPos - moveSpeedX*delta ;
			this.yPos = this.yPos - moveSpeedY*delta ;
		}
		else if (controller.isKeyDown(controller.moveDown) && controller.isKeyDown(controller.moveLeft)) {
			this.xPos = this.xPos - moveSpeedX*delta ;
			this.yPos = this.yPos + moveSpeedY*delta ;
		}
		else if (controller.isKeyDown(controller.moveDown) && controller.isKeyDown(controller.moveRight)) {
			this.xPos = this.xPos + moveSpeedX*delta ;
			this.yPos = this.yPos + moveSpeedY*delta ;
		}
		else if (controller.isKeyDown(controller.moveUp))
			this.yPos = this.yPos - moveSpeedY*delta ;
		else if (controller.isKeyDown(controller.moveDown))
			this.yPos = this.yPos + moveSpeedY*delta ;
		else if (controller.isKeyDown(controller.moveLeft))
			this.xPos = this.xPos - moveSpeedX*delta ;
		else if (controller.isKeyDown(controller.moveRight))
			this.xPos = this.xPos + moveSpeedX*delta ;
		
	}
	
	public static Player getPlayer() throws SlickException {
		if (player != null)
			return player ;
		else
			return new Player(100, 100, 32, 32, 0, new Image(Assets.getAssets().playerIcon), false) ;
	}

}
