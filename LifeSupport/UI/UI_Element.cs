using System;

abstract class UI_Element{

    //Screen Position
    public float XPos;
    public float YPos;
    public float scale;

	public UI_Element(float XPos, float YPos, Game game) {
        this.XPos = XPos;
        this.YPos = YPos;
        this.scale = scale;
	}


}
