using System.Collections;
using System.Collections.Generic;
using MiniRPG.BattleLogic;
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

            var battleController = Object.FindObjectOfType<Battle.BattleController>(true);
            if(!battleController)
            {
                logger.LogError("No Battle Controller Found.");
                return;
            }

            battleController.Init(new Battle.BattleControllerInitData(
                new BattleLogic.BattleInitData(
                    new BattleLogic.PlayerInitData(
                        0,
                        GenerateRandomUnits(3)
                    ),
                    new BattleLogic.PlayerInitData(
                        1,
                        GenerateRandomUnits(1)
                    )
                )
            ));
        }

        private UnitInitData[] GenerateRandomUnits(int count)
        {
            var result = new UnitInitData[count];
            for(int i = 0; i < count; i++)
            {
                result[i] = new UnitInitData(
                    "RandomName" + Random.Range(10, 100),
                    Random.Range(1, 20),
                    Random.Range(1, 20),
                    new UnitStat(
                        Random.Range(1, 20),
                        Random.Range(1, 20)
                    )
                );
            }

            return result;
        }
    }
}
