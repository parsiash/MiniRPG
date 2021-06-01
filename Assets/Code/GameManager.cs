using System.Collections;
using System.Collections.Generic;
using MiniRPG.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiniRPG
{
    public class GameManager : SingletonBehaviour<GameManager>
    {
        protected override void Init()
        {
            logger.Log("Game Manager Init");
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            logger.Log($"Scene {scene.name} Loaded");

            var battleView = Object.FindObjectOfType<BattleView.BattleView>(true);
            if(!battleView)
            {
                logger.Log("No Battle view Found");
            }
        }
    }
}
