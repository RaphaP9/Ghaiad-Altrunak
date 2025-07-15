using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "NewVideoCinematicSO", menuName = "ScriptableObjects/Cinematics/VideoCinematic")]

public class VideoCinematicSO : ScriptableObject
{
    public int id;
    public string cinematicName;
    public VideoCinematicTransitionType transitionType;
    public VideoClip videoClip;
}
