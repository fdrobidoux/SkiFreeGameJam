using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Player : Area2D
{
    [Export]
    public Label debugAngleLabel;
    [Export]
    public Label debugPlayerPosLabel;
    [Export]
    public Line2D line;
    private Vector2 lastMousePosition = Vector2.Zero;

    public Vector2 InitialPosition { get; private set; }

    public override void _Ready()
    {
        base._Ready();
        InitialPosition = this.Position;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);

        if (@event is InputEventMouse @mouseEvent)
        {
            GD.Print($"Mouse Event: {mouseEvent.AsText()}");
            lastMousePosition = mouseEvent.Position;
            var angle = getAngleFromMouse(mouseEvent.Position);
            debugAngleLabel.Text = $"Angle to mouse: {Mathf.RadToDeg(angle).ToString("###")} degrees";
        }
    }

    public override void _UnhandledKeyInput(InputEvent @event)
    {
        base._UnhandledKeyInput(@event);
        
        if (!(@event is InputEventKey @keyEvent)) return;

        GD.Print($"Key Event: {@keyEvent.AsText()}");

        if (@keyEvent.IsPressed())
        {
            if (@keyEvent.Keycode == Key.Z)
            {
                // Reset position to the center of the screen when 'Z' is pressed
                GD.Print("Player position reset to beginning.");
                this.Position = InitialPosition;
            }
        }
    }

    private float getAngleFromMouse(Vector2 mousePosition)
    {
        var carPositionScreen = GetGlobalTransformWithCanvas().Origin;
        var angle = carPositionScreen.AngleToPoint(mousePosition);
        return angle;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        this.Position += new Vector2(0.0f, (float)(50F * delta));
        debugPlayerPosLabel.Text = $"Player Position: {GetGlobalTransformWithCanvas().Origin} ({this.Position})";
        line.SetPointPosition(1, lastMousePosition - GetGlobalTransformWithCanvas().Origin);
    }

    public override void _Draw()
    {
        base._Draw();
    }
}
