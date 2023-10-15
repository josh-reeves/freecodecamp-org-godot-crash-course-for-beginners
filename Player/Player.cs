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
		Fall,
		Damaged

	}

	public State playerState;

	[Export]
	public float bounceStrength = -200.0f,
				 Speed = 300.0f,
				 JumpVelocity = -400.0f;
	[Export]
	public int hitPoints = 3;

	AnimationPlayer animationPlayer;
	Vector2 velocity,
			direction;
	HBoxContainer healthUI = new HBoxContainer();
	TextureRect heartContainer;
	AudioStreamPlayer playerSFX;
	Game game = new Game();
	
	public override void _Ready()
	{
		animationPlayer = (AnimationPlayer)GetNode("AnimationPlayer");
		healthUI = (HBoxContainer)GetNode("CanvasLayer/HBoxContainer");
		playerSFX = (AudioStreamPlayer)GetNode("PlayerSFX");

	}

	public override void _PhysicsProcess(double delta)
	{
		velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity.Y += gravity * (float)delta;

		}

		// Get input direction. As good practice, you should replace UI actions with custom gameplay actions.
		direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		// Sprite transformation to handle direction.
		if (direction == Vector2.Left)
		{
			((AnimatedSprite2D)GetNode("AnimatedSprite2D")).FlipH = true;

		}
		else if (direction == Vector2.Right)
		{
			((AnimatedSprite2D)GetNode("AnimatedSprite2D")).FlipH = false;

		}

		// Basic movement handling.
		velocity.X = direction.X * Speed;

		switch (playerState)
		{
			case State.Idle:
				animationPlayer.Play("Idle");

				velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);

				JumpCheck();
				RunCheck();
				FallCheck();

				// Debug.Print("Idle");

				break;

			case State.Run:
				animationPlayer.Play("Run");

				IdleCheck();
				JumpCheck();
				FallCheck();

				// Debug.Print("Run");

				break;

			case State.Jump:
				animationPlayer.Play("Jump");

				if (IsOnFloor())
				{
					velocity.Y = JumpVelocity;

				}

				FallCheck();

				// Debug.Print("Jump");

				break;

			case State.Bounce:
				animationPlayer.Play("Jump");

				if (IsOnFloor())
				{
					velocity.Y = bounceStrength;

				}

				FallCheck();

				// Debug.Print("Bounce");

				break;

			case State.Fall:
				animationPlayer.Play("Fall");

				IdleCheck();
				RunCheck();

				// Debug.Print("Fall");

				break;

			case State.Damaged:

				break;

		}

		if (playerState != State.Damaged)
		{
			Velocity = velocity;

		}

		MoveAndSlide();

		if (healthUI.GetChildCount() < hitPoints)
		{
			heartContainer = new TextureRect();
			heartContainer.Texture = (Texture2D)ResourceLoader.Load("res://Heart_003/Heart_003_64px.png");

			healthUI.AddChild(heartContainer);

			// Debug.Print($"Hit Points: {Convert.ToString(hitPoints)}\n" +
			//			   $"Children: {Convert.ToString(healthUI.GetChildCount())}");

		}
		else if (healthUI.GetChildCount() > hitPoints)
		{
			healthUI.GetChild(healthUI.GetChildCount() - 1).QueueFree();

		}
		
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

	public async void TakeDamage(float xVelocity, int damage)
	{
		playerState = State.Damaged;

		animationPlayer.Play("Damaged");

		playerSFX.Stream = (AudioStream)ResourceLoader.Load("res://Dmg_Lo-Fi_002.mp3");
		playerSFX.Play();
		
		velocity.X = xVelocity;
		Velocity = velocity;
		
		await ToSignal(animationPlayer, "animation_finished");

		hitPoints -= damage;
		if (hitPoints <= 0)
		{
			Death();

		}

		IdleCheck();
		RunCheck();
		JumpCheck();
		FallCheck();

	}

	public void Death()
	{
		QueueFree();
		GetTree().ChangeSceneToFile("res://game-over.tscn");

	}
	
	private void _on_visible_on_screen_notifier_2d_screen_exited()
	{
		Death();

	}

}





