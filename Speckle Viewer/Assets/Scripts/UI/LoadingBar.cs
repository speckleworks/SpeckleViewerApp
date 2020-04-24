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

        if (updateData.updateProgress == 1) dialogBox.Close ();

        progressBar.fillAmount = updateData.updateProgress;
    }
}
