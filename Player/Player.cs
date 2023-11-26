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
		Damaged,
		Victory

	}

	public State playerState;

	public int collectableCount = 0; // Integer to track the number of collectables acquired.

	[Export]
	public float bounceStrength = -200.0f,
				 Speed = 300.0f,
				 JumpVelocity = -400.0f;
	[Export]
	public int hitPoints;

	AnimationPlayer playerAnimations,
					uiAnimations;
	HBoxContainer healthUI;
	Label collectableUI;
	TextureRect heartContainer;
	AudioStreamPlayer playerSFX;
	Vector2 velocity,
		direction;
	
	public override void _Ready()
	{
		playerAnimations = (AnimationPlayer)GetNode("PlayerAnimations");
		uiAnimations = (AnimationPlayer)GetNode("HUD/UIAnimations");
		healthUI = (HBoxContainer)GetNode("HUD/HeartContainers");
		collectableUI = (Label)GetNode("HUD/Collectables/CountLabel");
		playerSFX = (AudioStreamPlayer)GetNode("PlayerSFX");
		hitPoints = Game.globalHitPoints;

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
		velocity.X = direction.X * Speed; /* This may be the cause of the elusive "flying" glitch I have so much trouble recreating. I'm wondering if this is overlapping with
										   * other velocity handling code and basically resulting in a divide by 0 glitch somewhere? Either way, simplifying velocity hanlding
										   * would make everything much cleaner and more stable.*/

		switch (playerState)
		{
			case State.Idle:
				playerAnimations.Play("Idle");

				velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);

				JumpCheck();
				RunCheck();
				FallCheck();

				// Debug.Print("Idle");

				break;

			case State.Run:
				playerAnimations.Play("Run");

				IdleCheck();
				JumpCheck();
				FallCheck();

				// Debug.Print("Run");

				break;

			case State.Jump:
				playerAnimations.Play("Jump");

				if (IsOnFloor())
				{
					velocity.Y = JumpVelocity;

				}

				FallCheck();

				// Debug.Print("Jump");

				break;

			case State.Bounce:
				playerAnimations.Play("Jump");

				if (IsOnFloor())
				{
					velocity.Y = bounceStrength;

				}

				FallCheck();

				// Debug.Print("Bounce");

				break;

			case State.Fall:
				playerAnimations.Play("Fall");

				IdleCheck();
				RunCheck();

				// Debug.Print("Fall");

				break;

			case State.Damaged:

				break;

			case State.Victory:

				break;

		}

		if (playerState != State.Damaged)
		{
			Velocity = velocity;

		}

		MoveAndSlide(); // Should automatically account for moving platforms according to documentation.

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

		if (Convert.ToInt32(collectableUI.Text) != collectableCount) // Checks to see if the collectableUI's text value no longer matches the number of collectables acquired.
		{
			playerSFX.PitchScale = (float)(Convert.ToInt32(collectableUI.Text) * .0125) + 1; /* Increases the pitch and speed of the collectable sound effect based on the number of collectables acquired collectableCount is
																							 * already 1 at this point. Converting UI text just before updating it prevents need for weird-looking extra math.*/

			collectableUI.Text = Convert.ToString(collectableCount); // Update collectable UI text to match the number of collectables acquired.

			playerSFX.Stream = (AudioStream)ResourceLoader.Load("res://collectables/collectable.mp3");
			playerSFX.Play();

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

		playerAnimations.Play("Damaged");

		playerSFX.PitchScale = 1;
		playerSFX.Stream = (AudioStream)ResourceLoader.Load("res://Dmg_Lo-Fi_002.mp3");
		playerSFX.Play();
		
		velocity.X = xVelocity;
		Velocity = velocity;
		
		await ToSignal(playerAnimations, "animation_finished");

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
	
	public async void Victory()
	{
		GetTree().ChangeSceneToFile("res://victory.tscn");

	}

	public void UICollectableCounterFlash()
	{
		uiAnimations.Play("CollectableCounterFlash");

	}

}





