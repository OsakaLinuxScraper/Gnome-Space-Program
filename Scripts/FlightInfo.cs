using Godot;
using System;

public partial class FlightInfo : Node3D
{
    [Export] public float throttle = 0;

    // I fucking WISH I could use angles but unfortunately nobody on this fucking planet knows how to do that??
    // Is it really that complicated? Or is it just google search being dumbed down in favor of advertisers and AI trash?? 
    // Maybe we need another Luigi to handle the Google CEO... That inhuman scum piece of shit leech doesn't seem to be improving things too much.
    [Export] public Vector3 targetRotation = new();

    public Node3D rootPart = null;

    private float rotationAmnt = 100f;

    private float throttleIncrement = 0.01f;
    private float throttleIncrementing;

    private FlightUi flightUi;

    private UniverseManager universeManager;

    public override void _Ready()
    {
        universeManager = (UniverseManager)GetTree().GetFirstNodeInGroup("UniverseManager");
        flightUi = (FlightUi)GetTree().GetFirstNodeInGroup("FlightUI");
    }

    public override void _Process(double delta)
    {
        if (throttle <= 1 && throttle >= 0)
        {
            throttle += throttleIncrementing;
        }else if (throttle > 1){
            throttle = 1;
        }else if (throttle < 0){
            throttle = 0;
        }

        flightUi.throttleBar.Value = throttle;
        if(rootPart!=null)
        {
            Transform3D rotationFromPlanet = universeManager.currentPlanet.Transform.LookingAt(rootPart.GlobalPosition);
            flightUi.navball.Transform = rotationFromPlanet;
            flightUi.navball.GlobalPosition = Vector3.Zero;
            flightUi.navball.GlobalRotation += rootPart.GlobalRotation;

            float altitudeFromCore = universeManager.currentPlanet.GlobalPosition.DistanceTo(rootPart.GlobalPosition);
            flightUi.altitude.Text = altitudeFromCore - universeManager.currentPlanet.radius + "m";
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey keyEvent)
        {
            switch (keyEvent.Keycode)
            {
                case Key.Shift:
                    if(keyEvent.Pressed)
                    {
                        throttleIncrementing = throttleIncrement;
                    }else{
                        throttleIncrementing = 0;
                    }
                    break;
                case Key.Ctrl:
                    if(keyEvent.Pressed)
                    {
                        throttleIncrementing = -throttleIncrement;
                    }else{
                        throttleIncrementing = 0;
                    }
                    break;
                // craft rotation controls
                case Key.S:
                    if(keyEvent.Pressed)
                    {
                        targetRotation.Z = rotationAmnt;
                    }else{
                        targetRotation.Z = 0;
                    }
                    break;
                case Key.W:
                    if(keyEvent.Pressed)
                    {
                        targetRotation.Z = -rotationAmnt;
                    }else{
                        targetRotation.Z = 0;
                    }
                    break;
                case Key.D:
                    if(keyEvent.Pressed)
                    {
                        targetRotation.X = rotationAmnt;
                    }else{
                        targetRotation.X = 0;
                    }
                    break;
                case Key.A:
                    if(keyEvent.Pressed)
                    {
                        targetRotation.X = -rotationAmnt;
                    }else{
                        targetRotation.X = 0;
                    }
                    break;
                case Key.Q:
                    if(keyEvent.Pressed)
                    {
                        targetRotation.Y = rotationAmnt;
                    }else{
                        targetRotation.Y = 0;
                    }
                    break;
                case Key.E:
                    if(keyEvent.Pressed)
                    {
                        targetRotation.Y = -rotationAmnt;
                    }else{
                        targetRotation.Y = 0;
                    }
                    break;
            }
        }
    }
}
