using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPause : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    private void Start()
    {
        pausePanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pausePanel.SetActive(true);           
            
            Time.timeScale = 0;
        }
    }

    public void ButtonOk()
    {
        Time.timeScale = 1;       
        pausePanel.SetActive(false);
    }
}
