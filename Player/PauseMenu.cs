using Godot;
using System;

public partial class PauseMenu : Panel
{
	Player player;

	public override void _Ready()
	{
		player = (Player)GetNode("../../.");


	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("pause") && GetTree().Paused == false)
		{
			Visible = true;

			GetTree().Paused = true;

		}
		else if (Input.IsActionJustPressed("pause") && GetTree().Paused == true)
		{
			Visible = false;

			GetTree().Paused = false;

		}

	}
	
	private void _on_btn_continue_pressed()
	{
		if (GetTree().Paused == true)
		{
			Visible = false;

			GetTree().Paused = false;

		}

	}

	private void _on_btn_save_pressed()
	{
		Game.globalHitPoints = player.hitPoints;
		Game.SaveGame();

		GetTree().Quit();

	}

}



