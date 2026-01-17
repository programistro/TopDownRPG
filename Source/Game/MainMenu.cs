using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.GUI;

namespace Game;

/// <summary>
/// MainMenu Script.
/// </summary>
public class MainMenu : Script
{
    public ControlReference<Button> ExitButton;
    public ControlReference<BlurPanel> Blur;
    public bool IsVisibleMenu = false;
    
    /// <inheritdoc/>
    public override void OnStart()
    {
        Blur.Control.Visible = IsVisibleMenu;
        ExitButton.Control.ButtonClicked += Exit;
    }

    private void Exit(Button obj)
    {
        Engine.RequestExit();
    }

    /// <inheritdoc/>
    public override void OnEnable()
    {
        // Here you can add code that needs to be called when script is enabled (eg. register for events)
    }

    /// <inheritdoc/>
    public override void OnDisable()
    {
        // Here you can add code that needs to be called when script is disabled (eg. unregister from events)
    }

    /// <inheritdoc/>
    public override void OnUpdate()
    {
        // Here you can add code that needs to be called every frame
        if (Input.GetKeyDown(KeyboardKeys.Escape))
        {
            IsVisibleMenu = !IsVisibleMenu;
            Blur.Control.Visible = IsVisibleMenu;
            if (IsVisibleMenu)
            {
                Time.TimeScale = 0;
            }
            else
            {
                Time.TimeScale = 1;
            }
        }
    }
}
