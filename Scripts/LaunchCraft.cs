using Godot;
using System;

public partial class LaunchCraft : Button
{
	[Export] public CraftAssembler craftAssembler;
	public void OnClick()
    {
        GD.Print("Launching Craft...");
        craftAssembler.craft.Instantiate(GetTree().Root.GetChild(0));
        craftAssembler.craft.CreateJoints(GetTree().Root.GetChild(0));
        craftAssembler.craft.FreezeCraft(false);
    }
}
