using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDialogue : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private DialogueSO dialogueSO;

    private void Update()
    {
        Test();
    }

    private void Test()
    {
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            DialogueManager.Instance.StartDialogue(dialogueSO);
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            DialogueManager.Instance.EndSentence();
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            DialogueManager.Instance.EndDialogue();
        }
    }
}
