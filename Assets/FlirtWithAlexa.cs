using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;

public class FlirtWithAlexa : MonoBehaviour {
	const string JSON_CONTENT_TYPE = "application/json";
	public GameObject cubeModel;
	private IEnumerator coroutine;
	private string lastColor;

	// Use this for initialization
	void Start () {
		ServicePointManager.ServerCertificateValidationCallback = AlwaysCorrect;

		coroutine = CheckForChangedColor(1.0f);

		StartCoroutine(coroutine);
	}

	private IEnumerator CheckForChangedColor(float waitTime) {
		while (true) {
			yield return new WaitForSeconds(waitTime);
            string cubeColor = GetCubeColor();

			if (cubeColor != lastColor) {
				GameObject createdCube = (GameObject)Instantiate (cubeModel);

				MeshRenderer gameObjectRenderer = createdCube.GetComponent<MeshRenderer>();

				Color MyColour = Color.clear;
				ColorUtility.TryParseHtmlString (cubeColor, out MyColour);
				gameObjectRenderer.material.color = MyColour;

				lastColor = cubeColor;

				print ("Done Changing color to: " + cubeColor);
			} else {
				print ("No color change detected, still " + cubeColor);
			}
		}
	}

    string GetCubeColor()
    {
        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://alexavr.herokuapp.com/model/last");
        request.Method = WebRequestMethods.Http.Get;
        request.Accept = JSON_CONTENT_TYPE;
        var responseStream = request.GetResponse().GetResponseStream();
        string jsonResponse = new StreamReader(responseStream).ReadToEnd();

        return JSONData.Parse(jsonResponse)["color"];
    }

	bool AlwaysCorrect (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
	{
		return true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
