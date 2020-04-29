using UnityEngine;
//using UnityEngine.InputSystem;
//using UnityEngine.InputSystem.EnhancedTouch;
using System.Collections;
//using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class CameraSystem : MonoBehaviour
{

    protected Transform camTransform;
    protected Transform pivotTransform;

    protected Vector3 eulerRotation = new Vector3 (15, 30, 0);
    protected float distanceFromPivot = 30f;

    public float touchSensitivity = 0.2f;
    public float pinchSensitvity = 0.2f;
    public float panSensitivity = 0.5f;

    [Space, Space]
    public float mouseSensitivity = 4f;
    public float scrollSensitvity = 2f;
    public float orbitDampening = 20f;
    public float scrollDampening = 20f;

    public bool cameraDisabled = false;

    private float previousPinchDistance = 0;

    // Use this for initialization
    void Start ()
    {
        Application.targetFrameRate = 30;

        camTransform = transform;
        pivotTransform = transform.parent;

        //EnhancedTouchSupport.Enable ();
    }

    public void FocusOnModel (Bounds modelBounds)
    {
        eulerRotation = new Vector3 (15, 30, 0);
        distanceFromPivot = modelBounds.size.magnitude * 0.75f;
        pivotTransform.position = modelBounds.center;
    }

    void LateUpdate ()
    {
        if (cameraDisabled) return;
        if (InputValidation.IsPointerOverUIObject ()) return;

        if (Application.platform != RuntimePlatform.Android)
            RegisterDesktopInput ();
        RegisterTouchInput ();                

        //Actual Camera Rig Transformations
        Quaternion desiredRotation = Quaternion.Euler (eulerRotation.x, eulerRotation.y, 0);
        pivotTransform.rotation = Quaternion.Lerp (pivotTransform.rotation, desiredRotation, Time.deltaTime * orbitDampening);

        distanceFromPivot = Mathf.Clamp (distanceFromPivot, 1, 10000);

        if (camTransform.localPosition.z != distanceFromPivot * -1f)
        {
            camTransform.localPosition = new Vector3 (0f, 0f, Mathf.Lerp (camTransform.localPosition.z, distanceFromPivot * -1f, Time.deltaTime * scrollDampening));
        }
    }

    private void RegisterDesktopInput ()
    {
        if (Input.GetMouseButton (0))
        {
            //Rotation of the Camera based on Mouse Coordinates
            if (Input.GetAxis ("Mouse X") != 0 || Input.GetAxis ("Mouse Y") != 0)
            {
                eulerRotation.y += Input.GetAxis ("Mouse X") * mouseSensitivity; // horizontal rotation is along the y axis
                eulerRotation.x -= Input.GetAxis ("Mouse Y") * mouseSensitivity;

                eulerRotation.x = Mathf.Clamp (eulerRotation.x, -90f, 90f);
            }
        }

        //Zooming Input from our Mouse Scroll Wheel
        if (Input.GetAxis ("Mouse ScrollWheel") != 0f)
        {
            float scrollAmount = Input.GetAxis ("Mouse ScrollWheel") * scrollSensitvity;

            scrollAmount *= (distanceFromPivot * 0.3f);

            distanceFromPivot += scrollAmount * -1f;

            distanceFromPivot = Mathf.Clamp (distanceFromPivot, 1.5f, 100f);
        }
    }

    private void RegisterTouchInput ()
    {
        if (Input.touchCount == 1 && Input.GetTouch (0).phase == TouchPhase.Moved)
        {
            eulerRotation.y += Input.GetTouch (0).deltaPosition.x * touchSensitivity;
            eulerRotation.x -= Input.GetTouch (0).deltaPosition.y * touchSensitivity;

            eulerRotation.x = Mathf.Clamp (eulerRotation.x, -90f, 90f);
        }

        if (Input.touchCount == 2 && (Input.GetTouch (0).phase == TouchPhase.Moved || Input.GetTouch (1).phase == TouchPhase.Moved))
        {
            float currentPinchDistance = Vector2.Distance (Input.GetTouch (0).position, Input.GetTouch (1).position);

            distanceFromPivot += currentPinchDistance * pinchSensitvity * (currentPinchDistance > previousPinchDistance ? -1 : 1);
            previousPinchDistance = currentPinchDistance;
        }

        if (Input.touchCount == 3 && Input.GetTouch (0).phase == TouchPhase.Moved)
        {
            pivotTransform.Translate (Vector3.right * Input.GetTouch (0).deltaPosition.x * panSensitivity * -1);
            pivotTransform.Translate (transform.up * Input.GetTouch (0).deltaPosition.y * panSensitivity * -1, Space.World);
        }
            
        /*
        if (Touch.activeTouches.Count == 1)
        {
            if (Touch.activeTouches[0].phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                Touch.activeTouches[0].screenPosition
            }
            if (Touch.activeTouches[0].phase != UnityEngine.InputSystem.TouchPhase.Began)
            {
                eulerRotation.y += Touch.activeTouches[0].delta.x * touchSensitivity;
                eulerRotation.x -= Touch.activeTouches[0].delta.y * touchSensitivity;

                eulerRotation.x = Mathf.Clamp (eulerRotation.x, -90f, 90f);
            }
        }

        if (Touch.activeTouches.Count == 2)
        {
            float currentPinchDistance = Vector2.Distance (Touch.activeTouches[0].screenPosition, Touch.activeTouches[1].screenPosition);

            if (Touch.activeTouches[1].phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                previousPinchDistance = currentPinchDistance;
            }
            else if (Touch.activeTouches[1].phase == UnityEngine.InputSystem.TouchPhase.Moved)
            {
                distanceFromPivot += (previousPinchDistance - currentPinchDistance) * pinchSensitvity;
            }
        }
        */
    }
}