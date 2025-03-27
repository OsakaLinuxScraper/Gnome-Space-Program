using Godot;
using System;

public partial class Engine : PartModule
{
    [Export] public float thrust;
    
    private PartDefinition rb3D;

    public override void _Ready()
    {
        rb3D = (PartDefinition)GetParent();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (enabled)
        {
            Vector3 direction = GlobalTransform.Basis.Y;

            Vector3 force = direction*thrust*rb3D.flightInfo.throttle;

            //GD.Print(rb3D.flightInfo.throttle);

            rb3D.ApplyImpulse(force, Position);
        }
    }
}
