using Godot;
using System;

public partial class QuitGame : Button
{
	public void OnClick()
    {
        GD.Print("GET OUT");
		GetTree().Quit();
    }
}
