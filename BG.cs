using Godot;
using System;
using System.Dynamic;

public partial class BG : ParallaxBackground
{
	[Export]
	public int scrollSpeed = 50;

	Vector2 scrollOffset;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		ScrollOffset = new Vector2((float)(ScrollOffset.X - (scrollSpeed * delta)), 0);

	}
	
}
