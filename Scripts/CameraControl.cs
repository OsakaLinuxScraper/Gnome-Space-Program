using Godot;
using System;
using System.Diagnostics;

public partial class CameraControl : Camera3D
{
	[Export]
	public float movementSpeed = 0.1f;
	[Export] public Label speedUI;
	[Export] public Label posUI;
	[Export] public Label falsePosUI;
	[Export] public PackedScene doodad;
	public Vector3 linearMotion = Vector3.Zero;
	private bool camRotating = false;
	private Viewport vp;

	private UniverseManager universeManager;
	private Node3D localSpace;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		universeManager = (UniverseManager)GetTree().GetFirstNodeInGroup("UniverseManager");
		localSpace = (Node3D)GetTree().GetFirstNodeInGroup("LocalSpace");
		RenderingServer.SetDebugGenerateWireframes(true);
		vp = GetViewport();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//GD.Print(camRotating);
		//GD.Print(this.Position);
		//GD.Print(linearMotion);
		Translate(linearMotion);

		//GD.Print((Position - new Vector3(0,0,0)).Length());

		if (Input.IsKeyPressed(Key.W)){
			linearMotion.Z = -movementSpeed;
		}else if (Input.IsKeyPressed(Key.S)){
			linearMotion.Z = movementSpeed;
		}else{
			linearMotion.Z = 0;
		}

		if (Input.IsKeyPressed(Key.A)){
			linearMotion.X = -movementSpeed;
		}else if (Input.IsKeyPressed(Key.D)){
			linearMotion.X = movementSpeed;
		}else{
			linearMotion.X = 0;
		}

		if (Input.IsKeyPressed(Key.R)){
			linearMotion.Y = movementSpeed;
		}else if (Input.IsKeyPressed(Key.F)){
			linearMotion.Y = -movementSpeed;
		}else{
			linearMotion.Y = 0;
		}

		if (Input.IsKeyPressed(Key.E)){
			RotateObjectLocal(Vector3.Forward, 1f * (float)delta);
		}else if (Input.IsKeyPressed(Key.Q)){
			RotateObjectLocal(Vector3.Forward, -1f * (float)delta);
		}

		speedUI.Text = "Cam Speed: " + movementSpeed.ToString();
		posUI.Text = "True Position: " + GlobalPosition;//(" + GlobalPosition.X + " " + GlobalPosition.Y + " " + GlobalPosition.Z + ")";
		falsePosUI.Text = "False Position: " + (GlobalPosition - universeManager.offsetPosition);
	}

	// What the fuck is this shit??
    public override void _UnhandledInput(InputEvent @event)
    {
		if (@event is InputEventMouseButton buttonEvent && buttonEvent.Pressed)
		{
			if (Input.IsMouseButtonPressed(MouseButton.Right))
			{
				camRotating = true;
			}
		}else if (@event is InputEventMouseButton buttonEvent2 && buttonEvent2.Pressed == false){
			camRotating = false;
		}
		if (@event is InputEventMouseMotion motion && camRotating == true)
		{
			//GD.Print(motion.Relative);
			RotateObjectLocal(Vector3.Up, -motion.Relative.X/300);
			RotateObjectLocal(Vector3.Right, -motion.Relative.Y/300);
		}
		// Kinda stinky bug I zon't care because it's just debug nonsense anyways.
		if (Input.IsKeyLabelPressed(Key.P))
		{
			if (vp.DebugDraw == Viewport.DebugDrawEnum.Wireframe)
			{
				vp.DebugDraw = Viewport.DebugDrawEnum.Disabled;
			}else{
				vp.DebugDraw = Viewport.DebugDrawEnum.Wireframe;
			}
		}
		
		if (Input.IsKeyLabelPressed(Key.O))
		{
			//MeshInstance3D mesh = new();
			//mesh.Mesh = new BoxMesh();
			//GetTree().Root.GetChild(0).AddChild(mesh);
			//mesh.GlobalPosition = GlobalPosition;
			Node3D newDoodad = (Node3D)doodad.Instantiate();
			localSpace.AddChild(newDoodad);
			newDoodad.GlobalPosition = GlobalPosition;
		}

		if (Input.IsMouseButtonPressed(MouseButton.WheelUp))
		{
			movementSpeed *= 1.1f;
		}
		if (Input.IsMouseButtonPressed(MouseButton.WheelDown))
		{
			movementSpeed /= 1.1f;
		}
    }
}