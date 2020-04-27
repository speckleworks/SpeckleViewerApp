using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MaterialUI;
using SpeckleCore;
using SpeckleUnity;

public class StreamSelectionBehaviour : MonoBehaviour
{
    public SpeckleUnityManager manager;
    public GameObject dividerPrefab;
    public StreamData streamDataPrefab;
    public Transform dataRoot;
    
    private DialogBoxConfig dialogBox;
    private bool initialized = false;


    // Start is called before the first frame update
    void Start()
    {
        dialogBox = GetComponent<DialogBoxConfig> ();
    }

    public async void Initialize ()
    {
        dialogBox.Open ();

        if (initialized) return;

        await manager.GetAllStreamMetaDataForUserAsync (CompleteInitialization);        
    }

    private void CompleteInitialization (SpeckleStream[] streams)
    {
        for (int i = 0; i < streams.Length; i++)
        {
            Instantiate (dividerPrefab, dataRoot);
            Instantiate (streamDataPrefab, dataRoot).Initialize (this, streams[i]);
        }

        Instantiate (dividerPrefab, dataRoot);
        initialized = true;
    }

    public async void OpenStream (SpeckleStream stream)
    {
        dialogBox.Close ();
        await manager.AddReceiverAsync (stream.StreamId, null, true);
    }
}
