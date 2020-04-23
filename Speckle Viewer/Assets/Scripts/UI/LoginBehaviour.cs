using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SpeckleUnity;
using SpeckleCore;

public class LoginBehaviour : MonoBehaviour
{
    private bool serverFieldHasInput = true;
    private bool emailFieldHasInput = false;
    private bool passwordFieldHasInput = false;

    private string serverURL = "https://hestia.speckle.works/api/";
    private string email = "";
    private string password = "";

    public TMP_InputField serverInput;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;

    public SpeckleUnityManager manager;
    public Button loginButton;

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
        Debug.Log (email);
        Debug.Log (password);
        await manager.LoginAsync (email, password, LoginCallback);
    }

    private void LoginCallback (User loggedInUser)
    {
        gameObject.SetActive (false);
    }
}
