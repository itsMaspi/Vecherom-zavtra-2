using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestApi : MonoBehaviour
{
    public GameObject loginPanel;

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

        string res = await Repository.Login(name, pass);
        Debug.Log($"Bondia, res: {res}");
        response.text = res;
    }



}
