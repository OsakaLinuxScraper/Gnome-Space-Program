using System.Collections.Generic;
using Godot;
using Godot.Collections;

public partial class CraftAssembler : Node3D
{
	[Export] public Camera3D camera;
	[Export] public float partLerpSpeed;

	public PartDefinition currentlyHeldPart;
	public float distanceToPart;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (currentlyHeldPart!=null)
		{
			Vector2 mousePos = GetViewport().GetMousePosition();
			Vector3 projectedPosition = camera.ProjectPosition(mousePos, distanceToPart);

			Array<Node> childNodes = GetChildren();
			bool connectToPart = false;
			AttachNode attach0 = null;
			AttachNode attach1 = null;

			if (childNodes.Count > 1)
			{
				PartDefinition closestPart = null;
				float closestPartDist = float.PositiveInfinity;
				for (int i = 0; i < childNodes.Count; i++)
				{
					if (childNodes[i] is PartDefinition part)
					{
						float partDist = part.GlobalPosition.DistanceTo(currentlyHeldPart.GlobalPosition);
						if (partDist < closestPartDist && part != currentlyHeldPart)
						{
							closestPartDist = partDist;
							closestPart = part;
						}
					}
				}

				List<AttachNode> targetAttaches = closestPart.attachNodes;
				List<AttachNode> currentAttaches = currentlyHeldPart.attachNodes;

				(attach0, 
				attach1, 
				float attachDist) = GetClosestAttachNodes(targetAttaches, currentAttaches);

				//GD.Print(closestPart.GlobalPosition);

				Vector3 falseAttachPos = projectedPosition+attach1.Position;
				float trueDist = attach0.GlobalPosition.DistanceTo(falseAttachPos);

				if (trueDist < 1f)
				{
					connectToPart = true;
				}
			}

			if (connectToPart)
			{
				Vector3 connectPosition = attach0.GlobalPosition-attach1.Position;
				currentlyHeldPart.GlobalPosition = currentlyHeldPart.GlobalPosition.Lerp(connectPosition, partLerpSpeed*(float)delta);
			}else{
				currentlyHeldPart.GlobalPosition = currentlyHeldPart.GlobalPosition.Lerp(projectedPosition, partLerpSpeed*(float)delta);
			}
		}
	}

	private (AttachNode, AttachNode, float) GetClosestAttachNodes(List<AttachNode> list0, List<AttachNode> list1)
	{
		AttachNode winningNode0 = null;
		AttachNode winningNode1 = null;
		float winningDistance = float.PositiveInfinity;
		for (int x = 0; x < list0.Count; x++)
		{
			AttachNode node0 = list0[x];
			for (int y = 0; y < list1.Count; y++)
			{
				AttachNode node1 = list1[y];
				float distance = node0.GlobalPosition.DistanceTo(node1.GlobalPosition);
				if (distance < winningDistance && node0 != node1)
				{
					winningDistance = distance;
					winningNode0 = node0;
					winningNode1 = node1;
				}
			}
		}
		return (winningNode0, winningNode1, winningDistance);
	}
}
