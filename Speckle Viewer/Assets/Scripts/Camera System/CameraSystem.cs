using UnityEngine;
using System.Collections;

public class CameraSystem : MonoBehaviour
{

    protected Transform _XForm_Camera;
    protected Transform _XForm_Parent;

    protected Vector3 _LocalRotation = new Vector3 (15, 30, 0);
    protected float _CameraDistance = 30f;

    public float MouseSensitivity = 4f;
    public float ScrollSensitvity = 2f;
    public float OrbitDampening = 10f;
    public float ScrollDampening = 6f;

    public bool CameraDisabled = false;


    // Use this for initialization
    void Start ()
    {
        _XForm_Camera = transform;
        _XForm_Parent = transform.parent;
    }

    public void FocusOnModel (Bounds modelBounds)
    {
        _LocalRotation = new Vector3 (15, 30, 0);
        _CameraDistance = modelBounds.size.magnitude;
        _XForm_Parent.position = modelBounds.center;
    }

    void LateUpdate ()
    {
        if (CameraDisabled) return;

        if (Input.GetMouseButton (0))
        {
            //Rotation of the Camera based on Mouse Coordinates
            if (Input.GetAxis ("Mouse X") != 0 || Input.GetAxis ("Mouse Y") != 0)
            {
                _LocalRotation.y += Input.GetAxis ("Mouse X") * MouseSensitivity;
                _LocalRotation.x += Input.GetAxis ("Mouse Y") * MouseSensitivity * -1;

                //Clamp the y Rotation to horizon and not flipping over at the top
                if (_LocalRotation.x < -90f)
                    _LocalRotation.x = -90f;
                else if (_LocalRotation.x > 90f)
                    _LocalRotation.x = 90f;
            }
        }
            

        //Zooming Input from our Mouse Scroll Wheel
        if (Input.GetAxis ("Mouse ScrollWheel") != 0f)
        {
            float ScrollAmount = Input.GetAxis ("Mouse ScrollWheel") * ScrollSensitvity;

            ScrollAmount *= (_CameraDistance * 0.3f);

            _CameraDistance += ScrollAmount * -1f;

            _CameraDistance = Mathf.Clamp (_CameraDistance, 1.5f, 100f);
        }

        //Actual Camera Rig Transformations
        Quaternion QT = Quaternion.Euler (_LocalRotation.x, _LocalRotation.y, 0);
        _XForm_Parent.rotation = Quaternion.Lerp (_XForm_Parent.rotation, QT, Time.deltaTime * OrbitDampening);

        if (_XForm_Camera.localPosition.z != _CameraDistance * -1f)
        {
            _XForm_Camera.localPosition = new Vector3 (0f, 0f, Mathf.Lerp (_XForm_Camera.localPosition.z, _CameraDistance * -1f, Time.deltaTime * ScrollDampening));
        }
    }
}