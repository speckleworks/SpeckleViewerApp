using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class CameraSystem : MonoBehaviour
{

    protected Transform camTransform;
    protected Transform pivotTransform;

    protected Vector3 eulerRotation = new Vector3 (15, 30, 0);
    protected float distanceFromPivot = 30f;

    public float mouseSensitivity = 4f;
    public float scrollSensitvity = 2f;
    public float orbitDampening = 20f;
    public float scrollDampening = 20f;

    public bool cameraDisabled = false;


    // Use this for initialization
    void Start ()
    {
        camTransform = transform;
        pivotTransform = transform.parent;
    }

    public void FocusOnModel (Bounds modelBounds)
    {
        eulerRotation = new Vector3 (15, 30, 0);
        distanceFromPivot = modelBounds.size.magnitude + 0.8f;
        pivotTransform.position = modelBounds.center;
    }

    void LateUpdate ()
    {
        if (cameraDisabled) return;
        if (InputValidation.IsPointerOverUIObject ()) return;

        if (Input.GetMouseButton (0))
        {
            //Rotation of the Camera based on Mouse Coordinates
            if (Input.GetAxis ("Mouse X") != 0 || Input.GetAxis ("Mouse Y") != 0)
            {
                eulerRotation.y += Input.GetAxis ("Mouse X") * mouseSensitivity; // horizontal rotation is along the y axis
                eulerRotation.x += Input.GetAxis ("Mouse Y") * mouseSensitivity * -1;

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

        //Actual Camera Rig Transformations
        Quaternion QT = Quaternion.Euler (eulerRotation.x, eulerRotation.y, 0);
        pivotTransform.rotation = Quaternion.Lerp (pivotTransform.rotation, QT, Time.deltaTime * orbitDampening);

        if (camTransform.localPosition.z != distanceFromPivot * -1f)
        {
            camTransform.localPosition = new Vector3 (0f, 0f, Mathf.Lerp (camTransform.localPosition.z, distanceFromPivot * -1f, Time.deltaTime * scrollDampening));
        }
    }
}