using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioData {
    public string name;
    public int weight;
    public int nextNoteValue;
}

public class AudioMelodyController : MonoBehaviour
{
    [SerializeField] float volume = .3f;

    [SerializeField] AudioClip[] audioClips = null;

    [SerializeField] AudioData[] audioNotes = null;

    public AudioData[] parsedAudioNotes = null;

    int total = 0;

    //int goingUpWeight = 5;
    //int goingDownWeight = 4;
    //int skippingAnOctaveWeight = 1;
    //int backAnOctaveWeight = 1;

    Dictionary<int, int> probabilityWeightDic;

    public int currentIndex = 0;

    AudioData currentAudioData = null;

    void Start()
    {
        probabilityWeightDic = new Dictionary<int, int>();
        parsedAudioNotes = new AudioData[audioClips.Length];

        for (int i = 0; i < audioNotes.Length; i++) {
            total += audioNotes[i].weight;
            parsedAudioNotes[i] = audioNotes[i];
            parsedAudioNotes[i].weight = total;
        }

        //probabilityWeightDic.Add(total, 1);
        //total += goingDownWeight;
        //probabilityWeightDic.Add(total, -1);
        //total += skippingAnOctaveWeight;
        //probabilityWeightDic.Add(total, 7);
        //total += backAnOctaveWeight;
        //probabilityWeightDic.Add(total, -7);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            Ping(transform.position);
        }
    }

    public void Ping(Vector3 pos) {

        float randomNumber = Random.Range(0, total);

        int localTotal = 0;
        int offset = 0;

        for (int i = 0; i < parsedAudioNotes.Length; i++) {
            localTotal += parsedAudioNotes[i].weight;

            if(randomNumber <= localTotal) {
                offset = parsedAudioNotes[i].nextNoteValue;
                break;
            }
        }

        var audio = GameplayManager.I.GetAudioFromPool(transform.position);
        audio.transform.position = pos;
        audio.PlayAndDisable(GetAudioClipWithOffset(offset), volume);


    }

    AudioClip GetAudioClipWithOffset(int offset) {
        currentIndex = currentIndex + offset;

        if (offset > 0) {
            currentIndex = currentIndex % audioClips.Length;
        } else {
            currentIndex = currentIndex < 0 ? audioClips.Length + currentIndex : currentIndex;
        }

        return audioClips[currentIndex];
    }

}