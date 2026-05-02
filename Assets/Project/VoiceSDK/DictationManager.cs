using UnityEngine;
using Oculus.Voice.Dictation;

public class SmartVoice : MonoBehaviour
{
    public AppDictationExperience dictation;

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
            Debug.Log("VOICE INPUT: " + text);
        }
    }
}