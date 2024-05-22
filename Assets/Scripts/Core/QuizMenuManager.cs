using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuizMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject quizMenu;
    public InputActionProperty showButton;
    [SerializeField] private Transform head;
    [SerializeField] private float spawnDistance = 2;
    [SerializeField] private Transform transformSpawner;


    private void Start()
    {
        quizMenu.SetActive(false);
    }

    private void Update()
    {
        if (showButton.action.WasPressedThisFrame())
        {
            quizMenu.SetActive(!quizMenu.activeSelf);
            quizMenu.transform.position = transformSpawner.position;
/*            quizMenu.transform.position = head.position + new Vector3(head.forward.x, 0, head.forward.z).normalized * spawnDistance;*/

            
        }

        quizMenu.transform.LookAt(new Vector3(head.position.x, quizMenu.transform.position.y, head.position.z));
        quizMenu.transform.forward *= -1;

    }
}
