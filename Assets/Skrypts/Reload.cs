﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reload : MonoBehaviour
{
 public void ReloadScen()
    {
        SceneManager.LoadScene(0);
    }

}
