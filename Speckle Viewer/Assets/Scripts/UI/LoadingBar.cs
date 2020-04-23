using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SpeckleUnity;

public class LoadingBar : MonoBehaviour
{
    public Image progressBar;

    // Start is called before the first frame update
    void Start()
    {
        progressBar.fillAmount = 0;
        gameObject.SetActive (false);
    }

    public void UpdateProgress (SpeckleUnityUpdate updateData)
    {
        if (updateData.updateProgress == 0) gameObject.SetActive (true);

        if (updateData.updateProgress == 1) gameObject.SetActive (false);

        progressBar.fillAmount = updateData.updateProgress;
    }
}
