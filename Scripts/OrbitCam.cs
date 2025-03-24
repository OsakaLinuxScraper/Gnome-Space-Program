using Godot;
using System;
using System.Diagnostics;

public partial class OrbitCam : Camera3D
{
	[Export] float lerpSpeed = 1.0f;
	[Export] float rotationAmnt = 1.0f;

	[Export] Node3D positionNode;
	[Export] Node3D rotNode_Y;
	[Export] Node3D rotNode_X;

	[Export] Vector3 posMove = new(0,1f,0);
	[Export] Vector3 outMove = new(0,0,1f);

	[Export] Vector3 posTarget;
	[Export] Vector3 outTarget;

	private Vector3 rotTarget_Y;
	private Vector3 rotTarget_X;

	private bool camRotating;

	private bool movingOut;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		float lerpy = lerpSpeed*(float)delta;

		positionNode.Position = positionNode.Position.Lerp(posTarget, lerpy);
		rotNode_Y.RotationDegrees = rotNode_Y.RotationDegrees.Lerp(rotTarget_Y, lerpy);
		rotNode_X.RotationDegrees = rotNode_X.RotationDegrees.Lerp(rotTarget_X, lerpy);
		Position = Position.Lerp(outTarget, lerpy);
	}

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton buttonEvent)
		{
			switch (buttonEvent.ButtonIndex)
			{
				case MouseButton.Right:
					camRotating = buttonEvent.Pressed;
					break;
				case MouseButton.WheelUp:
					if(!movingOut){posTarget += posMove;}else{outTarget -= outMove;}
					break;
				case MouseButton.WheelDown:
					if(!movingOut){posTarget -= posMove;}else{outTarget += outMove;}
					break;
			}
		}
		if (@event is InputEventMouseMotion motion && camRotating == true)
		{
			rotTarget_Y += Vector3.Up * -motion.Relative.X*rotationAmnt;
			rotTarget_X += Vector3.Right * -motion.Relative.Y*rotationAmnt;
		}
		if (@event is InputEventKey keyEvent)
		{
			switch (keyEvent.Keycode)
			{
				case Key.Shift:
					movingOut = keyEvent.Pressed;
					break;
			}
		}
    }
}
