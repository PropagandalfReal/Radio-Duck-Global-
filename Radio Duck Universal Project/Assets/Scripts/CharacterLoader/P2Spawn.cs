using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2Spawn : MonoBehaviour
{
    public CharacterDatabase characterDB;

    private GameObject Prefab;

    private GameObject spawnPoint;
    private GameObject newLocation;
    private GameObject NewPrefab;

    private int selectedOptionP2 = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("selectedOptionP2"))
        {
            selectedOptionP2 = 0;
        }
        else
        {
            Load();
        }
        UpdateCharacter(selectedOptionP2);
    }

    void Update()
    {
        spawnPoint = this.gameObject;
        newLocation = NewPrefab;
        spawnPoint.transform.position = newLocation.transform.Find("CameraTrack").gameObject.transform.position;
    }

    private void UpdateCharacter(int selectedOptionP2)
    {
        Character character = characterDB.GetCharacter(selectedOptionP2);
        GameObject Prefab = character.Prefab;
        NewPrefab = Instantiate(Prefab, transform.position, Quaternion.identity);

        Debug.Log("P2: " + selectedOptionP2);
    }

    private void Load()
    {
        selectedOptionP2 = PlayerPrefs.GetInt("selectedOptionP2");
    }
}
