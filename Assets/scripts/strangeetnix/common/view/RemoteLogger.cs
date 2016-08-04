using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace strangeetnix
{
	public class RemoteLogger : MonoBehaviour
	{
		string url = "https://script.google.com/macros/s/AKfycbyxcUeWUKDDhka1eC1h7Igj76dF_ldUcTCI-fO7BFaoTMpJM4k/exec";
		bool sema = false;
		Queue<string> log = new Queue<string>();
		
		// Use this for initialization
		void Awake () {
			DontDestroyOnLoad(gameObject);
		}
		
		// Use this for initialization
		void Start () {
			Debug.Log("Hello world!");
		}
		
		// Update is called once per frame
		void Update () {
			if (!sema && log.Count > 0) {
				SendLog();
			}
		}
		
		//get method here http://forum.antichat.ru/showthread.php?t=290347
		string UrlEncode(string instring)
		{
			StringReader strRdr = new StringReader(instring);
			StringWriter strWtr = new StringWriter();
			int charValue = strRdr.Read();
			while (charValue != -1)
			{
				if (((charValue >= 48) && (charValue <= 57)) // 0-9
				|| ((charValue >= 65)  && (charValue <= 90)) // A-Z
				|| ((charValue >= 97)  && (charValue <= 122))) // a-z
				{
					strWtr.Write((char) charValue);
				}
				else if (charValue == 32) // Space
				{
					strWtr.Write("+");
				}
				else
				{
					strWtr.Write("%{0:x2}", charValue);
				}
				charValue = strRdr.Read();
			}
			return strWtr.ToString();
		}
		
		void SendLog() {
			if (sema) return;
			sema = true;
			int count = log.Count > 10 ? 10 : log.Count;
			string t_url = url + "?p0=" + UrlEncode(log.Dequeue());
			for (int i = 1; i < count; i++) {
				t_url += "&p" + i + "=" + UrlEncode(log.Dequeue());
			}
			WWW www = new WWW(t_url);
			StartCoroutine(WaitForRequest(www));
		}

		IEnumerator WaitForRequest(WWW www)
		{
			yield return www;
			// check for errors
			if (www.error == null) {
				//OK
			} else {
				//Error
			}
			sema = false;
		}
		
		void OnEnable() {
			Application.RegisterLogCallback(HandleLog);
		}
		
		void OnDisable() {
			Application.RegisterLogCallback(null);
		}
		
		void HandleLog(string logString, string stackTrace, LogType type) {
			log.Enqueue(logString);
		}
	}
}

