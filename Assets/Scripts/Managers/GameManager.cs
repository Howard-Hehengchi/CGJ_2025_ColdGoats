using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void PlayerDie()
    {
        Time.timeScale = 0f;
        UIManager.Instance.ShowEndPanel();
    }

    public void PlayerSucceed()
    {
        Time.timeScale = 0f;
        UIManager.Instance.ShowSuccessPanel();
    }
}
