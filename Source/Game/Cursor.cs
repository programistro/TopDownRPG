using FlaxEngine;
using FlaxEngine.GUI;

namespace Game.Game;

/// <summary>
/// Cursor Script.
/// </summary>
public class Cursor : Script
{
    public CursorLockMode mode;
    public bool CursorVisible;
    public Texture CursorClick;
    public ControlReference<Image> TargetCursor;
    public Texture DefaultCursor;
    
    /// <inheritdoc/>
    public override void OnStart()
    {
        Screen.CursorVisible = CursorVisible;
        Screen.CursorLock = mode;
        
        // TargetCursor.Control.Cursor = CursorType.Hand;
    }
    
    /// <inheritdoc/>
    public override void OnEnable()
    {
    }
    
    private void HandleEdgeScroll()
    {
        Float2 pos = Input.MousePosition;
        if (pos.X > Screen.Size.X - 10) pos.X = 10;
        else if (pos.X < 10) pos.X = Screen.Size.X - 10;
        if (pos.Y > Screen.Size.Y - 10) pos.Y = 10;
        else if (pos.Y < 10) pos.Y = Screen.Size.Y - 10;
    
        Input.MousePosition = pos;
    }

    /// <inheritdoc/>
    public override void OnDisable()
    {
        // Here you can add code that needs to be called when script is disabled (eg. unregister from events)
    }

    /// <inheritdoc/>
    public override void OnUpdate()
    {
        Actor.LocalPosition = new Vector3(Input.MousePosition, 0);

        if (Input.GetMouseButtonDown(MouseButton.Left))
        {
        }

        if (Input.GetMouseButton(MouseButton.Middle))
        {
            // TargetCursor.Brush = new TextureBrush(CursorClick);
            TargetCursor.Control.Brush = new GPUTextureBrush(CursorClick.Texture);
            HandleEdgeScroll();
        }
        else
        {
            TargetCursor.Control.Brush = new GPUTextureBrush(DefaultCursor.Texture);
        }
    }
}
