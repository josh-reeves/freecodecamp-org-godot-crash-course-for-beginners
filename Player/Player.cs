using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;


	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	AnimationPlayer playerAnim;

	public override void _Ready()
	{
		playerAnim = (AnimationPlayer)GetNode("AnimationPlayer");

	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity.Y += gravity * (float)delta;

		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;

			playerAnim.Play("Jump");

		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		if (direction == Vector2.Left)
		{
			((AnimatedSprite2D)GetNode("AnimatedSprite2D")).FlipH = true;

		}
		else if (direction == Vector2.Right)
		{
			((AnimatedSprite2D)GetNode("AnimatedSprite2D")).FlipH = false;

		}

		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;

			if (velocity.Y == 0)
			{
				playerAnim.Play("Run");

			}

		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);

			if (velocity.Y == 0)
			{
				playerAnim.Play("Idle");

			}

		}

		if (velocity.Y > 0)
		{
			playerAnim.Play("Fall");

		}

		Velocity = velocity;
		MoveAndSlide();
		
	}

}
