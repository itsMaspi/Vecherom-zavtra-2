using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestApi : MonoBehaviour
{
    public GameObject loginPanel;

    TMPro.TextMeshProUGUI username;
    TMPro.TextMeshProUGUI password;
    TMPro.TextMeshProUGUI response;

    void Start()
    {
        username = loginPanel.transform.Find("Username").GetChild(0).Find("Text").GetComponent<TMPro.TextMeshProUGUI>();
        password = loginPanel.transform.Find("Password").GetChild(0).Find("Text").GetComponent<TMPro.TextMeshProUGUI>();
        response = loginPanel.transform.Find("Response").GetComponent<TMPro.TextMeshProUGUI>();

    }

    public void Login()
    {
        var name = username.text;
        var pass = password.text;

        var res = Repository.Login(name, pass);
        Debug.Log($"Bondia, res: {res}");
        response.text = res;
    }



}
