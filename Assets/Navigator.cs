using Controllers.VirtualJoystick;
using UnityEngine;
using Slider = UnityEngine.UI.Slider;
using Toggle = UnityEngine.UI.Toggle;

public class Navigator : MonoBehaviour
{
    public float Thrust;
    public float RollSensitivity;
    public float PitchSensitivity;
    public float YawSensitivity;
    private Rigidbody body;
    public float RotationInertia = 10f;
    public VirtualJoystick Joystick;

    public Slider Rudder;

    public Slider Throttle;

    public Toggle Warp;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        var boost = (Warp.isOn) ? 1 : 0.1f;
        body.AddForce(transform.forward * (Throttle.value * Thrust * boost * Time.fixedDeltaTime));
        body.AddRelativeTorque(new Vector3(Joystick.CurrentPosition.y * Time.fixedDeltaTime * PitchSensitivity,
            Rudder.value * Time.fixedDeltaTime * YawSensitivity,
            -Joystick.CurrentPosition.x * Time.fixedDeltaTime * RollSensitivity)*boost/RotationInertia);
    }
    public string GetStats()
    {
        var eulerAngles = transform.eulerAngles;
        return $"Speed: {body.velocity.magnitude}\nPitch: {eulerAngles.x}\nRoll: {eulerAngles.z}\nYaw: {eulerAngles.y}\nApplied Thrust: {Throttle.value * Thrust}\nFPS:{1f/Time.deltaTime}";
    }
    
}