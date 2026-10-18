using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "SignData", menuName = "Game/Sign Data")]
public class SignData : ScriptableObject
{
    public string word;
    public VideoClip video;
}