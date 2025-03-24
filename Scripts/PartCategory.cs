using Godot;
using System;

public partial class PartCategory : Button
{
	[Export] public TextureRect texture;
	[Export] public Control category;
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
		Control categoryParent = (Control)category.GetParent();
		foreach (Node child in categoryParent.GetChildren())
		{
			if (child is Control control)
			{
				control.Visible = false;
			}
		}
		category.Visible = true;
	}
}