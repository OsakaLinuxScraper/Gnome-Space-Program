using Godot;
using System;

public partial class SceneSwitcher : Node
{
    [Export] string scenePath;
    public void OnClick()
    {
        GD.Print("Scene Switcher Clicked...");
        GetTree().ChangeSceneToFile(scenePath);
    }
}
