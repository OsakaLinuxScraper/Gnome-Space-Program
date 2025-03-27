using Godot;
using System;

public partial class FlightInfo : Node3D
{
    [Export] public float throttle = 0;

    private float throttleIncrement = 0.01f;
    private float throttleIncrementing;

    private FlightUi flightUi;

    public override void _Ready()
    {
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
            }
        }
    }
}
