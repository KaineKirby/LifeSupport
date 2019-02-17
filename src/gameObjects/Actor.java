package gameObjects;

import org.newdawn.slick.Image;
import settings.Settings;

/*
 * Actor Class (Abstract)
 * 
 * This is an extension of the game object, except it is exclusive to things that move
 * (player, enemies)
 * 
 * moveSpeed updating is built into the class that can support screen scaling
 * a distinction must be made between x and y move speed because of scaling, update method built in to help with this
 */


public abstract class Actor extends GameObject {
	
	private Settings settings ;
	
	//move speed of actor before scaling
	public float moveSpeed ;
	
	//move speed of actor
	public float moveSpeedX ;
	public float moveSpeedY ;

	public Actor(float xPos, float yPos, int width, int height, int rotation, float moveSpeed, Image sprite) {
		super(xPos, yPos, width, height, rotation, sprite) ;
		
		this.settings = Settings.getSettings() ;
		
		//determining the properly scaled move speeds for the actor
		this.moveSpeed = moveSpeed ;
		this.moveSpeedX = (float)moveSpeed*settings.width/1920 ;
		this.moveSpeedY = (float)moveSpeed*settings.height/1080 ;
		
	}
	
	//because of scaling this will have to be updated with a method
	public void updateMoveSpeed(float speed) {
		this.moveSpeed = speed ;
		this.moveSpeedX = speed*settings.width/1920 ;
		this.moveSpeedY = speed*settings.height/1080 ;
	}

}
