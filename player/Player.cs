using Godot;
using System;

public partial class Player : Area2D
{
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
		if (velocity.Length()>0){
			velocity = velocity.Normalized()*Speed;
		}
		Position += velocity * (float)delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 54, _screenSize.X - 54),
			y: Mathf.Clamp(Position.Y, 68, _screenSize.Y - 68)
		);
	}
}
