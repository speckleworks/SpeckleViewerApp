using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SpeckleUnity;
using SpeckleCore;
using MaterialUI;

public class LoginBehaviour : MonoBehaviour
{
    private bool serverFieldHasInput = true;
    private bool emailFieldHasInput = false;
    private bool passwordFieldHasInput = false;

    private string serverURL = "https://hestia.speckle.works/api/";
    private string email = "";
    private string password = "";

    public SpeckleUnityManager manager;
    public DialogBoxConfig dialog;
    public NavDrawerConfig navDrawer;

    public InputField serverInput;
    public InputField emailInput;
    public InputField passwordInput;
    public Button loginButton;
    public GameObject errorMessage;

    public Text welcomeLabel;

    private void Start ()
    {
        dialog.Open ();
        loginButton.interactable = false;
        errorMessage.SetActive (false);
    }

    public void OnServerFieldInput (string input)
    {
        serverFieldHasInput = input.Length > 0;
        serverURL = input;
        UpdateLoginButtonState ();
    }

    public void OnEmailFieldInput (string input)
    {
        emailFieldHasInput = input.Length > 0;
        email = input;
        UpdateLoginButtonState ();
    }

    public void OnPasswordFieldInput (string input)
    {
        passwordFieldHasInput = input.Length > 0;
        password = input;
        UpdateLoginButtonState ();
    }

    private void UpdateLoginButtonState ()
    {
        loginButton.interactable = serverFieldHasInput && emailFieldHasInput && passwordFieldHasInput;
    }

    public async void AttemptLogin ()
    {
        manager.SetServerUrl (serverURL);
        try
        {
            await manager.LoginAsync (email, password, LoginCallback);

        }
        catch (SpeckleException se)
        {
            errorMessage.SetActive (true);
        }
    }

    private void LoginCallback (User loggedInUser)
    {
        if (!string.IsNullOrWhiteSpace (loggedInUser.Apitoken))
        {
            welcomeLabel.text = string.Format ("Welcome {0} {1}", loggedInUser.Name, loggedInUser.Surname);

            errorMessage.SetActive (false);
            dialog.Close ();
            navDrawer.Open ();
        }
        else
        { 
            errorMessage.SetActive (true);
        }
    }
}
