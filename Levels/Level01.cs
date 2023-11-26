using Godot;
using System;
using System.Diagnostics;

public partial class Level01 : Stage
{
	public override void _Ready()
	{
		collectableTotal = 10;
		player = (Player)GetNode("./Player");
		
	}
	
	private void _on_goal_body_entered(Node2D body)
	{
		if (body == player)
		{
			if (player.collectableCount < collectableTotal)
			{
				player.UICollectableCounterFlash();

			}
			else
			{
				player.Victory();
				
			}

		}
		
	}
	
}










