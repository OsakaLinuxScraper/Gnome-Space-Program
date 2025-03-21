using Godot;
using System;

public partial class GravityObject : RigidBody3D
{
    [Export] float gravConstant = 6.674e-11F; // 0.00000000006674

    private Planet currentPlanet;
    private UniverseManager universeManager;

    public override void _Ready()
    {
        universeManager = (UniverseManager)GetTree().GetFirstNodeInGroup("UniverseManager");
    }

    public override void _Process(double delta)
    {
        currentPlanet = universeManager.currentPlanet;

        Vector3 center = currentPlanet.GlobalPosition;
        Vector3 direction = GlobalPosition.DirectionTo(center);
        float distance = (center - GlobalPosition).Length();
        float planetMass = currentPlanet.mass;
        float objectMass = Mass;

        float force = gravConstant * planetMass * objectMass / Mathf.Pow(distance, 2);
        
        //GD.Print(force);
        ApplyForce(force*direction);
    }
}
