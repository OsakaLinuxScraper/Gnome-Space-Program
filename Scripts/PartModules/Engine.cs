using Godot;
using System;
using System.Collections.Generic;

public partial class Engine : PartModule
{
    [Export] public float thrust;
    
    private PartDefinition rb3D;

    public List<GpuParticles3D> particles = [];
    public List<Light3D> lights = [];

    public override void _Ready()
    {
        rb3D = (PartDefinition)GetParent();

        foreach (Node child in GetChildren())
        {
            if (child is GpuParticles3D particle)
            {
                particles.Add(particle);
            }else if (child is Light3D light)
            {
                lights.Add(light);
            }
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (enabled)
        {
            Vector3 direction = GlobalTransform.Basis.Y;

            Vector3 force = direction*thrust*rb3D.flightInfo.throttle;

            //GD.Print(rb3D.flightInfo.throttle);

            rb3D.ApplyImpulse(force, Position);

            foreach (GpuParticles3D particle in particles)
            {
                particle.Scale = Vector3.One * (rb3D.flightInfo.throttle + 0.01f);
            }
            foreach (Light3D light in lights)
            {
                light.LightEnergy = 100f * rb3D.flightInfo.throttle;
            }
        }
    }
}
