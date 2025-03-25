using System.Collections.Generic;
using Godot;
using Godot.Collections;

public partial class PartDefinition : RigidBody3D
{
	[Export] public bool inEditor = true;

	// this is for parsing in the VAB/SPH(?)
	[Export] public Vector3 cameraRot;
	[Export] public Vector3 cameraPos;
	[Export] public float cameraSize;
	[Export] public string categoryName;
	[Export] public CompressedTexture2D categoryIcon;
	
	public PackedScene originalPrefab;

	public List<AttachNode> attachNodes = [];

	public CraftAssembler craftAssembler;

    public override void _Ready()
    {
        Array<Node> childNodes = GetChildren();
		foreach (Node child in childNodes)
		{
			if (child is AttachNode attachNode)
			{
				attachNodes.Add(attachNode);
			}
		}
    }

	// A majority of this is handled in the "CraftAssembler" class to keep things centralized I guess
	public void OnInputEvent(Camera3D cam, InputEvent @event, Vector3 eventPos, Vector3 normal, int shapeIDX)
    {
        if (@event is InputEventMouseButton mouse && mouse.Pressed && inEditor)
		{
			if (Input.IsMouseButtonPressed(MouseButton.Left))
			{
				float distance = cam.GlobalPosition.DistanceTo(GlobalPosition);
				craftAssembler.distanceToPart = distance;
				if (craftAssembler.currentlyHeldPart == this)
				{
					craftAssembler.currentlyHeldPart = null;
				}else{
					craftAssembler.currentlyHeldPart = this;
				}
			}
		}
    }
}
