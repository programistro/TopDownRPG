using FlaxEngine;

public class FreeCamera : Script
{
    [Limit(0, 100), Tooltip("Camera movement speed factor")]
    public float MoveSpeed { get; set; } = 4;

    [Tooltip("Camera rotation smoothing factor")]
    public float CameraSmoothing { get; set; } = 20.0f;

    private float pitch;
    private float yaw;
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
        if (Input.GetMouseButtonDown(MouseButton.Left))
        {
            var pos = Input.MousePosition;
            var ray = Camera.MainCamera.ConvertMouseToRay(pos);
            if (!Physics.RayCast(ray.Position, ray.Direction, out var hit))
                return;

            if (hit.Collider.Tag == "Player")
            {
                // Actor.Position = hit.Point;
                Actor.LookAt(new Vector3(0, 0, 0), Vector3.Up);
                // Actor.Orientation = new Quaternion(hit.Point.X, hit.Point.Y, hit.Point.Z, 0);
            }
        }
        Debug.Log($"Mouse pos = {Input.MousePosition}, {Screen.Size.X}");  
    }
    
    public override void OnFixedUpdate()
    {
        var inputX = Input.GetAxis("Horizontal");
        var inputY = Input.GetAxis("Vertical");
        
        Vector3 move = new  Vector3(inputX, 0, inputY);
        
        move.Normalize();
        var camTrans = Actor.Transform;
        move = camTrans.TransformDirection(move);
        move.Y = 0;
        camTrans.Translation += move * MoveSpeed * Time.DeltaTime;
        
        
        if (Input.Mouse.GetButton(MouseButton.Middle))
        {
            var mouseDelta = new Float2(Input.GetAxis("Mouse X"), 0);
            pitch = Mathf.Clamp(pitch + mouseDelta.Y, -88, 88);
            yaw += mouseDelta.X;
            
            var camFactor = Mathf.Saturate(CameraSmoothing * Time.DeltaTime);
            var orit = Quaternion.Lerp(camTrans.Orientation, Quaternion.Euler(pitch, yaw, 0), camFactor);
            camTrans.Orientation = orit;
            Debug.Log($"pitch  = {pitch}");
            Debug.Log($"yaw  = {yaw}");
            
            
        }
        Actor.Transform = camTrans;

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
            
            Actor.Position = Target.Position - dir *  newDistance;
            Debug.Log(Target.Position - dir * newDistance);
        }
    }
}