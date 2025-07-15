using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleVideoCinematicTransitionUINotifier : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private VideoCinematicUI videoCinematicUI;

    public void NotifyTransitionInOpeningEnd() => videoCinematicUI.TransitionInOpeningEnd();
    public void NotifyTransitionInClosingEnd() => videoCinematicUI.TransitionInClosingEnd();
    public void NotifyTransitionOutOpeningEnd() => videoCinematicUI.TransitionOutOpeningEnd();
    public void NotifyTransitionOutClosingEnd() => videoCinematicUI.TransitionOutClosingEnd();
}
