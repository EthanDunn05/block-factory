using BlockFactory.scripts.block_library;
using Godot;

namespace BlockFactory.scripts.player;

public partial class PlayerMovement : Node
{
	private Player player;

	[Export] public Camera3D Camera;
	[Export] private Aabb collision;

	[Export] private float moveSpeed;
	[Export] private float accel;
	[Export] private float jumpForce;
	[Export] private float gravity;
	[Export] private float terminalVel;

	private bool mouseCaptured = false;

	private Vector2 moveAxis;
	private bool jumpHeld;
	private Vector2 lookDir = Vector2.Zero;

	private const float SpeedMult = 0.001f;
	private Vector3 walkVel;
	private Vector3 gravVel;
	private Vector3 prevVel;

	private VoxelBoxMover boxMover = new();

	public override void _Ready()
	{
		player = GetParent<Player>();

		Input.SetMouseMode(Input.MouseModeEnum.Captured);
		mouseCaptured = true;
		
		boxMover.SetMaxStepHeight(1f); // Full block step height :)
		boxMover.SetStepClimbingEnabled(true);
		boxMover.SetCollisionMask(1);
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

	// Physics process gives stuttery movement and isn't good for first person player movement.
	public override void _Process(double delta)
	{
		if (!HasNode(player.Terrain.GetPath())) return;
		if (!player.TerrainTool.IsAreaEditable(new Aabb(collision.Position + player.Position, collision.Size))) return;
		
		var onFloor = CheckOnFloor();
		
		var forward = Camera.GlobalTransform.Basis * new Vector3(moveAxis.X, 0f, moveAxis.Y);
		var walkDir = new Vector3(forward.X, 0f, forward.Z).Normalized();

		walkVel = walkVel.MoveToward(walkDir * moveSpeed, accel * (float) delta);

		if (onFloor)
		{
			gravVel = gravity * Vector3.Down * (float) delta;

			if (jumpHeld)
			{
				gravVel = Vector3.Up * jumpForce;
			}
		}
		else
		{
			gravVel = gravVel.MoveToward(terminalVel * Vector3.Down, gravity * (float) delta);
		}

		var totalVel = (walkVel + gravVel) * (float) delta;
		var movement = boxMover.GetMotion(player.GlobalPosition, totalVel, collision, player.Terrain);
		prevVel = movement;
		player.GlobalTranslate(movement);
	}

	private bool CheckOnFloor()
	{
		// Yes this lags behind by a frame, but I don't care
		if (prevVel.Y > -0.001f && prevVel.Y < 0.001f)
		{
			var colRect = new Rect2(collision.Position.X, collision.Position.Z,collision.Size.X, collision.Size.Y);

			// Raycast the 4 corners of the collision
			if (CastFloor(colRect.Position)) return true;
			if (CastFloor(colRect.Position + colRect.Size * Vector2.Right)) return true;
			if (CastFloor(colRect.Position + colRect.Size * Vector2.Down)) return true;
			if (CastFloor(colRect.Position + colRect.Size * Vector2.One)) return true;
		}

		return false;
	}

	private bool CastFloor(Vector2 offset)
	{
		var cast = player.TerrainTool.Raycast(player.GlobalPosition + new Vector3(offset.X, 0f, offset.Y), Vector3.Down, 0.1f);
		return cast != null && cast.Distance < 0.01f;
	}
}
