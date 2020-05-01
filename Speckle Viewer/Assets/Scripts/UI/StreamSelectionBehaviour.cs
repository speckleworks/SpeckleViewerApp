using System;
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
    
    protected DialogBoxConfig dialogBox;

    [SerializeField] private DialogBoxConfig errorDialog;


    // Start is called before the first frame update
    void Start()
    {
        dialogBox = GetComponent<DialogBoxConfig> ();
    }

    public virtual async void Initialize ()
    {
        dialogBox.Open ();

        try
        {
            await manager.GetAllStreamMetaDataForUserAsync (CompleteInitialization);
        }
        catch (Exception)
        {
            HandleError ();
        }
    }

    protected virtual void CompleteInitialization (SpeckleStream[] streams)
    {
        foreach (Transform child in dataRoot)
        {
            Destroy (child.gameObject);
        }

        for (int i = 0; i < streams.Length; i++)
        {
            Instantiate (dividerPrefab, dataRoot);
            Instantiate (streamDataPrefab, dataRoot).Initialize (this, streams[i]);
        }

        Instantiate (dividerPrefab, dataRoot);
    }

    public virtual async void SelectStream (SpeckleStream stream)
    {
        dialogBox.Close ();

        try
        {
            await manager.AddReceiverAsync (stream.StreamId, null, true);
        }
        catch (Exception)
        {
            HandleError ();
        }
    }

    protected virtual void HandleError ()
    {
        dialogBox.Close ();
        errorDialog.Open ();
    }
}
