using Godot;
using System;

public partial class MainCamera : Camera2D
{
    [ExportCategory("Node Paths")]
    [Export]
    public NodePath uiNodePath;
    [Export]
    public NodePath gameAreaNodePath;
    [Export]
    public Node2D toFollow;

    private Control uiNode;
    private Area2D gameAreaNode;
    private CollisionShape2D gameAreaShape;
    private Label uiSizeLabel;
    private Label gameAreaSizeLabel;
    private Label cameraPosLabel;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        uiNode = GetNode<Control>(uiNodePath);
        gameAreaNode = GetNode<Area2D>(gameAreaNodePath);
        gameAreaShape = gameAreaNode.GetChild<CollisionShape2D>(0);

        uiSizeLabel = uiNode.GetNode<Label>("./UI_SizeLabel");
        gameAreaSizeLabel = uiNode.GetNode<Label>("./GameArea_SizeLabel");
        cameraPosLabel = uiNode.GetNode<Label>("./Camera_PosLabel");

        this.Position = Vector2.Zero;

        // Initialize UI or perform any setup needed for the game scene.
        GetWindow().GetViewport().SizeChanged += GameView_SizeChanged;
        GameView_SizeChanged();

        this.MakeCurrent();
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
        toFollow.Position += new Vector2(0.0f, -0.5f);
        cameraPosLabel.Text = $"Camera Position: {this.Position.X}, {this.Position.Y}\n{this.GetScreenCenterPosition()}";

        if (Input.IsKeyPressed(Key.Z))
        {
            toFollow.Position = Vector2.Zero;
        }
    }
}
