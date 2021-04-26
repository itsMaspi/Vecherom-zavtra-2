using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

public class Response
{
    public string Message { get; set; }
    public int UserID { get; set; }

    public Response(string Message, int UserID)
    {
        this.Message = Message;
        this.UserID = UserID;
    }
}
public class Repository
{
    private readonly static string API_URL = "https://25.71.74.254:45455/api/";


    public static string Login(string usr, string psw)
    {
        string response = (string) MakeRequest($"{API_URL}users/{usr}/{psw}", null, null, "GET", "application/json", typeof(Response));
        return response;
    }



    /** 
    * Request amb un simple string al body
    * 
    *     requestUrl: Url completa del Web Service, amb l'opci� sol�licitada
    *     requestHeader: string que se li passa al header
    *     JSONrequest: objecte que se li passa en el body 
    *     JSONmethod: "GET"/"POST"/"PUT"/"DELETE"
    *     JSONContentType: "application/json" en els casos que el Web Service torni objectes
    *     JSONRensponseType:  tipus d'objecte que torna el Web Service (typeof(tipus))
    */
    public static object MakeRequest(string requestUrl, string requestHeader, string strBody, string JSONmethod, string JSONContentType, Type JSONResponseType)
    {
        try
        {
            HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest; //WebRequest WR = WebRequest.Create(requestUrl);
            request.Method = JSONmethod;  // "GET"/"POST"/"PUT"/"DELETE";  
            //request.Headers.Add("Authorization: " + requestHeader);

            if (JSONmethod != "GET")
            {
                request.ContentType = JSONContentType; // "application/json";   
                Byte[] bt = Encoding.UTF8.GetBytes(strBody); // aqui hauria d'anar "sb" si no fos un string el body
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
            }

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(String.Format("Server error (HTTP {0}: {1}).", response.StatusCode, response.StatusDescription));

                Stream stream1 = response.GetResponseStream();
                StreamReader sr = new StreamReader(stream1);
                string strsb = sr.ReadToEnd(); 
                object objResponse = JsonUtility.FromJson(strsb, JSONResponseType);
                return objResponse;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }

    /**
    * Request amb un objecte JSON al body
    * 
    *     requestUrl: Url completa del Web Service, amb l'opci� sol�licitada
    *     requestHeader: string que se li passa al header
    *     JSONrequest: objecte que se li passa en el body 
    *     JSONmethod: "GET"/"POST"/"PUT"/"DELETE"
    *     JSONContentType: "application/json" en els casos que el Web Service torni objectes
    *     JSONRensponseType:  tipus d'objecte que torna el Web Service (typeof(tipus))
    */
    public static object MakeRequestJSON(string requestUrl, string requestHeader, object JSONRequest, string JSONmethod, string JSONContentType, Type JSONResponseType)
    {
        try
        {
            HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest; //WebRequest WR = WebRequest.Create(requestUrl);   
            string sb = JsonUtility.ToJson(JSONRequest);
            request.Method = JSONmethod;  // "GET"/"POST"/"PUT"/"DELETE";  
            request.Headers.Add("Authorization: " + requestHeader);

            if (JSONmethod != "GET")
            {
                request.ContentType = JSONContentType;
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
            }

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(String.Format("Server error (HTTP {0}: {1}).", response.StatusCode, response.StatusDescription));

                Stream stream1 = response.GetResponseStream();
                StreamReader sr = new StreamReader(stream1);
                string strsb = sr.ReadToEnd();
                object objResponse = JsonUtility.FromJson(strsb, JSONResponseType);
                return objResponse;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }
}
