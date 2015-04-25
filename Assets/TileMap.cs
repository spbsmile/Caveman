using UnityEngine;

public class TileMap : MonoBehaviour
{
    public int width = 15;
    public int height = 15;

    private float tileSize = 1.5f;

    private const string TileName = "Map/ground1";

	public void Start ()
	{
	    for (var i = 0; i < width; i++)
	    {
            for (var j = 0; j < height; j++)
	        {
                var gameObject = Instantiate(Resources.Load(TileName, typeof(GameObject)) as GameObject);
                gameObject.transform.SetParent(transform    );
                gameObject.transform.position = new Vector2(i * tileSize, j * tileSize);

	        }  
	    }
        




	}
	
	public void Update () 
    {
	
	}
}
