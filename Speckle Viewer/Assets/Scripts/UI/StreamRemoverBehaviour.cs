using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpeckleCore;

public class StreamRemoverBehaviour : StreamSelectionBehaviour
{
    public override void Initialize ()
    {
        dialogBox.Open ();

        CompleteInitialization (manager.GetCurrentReceivedStreamMetaData ());
    }

    public override void SelectStream (SpeckleStream stream)
    {
        dialogBox.Close ();
        manager.RemoveReceiver (stream.StreamId);
    }
}
