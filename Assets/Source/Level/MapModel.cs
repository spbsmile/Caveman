using System.Collections;
using System.Collections.Generic;
using Caveman.Bonuses;
using Caveman.Configs;
using Caveman.Pools;
using Caveman.Setting;
using Caveman.Utils;
using Caveman.Weapons;
using UnityEngine;
using Random = System.Random;

namespace Caveman.Level
{
    public class MapModel : MonoBehaviour
    {
        private Random rand;
        private int width;
        private int height;

        public void CreateTerrain(Random rand, string pathPrefabTile, List<MapConfig.Artefacts> artefacts, int width, int height, bool isMultiplayer)
        {
            this.width = width;
            this.height = height;
            this.rand = rand;

            var prefabCell = Instantiate(Resources.Load(pathPrefabTile, typeof(GameObject))) as GameObject;

            var tileSize = prefabCell.GetComponent<SpriteRenderer>().bounds.size;
            var tilesOnWidth = width / tileSize.x;
            var tilesOnHeight = height / tileSize.y;

            for (var i = 0; i < tilesOnWidth; i++)
            {
                for (var j = 0; j < tilesOnHeight; j++)
                {
                    var item = Instantiate(prefabCell);
                    item.transform.SetParent(transform);
                    item.transform.position = new Vector2(i * tileSize.x, j * tileSize.y);
                }
            }

            foreach (var artefact in artefacts)
            {
                var artefactPrefab = Instantiate(Resources.Load(artefact.PathPrefab, typeof(GameObject))) as GameObject;

                for (var i = 0; i < artefact.Count; i++)
                {
                    var item = Instantiate(artefactPrefab);
                    item.transform.SetParent(transform);
                    if (!isMultiplayer)
                    {
                        gameObject.transform.position = new Vector2(rand.Next(width),
                        rand.Next(height));
                    }
                    else
                    {
                        gameObject.transform.position = new Vector2(width / (artefact.Count * 2) * (i + 2), height / (artefact.Count * 2) * (i + 2));
                    }
                }
            }
        }

        public void PutAllItemsOnMap(string[] typesItems)
        {
            for (var i = 0; i < typesItems.Length; i++)
            {
                switch (typesItems[i])
                {
                    case "weapons":
                        // todo sort config to folder/levels game
                        foreach (var weaponConfig in EnterPoint.CurrentSettings.WeaponsConfigs.Values)
                        {
                            StartCoroutine(PutItemsOnMap<WeaponModelBase>(weaponConfig.PrefabPath,
                                weaponConfig.TimeRespawn));
                        }
                        break;
                    case "bonuses":
                        foreach (var bonusConfig in EnterPoint.CurrentSettings.BonusesConfigs.Values)
                        {
                            StartCoroutine(PutItemsOnMap<BonusBase>(bonusConfig.PrefabPath, bonusConfig.TimeRespawn));
                        }
                        break;
                }
            }
        }

        private IEnumerator PutItemsOnMap<T>(string poolId, float timeRespawn) where T : MonoBehaviour
        {
            yield return new WaitForSeconds(timeRespawn);
            //todo length
            // todo var bound = Settings.BonusSpeedMaxCount - PoolsManager.instance.BonusesSpeed.GetActivedCount; 
            for (var i = 0; i < Settings.WeaponInitialLying; i++)
            {
                PutItemOnMap((ObjectPool<T>)PoolsManager.instance.Pools[poolId]);
            }
            StartCoroutine(PutItemsOnMap<T>(poolId, timeRespawn));
        }

        private void PutItemOnMap<T>(ObjectPool<T> pool) where T : MonoBehaviour
        {
            var item = pool.New();
            StartCoroutine(UnityExtensions.FadeIn(item.GetComponent<SpriteRenderer>()));
            item.transform.position = new Vector2(rand.Next(1, width - 1), rand.Next(1, height - 1));
        }
    }
}
