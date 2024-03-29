﻿using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    GUIText m_PlayGame;
    GUIText m_ExitGame;
    GUIText m_Practice;

    Game m_Game;

    void Start()
    {
        m_PlayGame = GameObject.Find("PlayGame").guiText;
        m_ExitGame = GameObject.Find("ExitGame").guiText;
        m_Practice = GameObject.Find("Practice").guiText;
        m_Game = Game.Instance;
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
	        if (m_PlayGame.HitTest(Input.mousePosition))
	        {
	            // TODO:: player needs to be able to load a saved level (in a certain world)
                Application.LoadLevel("LevelSelect");
	        }
            else if (m_ExitGame.HitTest(Input.mousePosition))
	        {
                Application.Quit();
	        }
            else if (m_Practice.HitTest(Input.mousePosition))
            {
                m_Game.StartPractice();
            }
        }
    }
}