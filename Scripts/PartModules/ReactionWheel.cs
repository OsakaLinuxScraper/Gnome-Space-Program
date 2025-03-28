using Godot;
using System;

public partial class ReactionWheel : PartModule
{
    [Export] public float rotationDamp = 100000000f;
    [Export] public bool stabilityAssist = true;
    
    private PartDefinition rb3D;

    public override void _Ready()
    {
        rb3D = (PartDefinition)GetParent();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (enabled)
        {
            // Hey looks like research is fully impossible now because there is zero fucking information on how to
            // fucking apply a force to an object to match a rotation because fuck you
            // FUCK YOU BITCH im doing this and nobody will fucking stop me this is all your fucking fault google you cock sucking fat fucking losers
            // OOH we love AI we love AI SO FUCKING MUCH it has made research BETTER I LOVE IT SO MUCH god how much I WANT TO FUCKING STRANGLE
            // EVERY CEO THAT THINKS AI IS A GOOD FUCKING THING EVEN CHATGPT DOESNT KNOW HOW TO DO THIS! I'll use your little """"""""AI"""""""" 
            // to build an EMP that FRIES GOOGLE'S SERVERS SO THEY CAN NEVER OPERATE AGAIN! Don't think it's just google doing all the polluting!
            // It's also OpenAI, Microsoft, Apple, and I'm sure many others if I could search up "every company responsible for developing AI" without
            // getting a duovigintillion ChatGPT """written""" """"""""""articles"""""""""" that say NOTHING! In conclusion, all big ""tech"" billionaires
            // deserve to rot forever in hell! And I'm starting to think that even hell isn't bad enough for those "people." Fucking hell you can't
            // even call those things people. Those things are barely fucking human.
            if (stabilityAssist)
            {
                rb3D.AngularVelocity /= rotationDamp;
            }
            rb3D.ApplyTorqueImpulse(rb3D.GlobalTransform.Basis * rb3D.flightInfo.targetRotation);
        }
    }
}
