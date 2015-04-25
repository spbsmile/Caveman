using UnityEngine;
using Random = System.Random;

public class TileMap : MonoBehaviour
{
    public int widthTile = 15;
    public int heightTile = 15;

    private float tileSize = 1.5f;
    private const string TilePath = "Map/ground1";
    private const string PoolPath = "Map/pool";
    private const string DotPath = "Map/dot";
    private const int DotCount = 30;
    private const int PoolCount = 3;

    private float width;
    private float heigth;

    private Random random;

	public void Start ()
	{
        random = new Random();
	    width = widthTile*tileSize;
	    heigth = heightTile*tileSize;
        var positionStart = new Vector2(-width / 2, -heigth / 2);
	    for (var i = 0; i < widthTile; i++)
	    {
            for (var j = 0; j < heightTile; j++)
	        {
                var gameObject = Instantiate(Resources.Load(TilePath, typeof(GameObject)) as GameObject);
                gameObject.transform.SetParent(transform);
                gameObject.transform.position = positionStart + new Vector2(i * tileSize, j * tileSize);
	        }  
	    }

        for (var i = 0; i < DotCount; i++)
	    {
            var gameObject = Instantiate(Resources.Load(DotPath, typeof(GameObject)) as GameObject);
            gameObject.transform.position = new Vector2(random.Next((int)(-width / 2), (int)(width / 2)), random.Next((int)(-heigth / 2), (int)(heigth / 2)));
	    }

	    for (var i = 0; i < PoolCount; i++)
	    {
            var gameObject = Instantiate(Resources.Load(PoolPath, typeof(GameObject)) as GameObject);
            gameObject.transform.position = new Vector2(random.Next((int)(-width / 3), (int)(width / 3)), random.Next((int)(-heigth / 3), (int)(heigth / 3)));
	    }
	}
}
