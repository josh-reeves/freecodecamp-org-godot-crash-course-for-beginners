using Godot;
using System;
using System.Diagnostics;

public partial class Frog : CharacterBody2D
{
	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	[Export]			 
	public float moveSpeed = 5;

	public CharacterBody2D player;
	public Vector2 playerDir;
	public bool chase;

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		if (!IsOnFloor())
		{
			velocity.Y += gravity * (float)delta;

		}

		if (chase)
		{
			if (playerDir.X > 0)
			{
				// Debug.Print("Right");

				((AnimatedSprite2D)GetNode("AnimatedSprite2D")).FlipH = true;

			}

			if (playerDir.X < 0)
			{
				// Debug.Print("Left");

				((AnimatedSprite2D)GetNode("AnimatedSprite2D")).FlipH = false;

			}

			velocity.X += (float)((playerDir.X * moveSpeed) * delta);

		}
		else
		{
			velocity.X = 0;

		}
		
		Velocity = velocity;
		MoveAndSlide();

	}
	
	private void _on_detection_radius_body_entered(Node2D body)
	{
		if (body.Name == "Player")
		{
			player = (CharacterBody2D)GetNode($"../../Player");

			playerDir = (player.Position - Position);
			chase = true;

			Debug.Print("Player");
			Debug.Print(Convert.ToString(player.GlobalPosition));

		}

	}
	
	private void _on_detection_radius_body_exited(Node2D body)
	{
		if (body.Name == "Player")
		{
			chase = false;

		}

	}
	
}






