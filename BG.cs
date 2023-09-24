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
		scrollOffset = ScrollOffset;

		scrollOffset.X -= (float)(scrollSpeed * delta);

		ScrollOffset = scrollOffset;

	}
}
