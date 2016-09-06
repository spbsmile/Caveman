using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Caveman.Bonuses;
using Caveman.Configs;
using Caveman.Pools;
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

        public void CreateTerrain(Random rand, string pathPrefabTile, IEnumerable<MapConfig.Artefacts> artefacts, int width, int height, bool isMultiplayer)
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

        public void StartItemPeriodicals(IEnumerable<MapConfig.ItemPeriodical> configItemPeriodicals)
        {
            foreach (var config in configItemPeriodicals)
            {
                if (config.Type == "weapon")
                {
                    StartCoroutine(
                        PutItems<WeaponModelBase>(EnterPoint.CurrentSettings.WeaponsConfigs[config.Name].PrefabPath,
                            config.Period, config.Count
                        ));
                }
                else if (config.Type == "bonus")
                {
                    StartCoroutine(PutItems<BonusBase>(
                        EnterPoint.CurrentSettings.BonusesConfigs[config.Name].PrefabPath, config.Period, config.Count));
                }
            }
        }

        private IEnumerator PutItems<T>(string poolId, int period, int count) where T : MonoBehaviour
        {
            yield return new WaitForSeconds(period);
            //todo length
            // todo var bound = Settings.BonusSpeedMaxCount - PoolsManager.instance.BonusesSpeed.GetActivedCount; 
            for (var i = 0; i < count; i++)
            {
                PutItem((ObjectPool<T>)PoolsManager.instance.Pools[poolId]);
            }
            StartCoroutine(PutItems<T>(poolId, period, count));
        }

        private void PutItem<T>(ObjectPool<T> pool) where T : MonoBehaviour
        {
            var item = pool.New();
            StartCoroutine(UnityExtensions.FadeIn(item.GetComponent<SpriteRenderer>()));
            item.transform.position = new Vector2(rand.Next(1, width - 1), rand.Next(1, height - 1));
        }
    }
}
