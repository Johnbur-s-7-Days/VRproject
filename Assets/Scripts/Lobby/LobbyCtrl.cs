using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCtrl : MonoBehaviour
{
    public static bool isGameStart;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnGameStart()
    {
        isGameStart = true;
    }
}
