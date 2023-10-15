using Godot;
using System;
using System.Diagnostics;
using System.Dynamic;
using System.Threading.Tasks;

public partial class Frog : CharacterBody2D
{
	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle(),
				 hitVelocity;
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
			playerDir = (player.Position - Position);

			if (playerDir.X > 5)
			{
				animatedSprite2D.FlipH = true;

				velocity.X = moveSpeed;

				// Debug.Print("Right");

			}

			if (playerDir.X < -5)
			{
				animatedSprite2D.FlipH = false;

				velocity.X = moveSpeed * -1;

				// Debug.Print("Left");

			}

			animatedSprite2D.Play("Jump");


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
		if (body == player)
		{
			// Debug.Print("Player");

			((AnimatedSprite2D)GetNode("AnimatedSprite2D")).Play("Death");

			player.playerState = Player.State.Bounce;
		
			await ToSignal((AnimatedSprite2D)GetNode("AnimatedSprite2D"), "animation_finished" );			

			QueueFree();

		}

	}

	private void _on_player_damage_body_entered(Node2D body)
	{
		if (playerDir.X > 0)
		{
			hitVelocity = 250;

		}
		else
		{
			hitVelocity = -250;			

		}

		if (body == player)
		{
			player.TakeDamage(hitVelocity, 1);

		}

	}
	
}



