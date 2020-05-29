using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpeckleCore;

public class StreamRemoverBehaviour : StreamSelectionBehaviour
{
    public CameraSystem cameraSystem;

    public override void Initialize ()
    {
        dialogBox.Open ();

        CompleteInitialization (manager.GetCurrentReceivedStreamMetaData ());
    }

    protected override void CompleteInitialization (SpeckleStream[] currentStreams)
    {
        ReconstructOptions (currentStreams);
    }

    public override void SelectStream (SpeckleStream stream)
    {
        dialogBox.Close ();
        manager.RemoveReceiver (stream.StreamId);

        Bounds modelBounds = manager.GetBoundsForAllReceivedStreams ();
        cameraSystem.FocusOnModel (modelBounds);
    }
}
