using Godot;
using System;

public partial class GameScene : Node2D
{
	private Godot.Control uiNode;
    private Godot.Area2D gameAreaNode;
    private Godot.CollisionShape2D gameAreaShape;
    private Godot.Label uiSizeLabel;
    private Godot.Label gameAreaSizeLabel;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        uiNode = GetNode<Control>("./UI");
        gameAreaNode = GetNode<Area2D>("./GameArea");
        gameAreaShape = gameAreaNode.GetChild<CollisionShape2D>(0);

        if (uiNode == null)
		{
			throw new Exception("UI node not found in the scene tree. Please ensure it exists at /Root/UI.");
        }

        GetWindow().GetViewport().SizeChanged += GameView_SizeChanged;

        // Initialize UI or perform any setup needed for the game scene.
        uiSizeLabel = uiNode.GetNode<Label>("./UI_SizeLabel");
        gameAreaSizeLabel = uiNode.GetNode<Label>("./GameArea_SizeLabel");

        uiSizeLabel.Text = "WHAT";

        GameView_SizeChanged();
    }

    private void GameView_SizeChanged()
    {
        var viewportRect = GetViewport().GetVisibleRect();
        uiNode.Size = viewportRect.Size;
        uiNode.Position = viewportRect.Position;
        gameAreaShape.Position = viewportRect.Size;

        uiSizeLabel.Text = $"UI Size: {uiNode.Size.X} x {uiNode.Size.Y}";
        gameAreaSizeLabel.Text = $"Game Area Size: {gameAreaShape.Position.X} x {gameAreaShape.Position.Y}";
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
