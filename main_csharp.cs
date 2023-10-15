using Godot;
using System;

public partial class main_csharp : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Game.LoadGame();

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
				
	}

	private void _on_play_pressed()
	{
		GetTree().ChangeSceneToFile("res://world.tscn");
		
	}
	
	private void _on_quit_pressed()
	{
		GetTree().Quit();
		
	}
	
	
}
