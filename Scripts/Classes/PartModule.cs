using Godot;
using System;

// Base class for all part modules, so that they can be more identified in bulk or whatever
public partial class PartModule : Node
{
    [Export] public bool enabled;
}
