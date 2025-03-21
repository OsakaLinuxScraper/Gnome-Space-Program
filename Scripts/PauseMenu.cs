using Godot;
using Godot.Collections;

public partial class PauseMenu : Control
{
	[Export] private Control gameUI;
	public bool open;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey)
		{
			if (Input.IsKeyPressed(Key.Escape))
			{
				if (open)
				{
					SetPause(false);
				}else{
					SetPause(true);
				}
			}
		}
    }

	public void SetPause(bool pause)
	{
		open = pause;
		Visible = pause;
		gameUI.Visible = !pause;
	}

	public void UnpauseButton()
	{
		if(open)SetPause(false);
	}
}
