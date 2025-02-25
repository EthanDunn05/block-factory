using BlockFactory.scripts.block_library;
using Godot;

namespace BlockFactory.scripts.player;

public partial class PlayerMovement : Node
{
	private Player player;

	[Export] public Camera3D Camera;

	[Export] private float moveSpeed;
	[Export] private float accel;
	[Export] private float jumpForce;

	private bool mouseCaptured = false;

	private Vector2 moveAxis;
	private bool jumpHeld;
	private Vector2 lookDir = Vector2.Zero;

	private Vector3 moveVel;
	private Vector3 gravVel;
	private Vector3 jumpVel;

	private VoxelBoxMover boxMover = new();

	public override void _Ready()
	{
		player = GetParent<Player>();

		Input.SetMouseMode(Input.MouseModeEnum.Captured);
		mouseCaptured = true;
	}

	public override void _UnhandledInput(InputEvent evt)
	{
		moveAxis = Input.GetVector("left", "right", "forward", "back");
		jumpHeld = Input.IsActionPressed("jump");

		if (Input.IsActionJustPressed("ui_cancel"))
		{
			if (mouseCaptured) Input.SetMouseMode(Input.MouseModeEnum.Visible);
			else Input.SetMouseMode(Input.MouseModeEnum.Captured);

			mouseCaptured = !mouseCaptured;
		}

		if (evt is InputEventMouseMotion mouseEvent && mouseCaptured)
		{
			lookDir = mouseEvent.Relative * 0.001f;

			var cameraRotation = Camera.Rotation;
			cameraRotation.Y -= lookDir.X;
			cameraRotation.X = Mathf.Clamp(cameraRotation.X - lookDir.Y, -Mathf.Pi / 2f, Mathf.Pi / 2f);
			Camera.Rotation = cameraRotation;
		}
	}

	public override void _Process(double delta)
	{
		// TODO: Change this to VoxelBoxMover
		
		var forward = Camera.GlobalTransform.Basis * new Vector3(moveAxis.X, 0f, moveAxis.Y);
		var walkDir = new Vector3(forward.X, 0f, forward.Z).Normalized();

		moveVel = moveVel.MoveToward(walkDir * moveSpeed * moveAxis.Length(), (float) (delta * accel));

		if (!player.IsOnFloor())
		{
			gravVel = gravVel.MoveToward(gravVel + player.GetGravity(), (float) (player.GetGravity().Length() * delta));
			jumpVel = jumpVel.MoveToward(Vector3.Zero, (float) (player.GetGravity().Length() * delta));
		}
		else
		{
			gravVel = Vector3.Zero;
			jumpVel = Vector3.Zero;

			if (jumpHeld)
			{
				jumpVel = -player.GetGravity().Normalized() * jumpForce;
			}
		}

		player.Velocity = moveVel + gravVel + jumpVel;

		// Check for stepping up block
		if (player.IsOnFloor())
		{
			var raycastOrigin = new Vector3(
				player.GlobalPosition.X, 
				player.GlobalPosition.Y + 0.5f, // player origin is at their feet, so go up half a block
				player.GlobalPosition.Z
			);
			var raycastDir = new Vector3(player.Velocity.X, 0f, player.Velocity.Z).Normalized();
			var raycastLen = (new Vector3(player.Velocity.X, 0f, player.Velocity.Z) * (float) delta).Length() + 0.5f;
			
			// Check block at feet and none at head
			var footRaycast = player.VoxelTool.Raycast(raycastOrigin, raycastDir, raycastLen);
			var headRaycast = player.VoxelTool.Raycast(raycastOrigin + Vector3.Up, raycastDir, raycastLen);
			
			if (footRaycast != null && headRaycast == null)
			{
				player.Position += Vector3.Up;
			}
		}

		player.MoveAndSlide();
	}
}
