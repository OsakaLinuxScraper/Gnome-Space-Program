using Godot;
using System;
using System.Threading.Tasks;

public partial class CraftManager : Node3D
{
	// Called when the node enters the scene tree for the first time.
	public Node3D launchLoc;
	public UniverseManager universeManager;
	public override void _Ready()
	{
		launchLoc = (Node3D)GetTree().GetFirstNodeInGroup("LaunchLocs");
		universeManager = (UniverseManager)GetTree().GetFirstNodeInGroup("UniverseManager");
		
		if (Globals.Launching)
		{
			Node3D craftContainer = new();
			AddChild(craftContainer);
			GD.Print(Globals.SavedCraft);
			Globals.Launching = false;
			Globals.SavedCraft.Instantiate(craftContainer);
			Globals.SavedCraft.CreateJoints(craftContainer);
			//Globals.SavedCraft.FreezeCraft(false);
			craftContainer.GlobalPosition = launchLoc.GlobalPosition;
			craftContainer.GlobalRotationDegrees = launchLoc.GlobalRotationDegrees;
			Globals.SavedCraft.FreezeCraft(false);
			Globals.SavedCraft.TogglePartModules(true);

			universeManager.player = Globals.SavedCraft.instancedParts[0];
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
