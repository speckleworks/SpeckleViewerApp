using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SpeckleCore;

public class StreamData : MonoBehaviour
{
    public Text nameLabel;
    public Text idLabel;
    public Text descriptionLabel;
    public StreamDataButton button;

    private SpeckleStream stream;

    public void Initialize (StreamSelectionBehaviour selector, SpeckleStream stream)
    {
        this.stream = stream;

        nameLabel.text = stream.Name;
        idLabel.text = stream.StreamId;
        descriptionLabel.text = stream.Description;

        button.Initialize (selector, stream);
    }
}
