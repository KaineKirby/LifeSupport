package random ;

import java.util.Random ;

/*
 * RandomGenerator Class (Singleton)
 * 
 * This is the class that we use for RandomNumber generation across the whole game.
 * 
 * In addition to having some nicer built in methods with this, we can eventually introduce new RNG algorithms that have the ability to seed
 * meaning that we can introduce the concept of game seeds.
 * 
 * It must be an object so we can control the seed, otherwise we could just make it a static class. In its current implementation it could be,
 * but just future-proofing.
 * 
 * NOTE: TODO: This is a very basic class for now, just calling upon the java built in random number generation, we can change this later
 */

public class RandomGenerator {
	
	public static RandomGenerator rand ;
	
	private RandomGenerator() {
		rand = this ;
	}
	
	public static RandomGenerator getRandomGenerator() {
		if (rand != null)
			return rand ;
		else
			return new RandomGenerator() ;
	}
	
	//we can introduce new methods as we need them
	public int getRandomIntRange(int min, int max) {
	    Random javaRand = new Random() ;

	    // nextInt is normally exclusive of the top value,
	    // so add 1 to make it inclusive
	    int randomNum = javaRand.nextInt((max - min) + 1) + min ;

	    return randomNum ;
	}
		
}
