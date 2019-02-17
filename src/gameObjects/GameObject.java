package gameObjects;

import org.newdawn.slick.Graphics;
import org.newdawn.slick.Image;

import settings.Settings;

/*
 * GameObject Class (Abstract)
 * 
 * The GameObject class will be the superclass from which all visuals objects in the game are derived
 * This can include the player, enemies, barriers, etc, anything drawn on screen (probably)
 * 
 * Anything that we plan to implement must use the gameobject as its parent, otherwise we can not use it nicely in the Main (or level)
 * 
 * For the width and height of the image representation, we assume the pixels at 1920x1080, and then scaling is taken into account for 16:9
 */

public abstract class GameObject {
	
	//positional information
	public float xPos ;
	public float yPos ;
	
	//width, height, rotation
	public int width ;
	public int height ;
	public int rotation ; //may need to be double if we go with radians
	
	//image to represent the sprite we use (maybe)
	//may need to switch to animation class later
	public Image sprite ;
	
	public GameObject(float xPos, float yPos, int width, int height, int rotation, Image sprite) {
		//for the width and height of the object as well as the position (in px) we need to scale it relative to the screen resolution
		Settings settings = Settings.getSettings() ;
		this.xPos = xPos*((float)settings.width/1920) ;
		this.yPos = yPos*((float)settings.height/1080) ;
		
		this.width = width*settings.width/1920 ;
		this.height = height*settings.height/1080 ;
		this.rotation = rotation ;
		
		this.sprite = sprite ;
		
	}
	
	//render the sprite with its current position (independent of update)
	//subclasses will likely have to call super(g) ;
	public void render(Graphics g) {
		sprite.draw(xPos, yPos, width, height) ;
	}
	
	//update the position of the object (this may be empty if the object is static, this is okay)
	public abstract void updatePosition(int delta) ;
	
}
