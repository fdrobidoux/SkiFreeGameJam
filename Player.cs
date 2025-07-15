using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Player : Area2D
{
    public static readonly Vector2 NEUTRAL_VECTOR = new Vector2(0f, 1f).Normalized();
    public static readonly Vector2 FIRST_ANGLE_VECTOR = new Vector2(3f, 7f).Normalized();
    public static readonly Vector2 SECOND_ANGLE_VECTOR = new Vector2(7f, 3f).Normalized();
    public static readonly Vector2 LAST_ANGLE_VECTOR = new Vector2(1f, 0.0f).Normalized();

    [Export]
    public Label debugAngleLabel;
    [Export]
    public Label debugPlayerPosLabel;
    [Export]
    public Label debugMousePosLabel;
    [Export]
    public Line2D line;
    private Vector2 lastMousePosition = Vector2.Zero;
    private Vector2 lastDirection = Vector2.Zero;

    public float speed = 0.0f;

    public override void _Ready()
    {
        base._Ready();
        InitialPosition = this.Position;
    }

    public Vector2 InitialPosition { get; private set; }
    public float CurrentAngle { get; set; } = 0.0f;

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);

        if (@event is InputEventMouse @mouseEvent)
        {
            GD.Print($"Mouse Event: {mouseEvent.AsText()}");

            lastMousePosition = mouseEvent.Position;
            CurrentAngle = Mathf.RadToDeg(getAngleFromMouse(mouseEvent.Position));

            debugAngleLabel.Text = $"Angle to mouse: {CurrentAngle.ToString("###.##")} degrees";
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
        processMovement(delta);
        // Debug stuff
        debugPlayerPosLabel.Text = $"Player Position: {GetGlobalTransformWithCanvas().Origin} ({this.Position})";
        line.SetPointPosition(1, lastMousePosition - GetGlobalTransformWithCanvas().Origin);
    }

    private void processMovement(double delta)
    {
        var reverse = CurrentAngle >= 90f;
        var normalizedAngle = CurrentAngle > 90f ? 90f - (CurrentAngle - 90f) : CurrentAngle;

        var directionVector = normalizedAngle switch 
        {
            var angle when angle <= 0f => Vector2.Zero,
            var angle when angle < 15f && angle >= 0f => LAST_ANGLE_VECTOR,
            var angle when angle < 45f && angle >= 15f => SECOND_ANGLE_VECTOR,
            var angle when angle < 75f && angle >= 45f => FIRST_ANGLE_VECTOR,
            _ => NEUTRAL_VECTOR
        };

        if (reverse) directionVector *= new Vector2(-1, 1);

        debugMousePosLabel.Text = $"Direction vector: {directionVector}";
        this.Position += directionVector * (float)delta * calculateSpeed(directionVector, delta);
    }

    private float calculateSpeed(Vector2 normalizedDir, double delta)
    {
        normalizedDir
    }

    public override void _Draw()
    {
        base._Draw();
    }
}
