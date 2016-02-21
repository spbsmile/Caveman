using System;
using System.Collections;
using Caveman.Setting;
using Caveman.Utils;
using UnityEngine;

namespace Caveman.Pools
{
    public class PoolManager
    {
        /*
                private IEnumerator PutBonuses()
                {
                    var bound = Settings.BonusSpeedMaxCount - poolBonusesSpeed.GetActivedCount;
                    for (var i = 0; i < bound; i++)
                    {
                        PutItem(poolBonusesSpeed);
                    }
                    yield return new WaitForSeconds(Settings.BonusTimeRespawn);
                    StartCoroutine(PutBonuses());
                }

                private IEnumerator PutWeapons()
                {
                    for (var i = 0; i < Settings.WeaponInitialLying; i++)
                    {
                        PutItem(poolStones);
                    }
                    for (var i = 0; i < Settings.CountLyingSkulls; i++)
                    {
                        PutItem(poolSkulls);
                    }
                    yield return new WaitForSeconds(Settings.WeaponTimeRespawn);
                    StartCoroutine(PutWeapons());
                }

                private void PutItem<T>(ObjectPool<T> pool) where T : MonoBehaviour
                {
                    var item = pool.New();
                    StartCoroutine(UnityExtensions.FadeIn(item.GetComponent<SpriteRenderer>()));
                    item.transform.position = new Vector2(r.Next(1, Settings.WidthMap - 1), r.Next(1, Settings.HeightMap - 1));
                }

                /// <summary>
                /// Used object pool pattern
                /// </summary>
                private ObjectPool<T> CreatePool<T>(int initialBufferSize, Transform container, T prefab,
                    Action<GameObject, ObjectPool<T>> init) where T : MonoBehaviour
                {
                    var pool = container.GetComponent<ObjectPool<T>>();
                    pool.CreatePool(prefab, initialBufferSize, serverNotify != null);
                    for (var i = 0; i < initialBufferSize; i++)
                    {
                        var item = Instantiate(prefab);
                        if (init != null)
                        {
                            init(item.gameObject, pool);
                        }
                        item.transform.SetParent(container);
                        pool.Store(item);
                    }
                    return pool;
                } */

    }
}
