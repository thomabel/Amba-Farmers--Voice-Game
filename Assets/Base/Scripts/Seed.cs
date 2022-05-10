using UnityEngine;

public class Seed : MonoBehaviour, IStorable, IInteractable
{
    public GameObject plant;

    void IStorable.Use()
    {
        Vector3 currentPostion = gameObject.transform.position;
        bool isTilled = GameObject.Find("Terrain").GetComponent<TerrainData>().GetTileData(currentPostion).tilled;
        bool isClear = CheckGoodSpacing();

        if (isClear && isTilled)
        {
            //Destroy(gameObject);
            Instantiate(plant, transform.position, Quaternion.identity, null);
        }

    }
    void IInteractable.Interact(GameObject with)
    {
    }

    private bool CheckGoodSpacing()
    {
        bool spaceIsClear = true;

        //Check if any colliders within radius of 0.5f
        Collider[] tooCloseColliders = Physics.OverlapSphere(gameObject.transform.position, 0.1f);
        foreach (var hitCollider in tooCloseColliders)
        {
            if (hitCollider.name == "Plant")
            {
                spaceIsClear = false;
            }
        }

        return spaceIsClear;
    }
}
