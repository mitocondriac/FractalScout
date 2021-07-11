using Controllers.VirtualJoystick;
using Controllers.VirtualThrottle;
using UnityEngine;

public class Navigator : MonoBehaviour
{
    public float Thrust;
    public float RollSensitivity;
    public float PitchSensitivity;
    public float YawSensitivity;
    private Rigidbody body;
    public VirtualJoystick Joystick;

    public VirtualJoystick Rudder;

    public VirtualThrottle Throttle;

    public bool Warp;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        var boost = (Warp) ? 1 : 0.1f;
        body.AddForce(transform.forward * (Throttle.Throttle * Thrust * boost * Time.fixedDeltaTime));
        body.AddRelativeTorque(new Vector3(Joystick.CurrentPosition.y * Time.fixedDeltaTime * PitchSensitivity,
            Rudder.CurrentPosition.x * Time.fixedDeltaTime * YawSensitivity,
            -Joystick.CurrentPosition.x * Time.fixedDeltaTime * RollSensitivity)*boost);
    }
    public string GetStats()
    {
        var eulerAngles = transform.eulerAngles;
        return $"Speed: {body.velocity.magnitude}\nPitch: {eulerAngles.x}\nRoll: {eulerAngles.z}\nYaw: {eulerAngles.y}\nApplied Thrust: {Throttle.Throttle * Thrust}\nFPS:{1f/Time.deltaTime}";
    }
}