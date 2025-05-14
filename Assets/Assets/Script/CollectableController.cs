using UnityEngine;

public class CollectableController : MonoBehaviour
{

    public bool gemCollected = false;
    public bool coffinCollected = false;
    public GameObject gem;
    public GameObject coffin;
    public GameObject openDoor;

    public void collectObject(GameObject collectedObject, string objectType)
    {
        if (objectType == "Gem")
        {
            gemCollected = true;
            gem.SetActive(false); 
            coffin.SetActive(false);
            openDoor.SetActive(true);
        }
        else if (objectType == "Coffin")
        {
            coffinCollected = true;
            gem.SetActive(false); 
            coffin.SetActive(false);
            openDoor.SetActive(true);
        } else {
             collectedObject.SetActive(false); // Desactiva el objeto recogido
        }
    }
}
