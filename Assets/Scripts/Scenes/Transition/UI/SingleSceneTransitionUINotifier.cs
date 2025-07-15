using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleSceneTransitionUINotifier : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SceneTransitionUIHandler sceneTransitionUIHandler;

    //Only purpose is to put this script on the GameObject with the Animator, since animation events can only refference methods from scripts on the gameobject with the animator
    //This Notify methods call the TransitionIn and Out End from the SceneTransitionUIHandler (above in GameObject Hierarchy)

    public void NotifyTransitionInEnd() => sceneTransitionUIHandler.TransitionInEnd();
    public void NotifyTransitionOutEnd() => sceneTransitionUIHandler.TransitionOutEnd();
}
