using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// Player Script.
/// </summary>
public class Player : Script
{
    [Limit(0, 100), Tooltip("Player movement speed factor")]
    public float MoveSpeed { get; set; } = 4;

    public RigidBody RigidBody;

    public Prefab Prefab;
    
    /// <inheritdoc/>
    public override void OnStart()
    {
        // Here you can add code that needs to be called when script is created, just before the first game update
        
        RayCastHit hit;
        if (Physics.RayCast(Actor.Position, Actor.Direction, out hit))
        {
            DebugDraw.DrawSphere(new BoundingSphere(hit.Point, 50), Color.Red);
        }
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
    }
    
    private Vector3 _velocity;
    public CharacterController _controller;

    public override void OnFixedUpdate()
    {
        var inputX = Input.GetAxis("Horizontal");
        var inputY = Input.GetAxis("Vertical");
        
        Vector3 move = new  Vector3(inputX, 0, inputY);
        
        move.Normalize();
        
        if (move != Vector3.Zero)
        {
            RigidBody.AddMovement(move);
            Debug.Log("Moving " + move);
        }
        
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown((MouseButton.Left)))
        {
            var pos = Input.MousePosition;
            var ray = Camera.MainCamera.ConvertMouseToRay(pos);
            if (!Physics.RayCast(ray.Position, ray.Direction, out var hit))
                return;
               
            DebugDraw.DrawSphere(new BoundingSphere(hit.Point, 50), Color.Red);
            PrefabManager.SpawnPrefab(Prefab, hit.Point);
        }
    }
}
