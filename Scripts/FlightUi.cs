using Godot;
using System;

public partial class FlightUi : Control
{
	[Export] public ProgressBar throttleBar;
	[Export] public MeshInstance3D navball;
	[Export] public Label altitude;
	[Export] public Label velocity;
	[Export] public HBoxContainer ivaContainer;
}
