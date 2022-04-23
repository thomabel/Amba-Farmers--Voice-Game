using UnityEngine;


public class TerrainData : MonoBehaviour
{
    //all variables
    public struct Data
    {
        public bool tilled;
        public float water;
        public float nutrients;

        public Data(bool new_tilled, float new_water, float new_nutrients)
        {
            water = new_water;
            nutrients = new_nutrients;
            tilled = new_tilled;
        }
    }
    public Data[,] terrain_data;
    public float tile_size, land_size;
    private int array_size;


    //runs when object is first created
    //creates a 2d array that corresponds to terrain that has been divided into tiles
    //each element in array holds a tile's data
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
        SetTileData(new Vector3(0.4f, 0, 0.4f), new Data(true, 2f, 3f));
        GetTileData(new Vector3(0.4f, 0, 0.4f));
    }


    //returns the corresponding tile's data given a vector3 position
    public Data GetTileData(Vector3 tile)
    {
        (int i, int k) = GetTile(tile);
        Data null_data = new Data(false, 0f, 0f);

        if (i >= array_size || i < 0 || k >= array_size || k < 0)
        {
            Debug.Log("[" + 0 + "," + 0 + "] : (0,0,0)");
            return null_data;
        }
        else
        {
            Data tile_data = terrain_data[i,k];
            int tilled = tile_data.tilled ? 1 : 0;
            Debug.Log("[" + i + "," + k + "] : (" + tilled + "," + tile_data.water + "," + tile_data.nutrients + ")");

            return terrain_data[i, k];
        }
    }


    //sets the data of a corresponding tile for a given vector3 position
    public void SetTileData(Vector3 tile, Data new_data)
    {
        (int i, int k) = GetTile(tile);

        if (i < array_size && i >= 0 && k < array_size && k >= 0)
        {
            int tilled = new_data.tilled ? 1 : 0;
            terrain_data[i, k] = new Data(new_data.tilled, new_data.water, new_data.nutrients);
            Data tile_data = terrain_data[i, k];

            Debug.Log("[" + i + "," + k + "] : (" + tilled + "," + tile_data.water + "," + tile_data.nutrients + ")");
        }
    }


    //get corresponding array indices of given vector3 position
    //does not check if indices are out of bounds
    private (int, int) GetTile(Vector3 tile)
    {
        float offset = land_size / tile_size;
        offset = offset / 2;
        int i, k = 0;

        i = Mathf.FloorToInt((tile.x / tile_size) + offset);
        k = Mathf.FloorToInt((tile.z / tile_size) + offset);

        return (i, k);
    }
}
