package levels ;

import org.newdawn.slick.Graphics;

import gameObjects.GameObject;

/*
 * Room Class
 * 
 * Each room has (up to) 4 doors that connect to other rooms
 * Also has an array of GameObjects that represent the spawn points of enemies, barriers, walls, etc
 * Each room must have barriers around the perimeter preventing the player/enemies from leaving game area
 * 
 * These will likely be built by another class and connected together by that class
 */

public class Room {
	
	//all four doors
	public Room door1 ;
	public Room door2 ;
	public Room door3 ;
	public Room door4 ;
	
	//game object array
	public GameObject[] objects ; //may want to switch this to an arraylist for unboundedness
	
	//if the room has been beat by the player or not
	public boolean isComplete ;
	
	public Room(GameObject[] objects) {	
		this.objects = objects ;
	}
	
	public void updateObjectPositions(int delta) {
		//update the positions of all game objects
		for (int i = 0 ; i < objects.length ; i++) {
			if (objects[i] != null)
				objects[i].updatePosition(delta) ;
		}
	}
	
	public void renderObjects(Graphics g) {
		//render the objects based on their current positions
		for (int i = 0 ; i < objects.length ; i++) {
			if (objects[i] != null)
				objects[i].render(g) ;
		}
	}
}
