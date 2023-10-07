using Godot;
using System;
using System.Diagnostics;
using System.Dynamic;
using System.Threading.Tasks;

public partial class Frog : CharacterBody2D
{
	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	public bool chase;

	[Export]			 
	public float moveSpeed = 5;

	public Player player;
	public Vector2 playerDir;
	public AnimatedSprite2D animatedSprite2D;

	public override void _Ready()
	{
		player = (Player)GetNode($"../../Player");
		animatedSprite2D = (AnimatedSprite2D)GetNode("AnimatedSprite2D");

	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		if (!IsOnFloor())
		{
			velocity.Y += gravity * (float)delta;

		}

		if (chase && animatedSprite2D.Animation != "Death")
		{
			if (playerDir.X > 0)
			{
				// Debug.Print("Right");

				animatedSprite2D.FlipH = true;

			}

			if (playerDir.X < 0)
			{
				// Debug.Print("Left");

				animatedSprite2D.FlipH = false;

			}

			animatedSprite2D.Play("Jump");

			velocity.X = (playerDir.X * moveSpeed) * (float)delta;

		}
		else if (animatedSprite2D.Animation != "Death")
		{
			animatedSprite2D.Play("Idle");

			velocity.X = 0;

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
			playerDir = (player.Position - Position);
			chase = true;

			// Debug.Print("Player");
			// Debug.Print(Convert.ToString(player.GlobalPosition));

		}

	}
	
	private void _on_detection_radius_body_exited(Node2D body)
	{
		if (body.Name == "Player")
		{
			chase = false;

		}

	}
	
	public async void _on_hitbox_body_entered(Node2D body)
	{
		if (body.Name == "Player")
		{
			// Debug.Print("Player");

			((AnimatedSprite2D)GetNode("AnimatedSprite2D")).Play("Death");

			player.playerState = Player.State.Bounce;
		
			await ToSignal((AnimatedSprite2D)GetNode("AnimatedSprite2D"), "animation_finished" );			

			QueueFree();

		}

	}
	
}
