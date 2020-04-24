using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MaterialUI;
using SpeckleCore;

public class StreamSelectionBehaviour : MonoBehaviour
{
    public GameObject dividerPrefab;
    public StreamData streamDataPrefab;
    public Transform dataRoot;
    private DialogBoxConfig dialogBox;

    // Start is called before the first frame update
    void Start()
    {
        dialogBox = GetComponent<DialogBoxConfig> ();
    }

    public void Initialize (SpeckleStream[] streams)
    {
        for (int i = 0; i < streams.Length; i++)
        {
            Instantiate (dividerPrefab, dataRoot);
            Instantiate (streamDataPrefab, dataRoot).Initialize (this, streams[i]);
        }
           
        Instantiate (dividerPrefab, dataRoot);
    }

    public void OpenStream (SpeckleStream stream)
    { 
        
    }
}
