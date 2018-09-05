using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace REEL.EAIEditor
{
	public class LoginButton : MonoBehaviour
	{
        [SerializeField] private string sceneName = "D2EEditor_Editor";
        [SerializeField] private string requestIP = "";
        [SerializeField] private InputField idField = null;
        [SerializeField] private InputField pwField = null;
        [SerializeField] private Toggle keepLoginStatusToggle = null;

		public void OnLoginButtonCliced()
        {
            Debug.Log(idField.text + " : " + pwField.text + " : " + keepLoginStatusToggle.isOn);

            // Try Login.
            //StartCoroutine(RequestLogin(idField.text, pwField.text));

            // Move to work space scene.
            SceneManager.LoadScene(sceneName);
        }

        private void MoveMainScene()
        {
            SceneManager.LoadScene(sceneName);
        }
        
        // Request login with input.
        IEnumerator RequestLogin(string id, string pw)
        {
            WWW www = new WWW(requestIP + "/" + id);
            WWWForm form = new WWWForm();
            form.AddField("password", pw);
            yield return www;

            // Check Result.
            string resultString = www.text;
            LoginResult result = JsonUtility.FromJson<LoginResult>(resultString);

            // test print.
            Debug.Log(result);
        }

        [System.Serializable]
        public class LoginResult
        {
            public bool success;
            public int code;
            public string message;
            public string id;

            public override string ToString()
            {
                return success.ToString() + " : " + code.ToString() + " : " + message + " : " + id;
            }
        }
	}
}