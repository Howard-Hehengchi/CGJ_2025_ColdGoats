using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] GameObject endPanel;
    [SerializeField] GameObject successPanel;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        endPanel.SetActive(false);
        successPanel.SetActive(false);
    }

    public void ShowEndPanel()
    {
        endPanel.SetActive(true);
    }

    public void ShowSuccessPanel()
    {
        successPanel.SetActive(true);
    }
}
