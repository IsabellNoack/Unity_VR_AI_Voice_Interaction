using UnityEngine;
using Oculus.Voice.Dictation;
using TMPro; // TextMeshPro

public class SmartVoice : MonoBehaviour
{
    public AppDictationExperience dictation;
    public TextMeshPro tmp3DText;

    private bool isRestarting;

    void Start()
    {
        Debug.Log("Start Voice Stuff");
        dictation.DictationEvents.OnFullTranscription.AddListener(OnText);
        dictation.DictationEvents.OnStoppedListening.AddListener(OnStopped);

        StartDictation();
    }

    void StartDictation()
    {
        if (!dictation.Active)
        {
            dictation.Activate();
        }
    }

    void OnStopped()
    {
        if (isRestarting) return;

        isRestarting = true;
        Invoke(nameof(Restart), 0.5f);
    }

    void Restart()
    {
        isRestarting = false;
        StartDictation();
    }

    void OnText(string text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            Debug.Log("VOICE: " + text); // Log to console

            if (tmp3DText != null)
            {
                tmp3DText.text = text; // Update 3D TextMeshPro Text
            }
        }
    }
}