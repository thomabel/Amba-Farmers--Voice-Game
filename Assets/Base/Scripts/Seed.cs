using UnityEngine;

public class Seed : MonoBehaviour, IStorable, IInteractable
{
    public GameObject plantToGrow;

    public GameObject plantCorn;
    public GameObject plantTomato;
    public GameObject plantOnion;

    void IStorable.Use()
    {
        PlantSeed();
    }
    void IInteractable.Interact(GameObject with)
    {
    }

    private bool SeedToPlant(Base.GoodType seedIn)
    {
        switch (seedIn)
        {
            case Base.GoodType.Seed_Corn:
                {
                    plantToGrow = plantCorn;
                    return true;
                }
            case Base.GoodType.Seed_Onion:
                {
                    plantToGrow = plantOnion;
                    return true;
                }
            case Base.GoodType.Seed_Tomato:
                {
                    plantToGrow = plantTomato;
                    return true;
                }
            default:
                {
                    Debug.Log("No case made for this seed type");
                    return false;
                }
        }
    }

    private bool CheckGoodSpacing()
    {
        bool spaceIsClear = true;

        //Check if any colliders within radius of 0.5f
        Collider[] tooCloseColliders = Physics.OverlapSphere(gameObject.transform.position, 0.1f);
        foreach (var hitCollider in tooCloseColliders)
        {
            if (hitCollider.GetComponent<Plant>() != null)
            {
                spaceIsClear = false;
            }
        }

        return spaceIsClear;
    }

    private void PlantSeed()
    {
        Vector3 currentPostion = gameObject.transform.position;
        bool isTilled = GameObject.Find("Terrain").GetComponent<TerrainData>().GetTileData(currentPostion).tilled;
        bool isClear = CheckGoodSpacing();

        if (isClear)
        {
            if (SeedToPlant(GetComponent<TypeLabel>().Type))
            {
                Instantiate(plantToGrow, transform.position, Quaternion.identity, null);
                Equipment equipment = GameObject.Find("Player").GetComponent<Equipment>();
                --equipment.eitem.quantity;
                if(equipment.eitem.quantity == 0)
                {
                    equipment.eitem = null;
                    equipment.Item = null;
                }
            }
        }
    }
}
