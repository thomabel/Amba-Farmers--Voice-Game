using UnityEngine;


public class TerrainData : MonoBehaviour
{
    public struct Data
    {
        public float water;
        public float nutrients;
        public bool tilled;
    }
    public Data[,] terrain_data;
    public float tile_size, land_size;
    private int array_size;
    
    public void Start()
    {
        array_size = Mathf.CeilToInt(land_size / tile_size);
        terrain_data = new Data[array_size,array_size];

        for (int i = 0; i < array_size; i++)
        {
            for (int j = 0; j < array_size; j++)
            {
                Data current = terrain_data[i,j];
                current = new Data();
                current.tilled = false;
                current.water = 0.0f;
                current.nutrients = 0.0f;
            }
        }

        GetTileData(new Vector3(0.0f, 0, 0.0f));
        GetTileData(new Vector3(0.4f, 0, -0.4f));
        GetTileData(new Vector3(-0.4f, 0, -0.4f));
        GetTileData(new Vector3(-0.4f, 0, 0.4f));
        GetTileData(new Vector3(0.4f, 0, 0.4f));
    }

    public Data GetTileData(Vector3 tile)
    {
        float offset = land_size / tile_size;
        offset = offset / 2;
        int i, k = 0;
        Data null_data = new Data();
        null_data.tilled = false;
        null_data.water = 0.0f;
        null_data.nutrients= 0.0f;

        i = Mathf.FloorToInt((tile.x / tile_size) + offset);
        k = Mathf.FloorToInt((tile.z / tile_size) + offset);

        if (i >= array_size || i < 0 || k >= array_size || k < 0)
        {
            Debug.Log("(" + 0 + "," + 0 + ")");
            return null_data;
        }
        else
        {
            Debug.Log("(" + i + "," + k + ")");

            return terrain_data[i, k];
        }
    }
}
