using UnityEngine;
using UnityEngine.UIElements;

public class MouseRotator : MonoBehaviour
{

    public float SpeedH = 2.0f;
    public float SpeedV = 2.0f;
    public float MaxLateralRotation = 120f;
    public float MaxVerticalRotation = 60f;
    private float _yaw = 0.0f;
    private float _pitch = 0.0f;
    private float _lastmousemovement;
    public MouseButton ControlButton;
    void Update()
    {
        if (Input.GetMouseButton((int)ControlButton))
        {
            _yaw += SpeedH * Input.GetAxis("Mouse X");
            _pitch -= SpeedV * Input.GetAxis("Mouse Y");
        }

        if (!Input.GetMouseButton((int)ControlButton) || (Mathf.Abs(Input.GetAxis("Mouse X")) < 0.00001f && Mathf.Abs(Input.GetAxis("Mouse Y")) < 0.00001f))
        {
            if (Time.fixedTime - _lastmousemovement > 2.0f)
            {
                _lastmousemovement = Time.fixedTime;
                _yaw = 0f;
                _pitch = 0f;
            }
        }
        else
        {
            _lastmousemovement = Time.fixedTime;
        }

        _yaw = Mathf.Clamp(_yaw, -MaxLateralRotation, MaxLateralRotation);
        _pitch = Mathf.Clamp(_pitch, -MaxVerticalRotation, MaxVerticalRotation);
        transform.localEulerAngles = new Vector3(_pitch, _yaw, 0.0f);


    }
}