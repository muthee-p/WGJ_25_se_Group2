using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeListener : MonoBehaviour
{

    void Start()
    {
        GameController.instance.ChangeScene();
    }

}
