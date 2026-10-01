using Godot;
using System;

public partial class Player : Area2D
{
	[Signal]
	public delegate void HitEventHandler();
	[Export]
	public int Speed {get; set; } = 400;
	private Vector2 _screenSize;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_screenSize = GetViewportRect().Size;
		GD.Print($"Ukuran layar terbaca: {_screenSize}");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var velocity = Vector2.Zero;

		if (Input.IsActionPressed("move_right")){
			velocity.X += 1;
		}
		if (Input.IsActionPressed("move_left")){
			velocity.X -= 1;
		}
		if (Input.IsActionPressed("move_down")){
			velocity.Y += 1;
		}
		if (Input.IsActionPressed("move_up")){
			velocity.Y -= 1;
		}
		var animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		if (velocity.Length()>0){
			velocity = velocity.Normalized()*Speed;
		} else
		{
			animatedSprite.Stop(); // terakhir
		}
		Position += velocity * (float)delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 54, _screenSize.X - 54),
			y: Mathf.Clamp(Position.Y, 68, _screenSize.Y - 68)
		);
		// Logika animasi dan membalik arah
		if (velocity.X != 0)
		{
			animatedSprite.Animation= "walk";
			animatedSprite.FlipV= false;
			animatedSprite.FlipH= velocity.X < 0; //kiri = true, kanan = false
		}
		else if (velocity.Y !=0)
		{
			animatedSprite.Animation= "up";
			animatedSprite.FlipV= false;
			animatedSprite.FlipH= velocity.Y >0; // bawah true, atas false
		}
	}
	private void OnBodyEntered(Node2D body)
	{
		Hide(); // nyelidekno player (gurita e)
		EmitSignal(SignalName.Hit); // kirim sinyal tabrakan
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}
	public void Start(Vector2 pos)
	{
		Position = pos;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}
}
