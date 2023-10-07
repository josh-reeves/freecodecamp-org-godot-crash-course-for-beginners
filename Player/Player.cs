using Godot;
using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

public partial class Player : CharacterBody2D
{
	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public enum State
	{
		Idle,
		Run,
		Jump,
		Bounce,
		Fall

	}

	public State playerState;

	[Export]
	public float bounceStrength = -200.0f,
				 Speed = 300.0f,
				 JumpVelocity = -400.0f;

	AnimationPlayer playerAnim;
	Vector2 velocity,
			direction;

	public override void _Ready()
	{
		playerAnim = (AnimationPlayer)GetNode("AnimationPlayer");

	}

	public override void _PhysicsProcess(double delta)
	{
		// Get the input direction.
		// As good practice, you should replace UI actions with custom gameplay actions.
		direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		velocity = Velocity;

		if (direction == Vector2.Left)
		{
			((AnimatedSprite2D)GetNode("AnimatedSprite2D")).FlipH = true;

		}
		else if (direction == Vector2.Right)
		{
			((AnimatedSprite2D)GetNode("AnimatedSprite2D")).FlipH = false;

		}

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity.Y += gravity * (float)delta;

		}

		velocity.X = direction.X * Speed;

		switch (playerState)
		{
			case State.Idle:
				playerAnim.Play("Idle");

				velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);

				JumpCheck();
				RunCheck();

				// Debug.Print("Idle");

				break;

			case State.Run:
				playerAnim.Play("Run");

				IdleCheck();
				JumpCheck();

				// Debug.Print("Run");

				break;

			case State.Jump:
				playerAnim.Play("Jump");

				if (IsOnFloor())
				{
					velocity.Y = JumpVelocity;

				}

				FallCheck();

				// Debug.Print("Jump");

				break;

			case State.Bounce:
				playerAnim.Play("Jump");

				if (IsOnFloor())
				{
					velocity.Y = bounceStrength;

				}

				FallCheck();

				// Debug.Print("Bounce");

				break;

			case State.Fall:
				playerAnim.Play("Fall");

				IdleCheck();
				RunCheck();

				// Debug.Print("Fall");

				break;

		}

		Velocity = velocity;
		MoveAndSlide();
		
	}

	public void IdleCheck()
	{
		if (direction == Vector2.Zero && IsOnFloor())
		{
			playerState = State.Idle;

		}
		
	}

	public void RunCheck()
	{
		if (direction != Vector2.Zero && velocity.Y == 0)
		{
			playerState = State.Run;

		}

	}

	public void JumpCheck()
	{
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			playerState = State.Jump;

		}

	}

	public void FallCheck()
	{
		if (velocity.Y > 0)
		{
			playerState = State.Fall;

		}

	}

}
