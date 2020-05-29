using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MaterialUI;
using SpeckleCore;
using SpeckleUnity;
using System.IO;

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
        catch (Exception e)
        {
            HandleError (e);
        }
    }

    protected virtual void CompleteInitialization (SpeckleStream[] availableStreams)
    {
        ReconstructOptions (FilterForNewStreams (availableStreams));
    }

    protected SpeckleStream[] FilterForNewStreams (SpeckleStream[] availableStreams)
    {
        SpeckleStream[] currentStreams = manager.GetCurrentReceivedStreamMetaData ();
        List<SpeckleStream> filteredStreams = new List<SpeckleStream> ();

        for (int i = 0; i < availableStreams.Length; i++)
        {
            bool streamAlreadyLoaded = false;

            for (int j = 0; j < currentStreams.Length; j++)
            {
                if (availableStreams[i].StreamId == currentStreams[j].StreamId)
                {
                    streamAlreadyLoaded = true;
                    break;
                }
            }

            if (!streamAlreadyLoaded) filteredStreams.Add (availableStreams[i]);
        }

        return filteredStreams.ToArray ();
    }

    protected virtual void ReconstructOptions (SpeckleStream[] streams)
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
        catch (Exception e)
        {
            HandleError (e);
        }
    }

    protected virtual void HandleError (Exception e)
    {
        dialogBox.Close ();
        errorDialog.Open ();
        Debug.LogError (e.ToString ());
    }
}
