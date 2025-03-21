using Godot;

public partial class CameraMouseMovement : Node3D
{
	[Export] public bool enabled = true;
	[Export] public float moveAmnt = 5f;

	[Export] public string upDir = "Z";
	[Export] public string rightDir = "X";

	private Vector3 originalRotation;

	private Vector3 up;
	private Vector3 right;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		originalRotation = GlobalRotationDegrees;

        up = upDir switch
        {
            "X" => Transform.Basis.X,
            "-X" => -Transform.Basis.X,
            "Y" => Transform.Basis.Y,
            "-Y" => -Transform.Basis.Y,
            "Z" => Transform.Basis.Z,
            "-Z" => -Transform.Basis.Z,
            _ => Transform.Basis.Z,
        };

		right = rightDir switch
        {
            "X" => Transform.Basis.X,
            "-X" => -Transform.Basis.X,
            "Y" => Transform.Basis.Y,
            "-Y" => -Transform.Basis.Y,
            "Z" => Transform.Basis.Z,
            "-Z" => -Transform.Basis.Z,
            _ => Transform.Basis.Z,
        };
        //up = Transform.Basis.Z;
		//right = Transform.Basis.X;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (enabled)
		{
			Vector2 mousePos = (GetViewport().GetMousePosition() / GetViewport().GetVisibleRect().Size - new Vector2(0.5f,0.5f))*moveAmnt;

			Vector3 upMovement = up*mousePos.Y;
			Vector3 rightMovement = right*mousePos.X;

			Vector3 newRot = originalRotation-upMovement-rightMovement;

			GlobalRotationDegrees = GlobalRotationDegrees.Lerp(newRot, 1f*(float)delta);
		}
	}
}
