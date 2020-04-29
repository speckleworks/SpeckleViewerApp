using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SpeckleUnity;
using SpeckleCore;
using MaterialUI;
using System.Threading.Tasks;

public class LoginBehaviour : MonoBehaviour
{
    private bool serverFieldHasInput = true;
    private bool emailFieldHasInput = false;
    private bool passwordFieldHasInput = false;

    private string serverURL = "https://hestia.speckle.works/api/";
    private string email = "";
    private string password = "";

    public bool useInjectedValues = true;
    public UserCredentials credentials;

    public SpeckleUnityManager manager;
    public DialogBoxConfig dialog;
    public StreamSelectionBehaviour streamSelection;

    public InputField serverInput;
    public InputField emailInput;
    public InputField passwordInput;
    public Button loginButton;
    public Text errorMessage;

    public Text welcomeLabel;

    private void Start ()
    {
        dialog.Open ();

        if (useInjectedValues)
        { 
            loginButton.interactable = true;
            email = credentials.email;
            password = credentials.password;
        }
        
        errorMessage.gameObject.SetActive (false);
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

    public void AttemptLogin ()
    {
        _ = RunAsyncLogin ();
    }

    private async Task RunAsyncLogin ()
    {
        manager.SetServerUrl (serverURL);

        try
        {
            await manager.LoginAsync (email, password, LoginCallback);

        }
        catch (SpeckleException se)
        {
            errorMessage.gameObject.SetActive (true);
            errorMessage.text = "The email or password was incorrect. Please try again.";
        }
        catch (TaskCanceledException tce)
        {
            errorMessage.gameObject.SetActive (true);
            errorMessage.text = "The server didn't respond in time. Please try again.";
        }
    }

    private void LoginCallback (User loggedInUser)
    {
        if (!string.IsNullOrWhiteSpace (loggedInUser.Apitoken))
        {
            welcomeLabel.text = string.Format ("Welcome {0} {1}", loggedInUser.Name, loggedInUser.Surname);

            errorMessage.gameObject.SetActive (false);
            dialog.Close ();
            streamSelection.Initialize ();
        }
        else
        { 
            errorMessage.gameObject.SetActive (true);
            errorMessage.text = "Login failed. Please try again.";
        }
    }
}
