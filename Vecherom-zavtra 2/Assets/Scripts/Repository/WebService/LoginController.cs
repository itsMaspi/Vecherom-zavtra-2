using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoginController : MonoBehaviour
{
    public GameObject loginPanel;
    public GameObject menuPanel;

    TMPro.TMP_InputField username;
    TMPro.TMP_InputField password;
    TMPro.TextMeshProUGUI response;
    [SerializeField] Toggle savePasswordToggle;

    private bool savePassword;

    void Start()
    {
        username = loginPanel.transform.Find("Username").GetComponent<TMPro.TMP_InputField>();
        password = loginPanel.transform.Find("Password").GetComponent<TMPro.TMP_InputField>();

        response = loginPanel.transform.Find("Response").GetComponent<TMPro.TextMeshProUGUI>();


        LoadSavedParameters();
    }

    public async void Login()
    {
        var name = username.text;
        var pass = password.text;

        if (name.Length == 0 || pass.Length == 0) return;

        response.color = new Color(1f, 0.4f, 0.4f);
        Response res = await Repository.Login(name, pass);
        if (res == null)
		{
            response.text = "Connection timed out...";
            return;
		}
        response.text = res.Message;
        

        if (res.Message.Equals("User successfully logged in"))
		{
            // Write session user info
            string path = Application.persistentDataPath + "/usr.vz";
            using (BinaryWriter w = new BinaryWriter(File.Open(path, FileMode.Create)))
            {
                w.Write(res.UserID);
                w.Write(name);
            }

            PlayerNameInput.DisplayName = name;

            // Write user login info
            SaveUserLoginInfo(name, pass, savePassword);

            loginPanel.SetActive(false);
            menuPanel.SetActive(true);
            menuPanel.transform.Find("Username").GetComponent<TMPro.TextMeshProUGUI>().text = $"Logged in: {name}";
            await GameObject.Find("ConfigManager").GetComponent<ConfigManager>().LoadUserSettingsFromDB();
        }
    }

    public async void Singup()
	{
        var name = username.text;
        var pass = password.text;

        if (name.Length == 0 || pass.Length == 0) return;

        response.color = new Color(1f, 1f, 1f);
        Response res = await Repository.Signup(name, pass);
        if (res == null)
        {
            response.color = new Color(1f, 0.4f, 0.4f);
            response.text = "Connection timed out...";
            return;
        }
        if (res.Message == "Success") response.color = new Color(0.2f, 1f, 0.2f);
        response.text = res.Message;
    }

    public void SetRememberPassword(bool remember)
	{
        savePassword = remember;
    }

    private void LoadSavedParameters()
	{
        string path = Application.persistentDataPath + "/loginInfo.vz";
        if (!File.Exists(path)) return;

        bool savePass;
        string usrname, passw;

        using (BinaryReader r = new BinaryReader(File.Open(path, FileMode.Open)))
		{
            savePass = r.ReadBoolean();
            usrname = r.ReadString();
            passw = savePass ? r.ReadString() : "";
        }

        username.text = usrname;
        password.text = passw;
        savePasswordToggle.isOn = savePass;
        savePassword = savePass;
	}

    /// <summary>Creates a file (<c>loginInfo.vz</c>) with:
    /// <list type="bullet">
    ///   <item>
    ///      <term>bool</term>
    ///      <description>savePassword</description>
    ///   </item>
    ///   <item>
    ///      <term>string</term>
    ///      <description>username</description>
    ///   </item>
    ///   <item>
    ///      <term>bool</term>
    ///      <description>password</description>
    ///   </item>
    /// </list>
    /// </summary>
    ///
    private void SaveUserLoginInfo(string username, string password, bool savePassword = false)
	{
        string path = Application.persistentDataPath + "/loginInfo.vz";
        if (!File.Exists(path))
        {
            FileStream fs = File.Create(path);
            fs.Close();
        }

        using (BinaryWriter w = new BinaryWriter(File.Open(path, FileMode.Create)))
        {
            w.Write(savePassword);
            w.Write(username);
            if (savePassword) w.Write(password);
        }
    }
}
