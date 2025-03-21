using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

// TODO: make a "sphre of influence" system to keep the influencing planet at 0,0,0 (floating point imprecision bullshit)
// Somehow invert the position of every body so that the actual planet of influence remains stationary??? HOW TF DO I DO THAT?

public partial class UniverseManager : Node
{
	[Export] public bool running = true;
	[Export] public Node3D player;
	[Export] public float maxDistanceFromOrigin = 100;
	[Export] public float scaleDownFactor = 10.0f;
	//[Export] public float distanceFalloff = 10.0f;
	//[Export] public float scaleFalloff = 10.0f;

	[Export] public float moveForward = 1f;

	[Export] public Planet currentPlanet;
	
	public Vector3 offsetPosition;
	public List<ScaledObject> objectList = new();

	private Node3D localSpace;
	private Node3D scaledSpace;

	private Vector3 scaledPosition;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		localSpace = (Node3D)GetTree().GetFirstNodeInGroup("LocalSpace");
		scaledSpace = (Node3D)GetTree().GetFirstNodeInGroup("ScaledSpace");

		//scaledSpace.Scale = new Vector3(1/scaleDownFactor,1/scaleDownFactor,1/scaleDownFactor);


	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(running)
		{
			Vector3 playerPosition = player.GlobalPosition;
			float playerMagnitude = playerPosition.Length();

			//scaledSpace.GlobalPosition = playerPosition - offsetPosition;

			Godot.Collections.Array<Node> allScaledMembers = scaledSpace.GetChildren();

			Vector3 plrAdjustedPosition = player.GlobalPosition - offsetPosition;

			foreach (ScaledObject obj in objectList)
			{
				float magnitude = (obj.truePosition - plrAdjustedPosition).Length();

				//float falloffFactor = distanceFalloff * Mathf.Pow(magnitude, 0.5f);
				//float fallonFactor = Mathf.Pow(magnitude/distanceFalloff, 1/0.5f);

				//float distance = distanceFalloff * Mathf.Pow(magnitude, 0.5f); // make it decrease over dustabce to avoid blah blah blah
				Vector3 direction = obj.truePosition.DirectionTo(plrAdjustedPosition);

				float targetScale = 1/scaleDownFactor;//scaleFalloff/1000f * Mathf.Pow(magnitude, 0.5f)/scaleDownFactor;

				//GD.Print(distance + " " + targetScale);

				//float distToSurface = magnitude-600000;
				
				obj.associatedNode.Position = obj.truePosition+direction*(magnitude/(1+(moveForward/1000f))); // why the fuck is it 1.0101?
				obj.associatedNode.Scale = new Vector3(targetScale,targetScale,targetScale);
				//GD.Print(obj.associatedNode.GlobalPosition);
			}

			if (playerMagnitude > maxDistanceFromOrigin)
			{
                player.GlobalPosition -= playerPosition;
                //GlobalPosition -= playerPosition;

                // there is a chance for everything to fall apart over time but that ok
                OffsetChildren(localSpace, playerPosition);
               // OffsetChildren(scaledSpace, playerPosition);
			   	scaledSpace.GlobalPosition -= playerPosition;
	
			
				offsetPosition -= playerPosition;
			}
		}
	}

	public static void OffsetChildren(Node3D parentNode, Vector3 offset)
	{
		foreach (Node3D goon in parentNode.GetChildren().Cast<Node3D>())
		{
			goon.GlobalPosition -= offset;
		}
	}
}
