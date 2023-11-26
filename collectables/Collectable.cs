using Godot;
using System;

public partial class Collectable : CharacterBody2D
{
	public Player player;

	public override void _Ready()
	{
		player = (Player)GetNode($"../../Player");

	}

	private void _on_area_2d_body_entered(Node2D body)
	{
		if (body == player)
		{
			player.collectableCount += 1;

			QueueFree();

		}
		
	}

}


