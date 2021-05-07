using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoginController : MonoBehaviour
{
    public GameObject loginPanel;
    public GameObject menuPanel;

    //TMPro.TextMeshProUGUI username;
    //TMPro.TextMeshProUGUI password;
    TMPro.TMP_InputField username;
    TMPro.TMP_InputField password;
    TMPro.TextMeshProUGUI response;

    void Start()
    {
        //username = loginPanel.transform.Find("Username").GetChild(0).Find("Text").GetComponent<TMPro.TextMeshProUGUI>(); //.GetComponent<TMPro.TMP_InputField>()
        //password = loginPanel.transform.Find("Password").GetChild(0).Find("Text").GetComponent<TMPro.TextMeshProUGUI>(); //.GetComponent<TMPro.TMP_InputField>()
        username = loginPanel.transform.Find("Username").GetComponent<TMPro.TMP_InputField>();
        password = loginPanel.transform.Find("Password").GetComponent<TMPro.TMP_InputField>();

        response = loginPanel.transform.Find("Response").GetComponent<TMPro.TextMeshProUGUI>();

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
            string path = Application.persistentDataPath + "/usr.vz";
            using (BinaryWriter w = new BinaryWriter(File.Open(path, FileMode.Create)))
            {
                w.Write(res.UserID);
                w.Write(name);
            }
            loginPanel.SetActive(false);
            menuPanel.SetActive(true);
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

}
