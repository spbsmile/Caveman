using Caveman.Setting;
using UnityEngine;
using Random = System.Random;

namespace Caveman.Level
{
    public class TileMap : MonoBehaviour
    {
        public int widthMap = 24;
        public int heightMap = 24;
        public int dotCount = 30;
        public int poolCount = 3;
        public Transform prefabDot;
        public Transform prefabPool;
        public Transform prefabGround;
        public bool multiplayer;

        public void Start()
        {
            var r = new Random();
            var tileSize = Instantiate(prefabGround).GetComponent<SpriteRenderer>().bounds.size;
            Settings.HeightMap = heightMap - (int) tileSize.x;
            var tilesOnWidth = widthMap/tileSize.x;
            var tilesOnHeight = heightMap/tileSize.y;

            for (var i = 0; i < tilesOnWidth; i++)
            {
                for (var j = 0; j < tilesOnHeight; j++)
                {
                    var gameObject = Instantiate(prefabGround);
                    gameObject.transform.SetParent(transform);
                    gameObject.transform.position = new Vector2(i*tileSize.x, j*tileSize.y);
                }
            }

            var deltaPoolBounds = (float)widthMap / (dotCount - dotCount / 3);
            for (var i = 0; i < dotCount; i++)
            {
                var gameObject = Instantiate(prefabDot);
                gameObject.transform.SetParent(transform);
                if (!multiplayer)
                {
                    gameObject.transform.position = new Vector2(r.Next(widthMap),
                        r.Next(heightMap));
                }
                else
                {
                    if (i < dotCount/3)
                    {
                        gameObject.transform.position = new Vector2(((float)widthMap/2 - i * deltaPoolBounds), i * deltaPoolBounds);                                
                    }
                    else
                    {
                        gameObject.transform.position = new Vector2((widthMap - (i - dotCount / 3 )* deltaPoolBounds), i * deltaPoolBounds);    
                    }
                }
            }

            for (var i = 0; i < poolCount; i++)
            {
                var gameObject = Instantiate(prefabPool);
                gameObject.transform.SetParent(transform);
                if (!multiplayer)
                {
                    gameObject.transform.position = new Vector2(r.Next(widthMap),
                    r.Next(heightMap));    
                }
                else
                {
                    gameObject.transform.position = new Vector2((widthMap / (poolCount*2)) * (i + 2), (heightMap / (poolCount*2)) * (i + 2));
                }
            }

            if (multiplayer)
            {
                var delta = (float)widthMap / (dotCount - dotCount / 1.5f);
                for (var i = 0; i < (dotCount - dotCount / 1.5f); i++)
                {
                    var gameObject = Instantiate(prefabDot);
                    gameObject.transform.SetParent(transform);
                    gameObject.transform.position = new Vector2(widthMap - (i *delta), i*delta);
                }
            }
        }
    }
}
