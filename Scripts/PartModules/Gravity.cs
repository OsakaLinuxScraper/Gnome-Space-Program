using Godot;
using System;

public partial class Gravity : PartModule
{
    const float gravConstant = 6.674e-11F; // 0.00000000006674

    private Planet currentPlanet;
    private UniverseManager universeManager;
    
    private RigidBody3D rb3D;

    public override void _Ready()
    {
        universeManager = (UniverseManager)GetTree().GetFirstNodeInGroup("UniverseManager");
        rb3D = (RigidBody3D)GetParent();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (enabled)
        {
            currentPlanet = universeManager.currentPlanet;

            Vector3 center = currentPlanet.GlobalPosition;
            Vector3 direction = rb3D.GlobalPosition.DirectionTo(center);
            float distance = (center - rb3D.GlobalPosition).Length();
            float planetMass = currentPlanet.mass;
            float objectMass = rb3D.Mass;

            float force = gravConstant * planetMass * objectMass / Mathf.Pow(distance, 2);
            
            //GD.Print(force);
            rb3D.ApplyForce(force*direction);
        }
    }
}
