using FlaxEngine;

public class FreeCamera : Script
{
    [Limit(0, 100), Tooltip("Camera movement speed factor")]
    public float MoveSpeed { get; set; } = 4;

    [Tooltip("Camera rotation smoothing factor")]
    public float CameraSmoothing { get; set; } = 20.0f;

    private float pitch;
    private float yaw;
    public CursorLockMode mode;
    // Player
    public Actor Target;
    public float MinDistance = 200;
    public float MaxDistance = 1500;
    public float ZoomSpeed = 500;

    public override void OnStart()
    {
        var initialEulerAngles = Actor.Orientation.EulerAngles;
        pitch = initialEulerAngles.X;
        yaw = initialEulerAngles.Y;

        Actor.LookAt(Target.Position, Vector3.Up);
    }

    public override void OnUpdate()
    {
        Screen.CursorVisible = true;
        Screen.CursorLock = mode;

        var mouseDelta = new Float2(Input.GetAxis("Mouse X"), Target.Position.Z);
        pitch = Mathf.Clamp(pitch + mouseDelta.Y, -88, 88);
        yaw += mouseDelta.X;
    }

    public override void OnFixedUpdate()
    {
        // Actor.LookAt(Target.Position, Vector3.Up);
        
        var inputX = Input.GetAxis("Horizontal");
        var inputY = Input.GetAxis("Vertical");
        
        
        
        Vector3 move = new  Vector3(inputX, 0, inputY);
        
        move.Normalize();
        var camTrans = Actor.Transform;
        move = camTrans.TransformDirection(move);
        camTrans.Translation += move * MoveSpeed;
        if (Input.Mouse.GetButton(MouseButton.Middle))
        {
            var camFactor = Mathf.Saturate(CameraSmoothing * Time.DeltaTime);
            Debug.Log($"camFactor: {camFactor}");
            camTrans.Orientation = Quaternion.Lerp(camTrans.Orientation, Quaternion.Euler(pitch, yaw, 0), camFactor);
        }
        Actor.Transform = camTrans;
        
        if (move != Vector3.Zero)
        {
            Transform.TransformDirection(move);
            Debug.Log("Moving " + move);
        }
        

        var scroll = Input.Mouse.ScrollDelta;
        if (scroll != 0)
        {
            var dir = Target.Position - Actor.Position;
            var distance = dir.Length;
            
            if (distance <= 0.001f)
                return;
            dir /= distance;
            
            var delta = scroll * ZoomSpeed * Time.DeltaTime;
            
            var newDistance = Mathf.Clamp(distance - delta, MinDistance, MaxDistance);
            
            Actor.Position = Target.Position - dir * newDistance;
            Debug.Log(Target.Position - dir * newDistance);
        }
    }
}

//todo сделать Lock (bool) систему, когда игрок кликнул на иконку персонажа камера вращалась на игрока, а когда нет то свободаня камера
