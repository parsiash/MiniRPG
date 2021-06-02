﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MiniRPG.BattleLogic;
using MiniRPG.Common;
using MiniRPG.Metagame;
using MiniRPG.Navigation;
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

#if DEBUG
        protected void Update()
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(0);
            }
        }
#endif

        public Game game { get; private set; }
        public INavigator rootNavigator { get; private set; }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            logger.Log($"Scene {scene.name} Loaded");

            rootNavigator = new Navigator(logger);
            var battlePage = Object.FindObjectOfType<Battle.BattlePage>(true);
            if(!battlePage)
            {
                logger.LogError("Cannot start the game. No Battle Page Found.");
                return;
            }

            game = new Game(
                new MetagameSimulation(
                    new User(
                        new UserProfile(
                            "GuestUser",
                            Enumerable.Range(1, 8).Select(i => GenerateHero(i)).ToArray()
                        )
                    ),
                    logger
                ),
                logger
            );
        }

        private ProfileHero GenerateHero(int heroId)
        {
            return new ProfileHero(
                heroId,
                "Hero_" + heroId,
                Random.Range(1, 20),
                Random.Range(1, 20),
                Random.Range(1, 5),
                Random.Range(20, 30)
            );
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
                        Random.Range(2, 5),
                        Random.Range(15, 20)
                    )
                );
            }

            return result;
        }
    }
}
