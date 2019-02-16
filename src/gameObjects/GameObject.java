package gameObjects;

import org.newdawn.slick.Graphics;
import org.newdawn.slick.Image;

/*
 * GameObject Class (Abstract)
 * 
 * The GameObject class will be the superclass from which all visuals objects in the game are derived
 * This can include the player, enemies, barriers, etc, anything drawn on screen (probably)
 * 
 * Anything that we plan to implement must use the gameobject as its parent, otherwise we can not use it nicely in the Main (or level)
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
	
	//whether or not the object is static (static objects need to be checked for collision)
	public boolean isStatic ;
	
	public GameObject(float xPos, float yPos, int width, int height, int rotation, Image sprite, boolean isStatic) {
		this.xPos = xPos ;
		this.yPos = yPos ;
		
		this.width = width ;
		this.height = height ;
		this.rotation = rotation ;
		
		this.sprite = sprite ;
		
		this.isStatic = isStatic ;
	}
	
	//render the sprite with its current position (independent of update)
	//subclasses will likely have to call super(g) ;
	public void render(Graphics g) {
		//TODO: this is probably wrong, don't think I am understanding this correctly
		g.drawImage(sprite, xPos, yPos) ;
	}
	
	//update the position of the object (this may be empty if the object is static, this is okay)
	public abstract void updatePosition(int delta) ;
	
}
