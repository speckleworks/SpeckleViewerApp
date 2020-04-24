using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MaterialUI;

public class NavDrawerBehaviour : MonoBehaviour
{
    private NavDrawerConfig navDrawer;

    // Start is called before the first frame update
    void Start()
    {
        navDrawer = GetComponent<NavDrawerConfig> ();
    }

    public void OnAddStream ()
    { 
        
    }

    public void OnRemoveStream ()
    { 
        
    }

    public void OnLogout ()
    {
        SceneManager.LoadScene (0);
    }
}
