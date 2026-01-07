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
    }

    public override void OnUpdate()
    {
        Screen.CursorVisible = true;
        // Screen.CursorLock = CursorLockMode.Clipped;
        Screen.CursorLock = mode;

        // var mouseDelta = new Float2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        // pitch = Mathf.Clamp(pitch + mouseDelta.Y, -88, 88);
        // yaw += mouseDelta.X;
    }

    public override void OnFixedUpdate()
    {
        // направляме камеру на игрока
        Actor.LookAt(Target.Position, Vector3.Up);
        Debug.Log(Vector3.Up + "up");
        if (Input.Mouse.GetButton(MouseButton.Middle))
        {
            var camTrans = Actor.Transform;
            var camFactor = Mathf.Saturate(CameraSmoothing * Time.DeltaTime);

            // camTrans.Orientation = Quaternion.Lerp(camTrans.Orientation, Quaternion.Euler(pitch, yaw, 0), camFactor);

            var inputH = Input.GetAxis("Horizontal");
            var inputV = Input.GetAxis("Vertical");
            var move = new Vector3(inputH, 0.0f, inputV);
            move.Normalize();
            move = camTrans.TransformDirection(move);

            camTrans.Translation += move * MoveSpeed;
            Actor.Transform = camTrans;
        }
        
        // var desiredPos = Target.Position + Offset;
        //
        // // Двигаем камеру (можно сразу, можно через lerp для плавности)
        // Actor.Position = Float3.Lerp(Actor.Position, desiredPos, 10.0f * Time.DeltaTime);
        //
        // // Поворачиваем камеру в сторону игрока
        // Actor.Orientation = Quaternion.LookAt(Actor.Position, Target.Position);

        var scroll = Input.Mouse.ScrollDelta;
        if (scroll != 0)
        {
            // Debug.Log($"scroll {inp}");
            // var camTrans = Actor.Transform;
            // // var camFactor = Mathf.Saturate(CameraSmoothing * Time.DeltaTime);
            //
            // // camTrans.Orientation = Quaternion.Lerp(camTrans.Orientation, Quaternion.Euler(pitch, yaw, 0), camFactor);
            // var move = new Vector3(inp, inp, inp);
            // move.Normalize();
            // move = camTrans.TransformDirection(move);
            //
            // camTrans.Translation += move * MoveSpeed;
            //
            // Actor.Transform = camTrans;
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
