using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using SpeckleCore;

public class StreamDataButton : MonoBehaviour, IPointerDownHandler
{
    private StreamSelectionBehaviour selector;
    private SpeckleStream stream;

    public void Initialize (StreamSelectionBehaviour selector, SpeckleStream stream)
    {
        this.selector = selector;
        this.stream = stream;
    }

    public void OnPointerDown (PointerEventData eventData)
    {
        selector.OpenStream (stream);
    }
}
