using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SpeckleUnity;
using MaterialUI;

public class LoadingBar : MonoBehaviour
{
    public Image progressBar;

    private DialogBoxConfig dialogBox;

    // Start is called before the first frame update
    void Start()
    {
        progressBar.fillAmount = 0;

        dialogBox = GetComponent<DialogBoxConfig> ();
    }

    public void UpdateProgress (SpeckleUnityUpdate updateData)
    {
        if (updateData.updateProgress == 0) dialogBox.Open ();

        if (updateData.updateProgress == 1)
        {
            dialogBox.Close ();
            StartCoroutine (AnimateDisolve (updateData.streamRoot));
        }
        progressBar.fillAmount = updateData.updateProgress;
    }

    private IEnumerator AnimateDisolve (Transform streamParent)
    {
        MeshRenderer[] meshes = streamParent.GetComponentsInChildren<MeshRenderer> ();

        Bounds modelBounds = meshes[0].bounds;

        for (int i = 1; i < meshes.Length; i++)
        {
            modelBounds.Encapsulate (meshes[i].bounds);
        }

        MaterialPropertyBlock block = new MaterialPropertyBlock ();

        float timeStarted = Time.timeSinceLevelLoad;

        float height = modelBounds.center.y + modelBounds.extents.y;
        float heightToTraverse = modelBounds.size.y;

        while (Time.timeSinceLevelLoad - timeStarted < 1 / 0.3f)
        {
            height -= heightToTraverse * Time.deltaTime * 0.3f;
            
            for (int i = 0; i < meshes.Length; i++)
            {
                meshes[i].GetPropertyBlock (block);
                block.SetFloat ("_Height", height);
                meshes[i].SetPropertyBlock (block);
            }

            yield return null;
        }
    }
}
