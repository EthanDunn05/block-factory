using System.Linq;
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
	[Export] private float gravity;
	[Export] private float terminalVel;

	private bool mouseCaptured = false;

	private Vector2 moveAxis;
	private bool jumpHeld;
	private Vector2 lookDir = Vector2.Zero;

	private Aabb collision;
	private bool onFloor = true;
	private float cameraHeight;

	private Vector3 walkVel;
	private Vector3 gravVel;
	private Vector3 prevVel;

	private VoxelBoxMover boxMover = new();

	public override void _Ready()
	{
		player = GetParent<Player>();

		cameraHeight = Camera.Position.Y;

		collision = player.CollisionBox;

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

	public override void _Process(double delta)
	{
		var cameraPos = Camera.GlobalPosition;
		var goalPos = player.GlobalPosition + new Vector3(0f, cameraHeight, 0f);
		var newPos = cameraPos.Lerp(goalPos, (float) (delta * 15f));
		Camera.GlobalPosition = goalPos;
	}

	// Physics process gives stuttery movement and isn't good for first person player movement.
	public override void _PhysicsProcess(double delta)
	{
		if (!HasNode(player.Terrain.GetPath())) return;
		if (!player.TerrainTool.IsAreaEditable(new Aabb(collision.Position + player.Position, collision.Size))) return;
		
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
		player.GlobalTranslate(movement);

		if (gravVel.Y < 0f)
		{
			const float tolerance = 0.001f;
			if (Mathf.Abs(totalVel.Y) - Mathf.Abs(movement.Y) > tolerance)
			{
				onFloor = true;
			}
			else
			{
				onFloor = false;
			}
		}
		else
		{
			onFloor = false;
		}
		
		prevVel = movement;
	}
}
