using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class OpenAIAPITest : MonoBehaviour
{
    private string apiKey;

    void Start()
    {
        apiKey = System.Environment.GetEnvironmentVariable("OPENAI_API_KEY");

        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("API Key missing!");
            return;
        }

        StartCoroutine(SendRequest("Hallo"));
    }

    IEnumerator SendRequest(string prompt)
    {
        string url = "https://api.openai.com/v1/responses";

        // Minimal request body for GPT-4o-mini
        string json = "{\"model\":\"gpt-4o-mini\",\"input\":\"" + prompt + "\"}";

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return request.SendWebRequest();

        // Console Output
        if (request.result == UnityWebRequest.Result.Success)
        {
            // komplette Response
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("FULL RESPONSE:\n" + jsonResponse);

            // nur GPT Text
            string extracted = ExtractText(jsonResponse);
            Debug.Log("GPT: " + extracted);
        }
        else
        {
            Debug.LogError("Request failed: " + request.error);
            Debug.LogError(request.downloadHandler.text);
        }
    }
    string ExtractText(string json)
    {
        string search = "\"text\": \"";
        int startIndex = json.IndexOf(search);

        if (startIndex == -1)
        {
            // fallback (manchmal ohne space nach :)
            search = "\"text\":\"";
            startIndex = json.IndexOf(search);

            if (startIndex == -1)
                return "No text found";
        }

        startIndex += search.Length;

        int endIndex = json.IndexOf("\"", startIndex);

        if (endIndex == -1)
            return "No closing quote found";

        return json.Substring(startIndex, endIndex - startIndex);
    }

}