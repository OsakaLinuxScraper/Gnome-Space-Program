using Godot;
using System;

public partial class PartSelector : Button
{
	[Export] public PackedScene partScene;
	[Export] public CraftAssembler craftContainer;
	[Export] public Camera3D camera;
	[Export] public SubViewport viewport;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnClick()
	{
		if (craftContainer.currentlyHeldPart == null)
		{
			PartDefinition part = (PartDefinition)partScene.Instantiate();
			part.craftAssembler = craftContainer;
			part.Freeze = true;
			part.originalPrefab = partScene;
			craftContainer.AddChild(part);
			craftContainer.currentlyHeldPart = part;
			craftContainer.distanceToPart = craftContainer.camera.GlobalPosition.DistanceTo(craftContainer.camera.posTarget);
		}else{
			craftContainer.currentlyHeldPart.QueueFree();
			craftContainer.currentlyHeldPart = null;
		}
	}
}
