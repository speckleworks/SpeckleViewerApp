using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MaterialUI;

public class NavDrawerBehaviour : MonoBehaviour
{
    public StreamSelectionBehaviour streamSelector;
    public StreamRemoverBehaviour streamRemover;

    private NavDrawerConfig navDrawer;


    // Start is called before the first frame update
    void Start()
    {
        navDrawer = GetComponent<NavDrawerConfig> ();
    }

    public void OnAddStream ()
    {
        streamSelector.Initialize ();
        navDrawer.Close ();
    }

    public void OnRemoveStream ()
    {
        streamRemover.Initialize ();
        navDrawer.Close ();
    }

    public void OnLogout ()
    {
        SceneManager.LoadScene (0);
    }
}
