import org.newdawn.slick.AppGameContainer;
import org.newdawn.slick.BasicGame;
import org.newdawn.slick.GameContainer;
import org.newdawn.slick.Graphics;
import org.newdawn.slick.Image;
import org.newdawn.slick.SlickException;

public class HelloWorld extends BasicGame {
	
	private Image example ;
	private int x ;
	private int y ;
	private String str ;

	public HelloWorld(String title) {
		super(title) ;
	}

	@Override
	public void init(GameContainer cont) throws SlickException {
		example = new Image("res/example.png") ;
		x = -300 ;
		y = 100 ;
		
		str = "" ;
		
	}
	
	@Override
	public void render(GameContainer cont, Graphics g) throws SlickException {
		example.draw(x, y) ;
		g.drawString(str, 370, 500) ;
		
	}

	@Override
	public void update(GameContainer cont, int delta) throws SlickException {
		x += delta ;
		if (x > 801)
			x = -300 ;
		
		str = x+"" ;
		
	}
	
	public static void main(String[] args) throws SlickException {
		AppGameContainer app = new AppGameContainer(new HelloWorld("Atici")) ;
		
		app.setDisplayMode(800, 600, false) ;
		app.start() ;
	}

}
