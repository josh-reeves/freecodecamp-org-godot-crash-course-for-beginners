using Godot;
using System;

public partial class GameOver : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		Timer timer = (Timer)GetNode("Timer");

		await ToSignal(timer, "timeout");
		
		Game.globalHitPoints = 3;
		Game.SaveGame();

		GetTree().ChangeSceneToFile("res://main.tscn");

		
	}

}
