using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

// Update this later to use external files rather than hardcoded .tscn files..,.,,,. for mOdDiNg SuPpOrT... Ever heard of just releasing the source code?
// I don't need your damn modding support what am I supposed to do with these? Demand to see your game studio's manager! 
// Make your studio rue the day it thought it could give Sushut modding support, do you know who I am? 
// I'm the man that's gonna burn your game studio down! With the free and open source software!

public partial class PartSelectionParser : Panel
{
	[Export] public PackedScene selectorButton;
	[Export] public PackedScene categoryButton;

	[Export] public Container categoryContainer;
	[Export] public Container partListContainer;

	[Export] public CraftAssembler craftContainer;

	// All the things in this array need a PartDefinition attached.
	[Export] public PackedScene[] partList;

	private List<string> categories = [];
	private List<GridContainer> categoryContainers = [];

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		for (int i = 0; i < partList.Length; i++)
		{
			PackedScene partScene = partList[i];
			PartDefinition part = (PartDefinition)partScene.Instantiate();
			part.Freeze = true;
			
			PartSelector partButton = CreateSelectionButton(part, partScene);

			string categoryName = part.categoryName;
			if (!categories.Contains(categoryName))
			{
				GD.Print("New category: " + categoryName);
				categories.Add(categoryName);
				
				CreateCategory(part.categoryIcon).AddChild(partButton);
			}else{
				int index = categories.IndexOf(categoryName);
				categoryContainers[index].AddChild(partButton);
			}
		}
		categoryContainers[0].Visible = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	//public override void _Process(double delta)
	//{
	//}

	private PartSelector CreateSelectionButton(PartDefinition part, PackedScene partScene)
	{
		PartSelector partButton = (PartSelector)selectorButton.Instantiate();
		partButton.viewport.AddChild(part);

		Camera3D viewportCam = partButton.camera;
		viewportCam.Position = part.cameraPos;
		viewportCam.RotationDegrees = part.cameraRot;
		viewportCam.Size = part.cameraSize;

		partButton.partScene = partScene;
		partButton.craftContainer = craftContainer;

		return partButton;
	}

	private GridContainer CreateCategory(CompressedTexture2D catIco)
	{
		PartCategory catButton = (PartCategory)categoryButton.Instantiate();
		categoryContainer.AddChild(catButton);
		catButton.texture.Texture = catIco;

        GridContainer newContainer = new()
        {
            Visible = false
        };
		
		catButton.category = newContainer;

        partListContainer.AddChild(newContainer);

		categoryContainers.Add(newContainer);

		return newContainer;
	}
}
