using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1Spawn : MonoBehaviour
{
    public CharacterDatabase characterDB;

    private GameObject Prefab;

    private GameObject spawnPoint;
    private GameObject newLocation;
    private GameObject NewPrefab;

    private int selectedOptionP1 = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("selectedOptionP1"))
        {
            selectedOptionP1 = 0;
        }
        else
        {
            Load();
        }
        UpdateCharacter(selectedOptionP1);
    }

    void Update()
    {
        spawnPoint = this.gameObject;
        newLocation = NewPrefab;
        spawnPoint.transform.position = newLocation.transform.position;
    }

    private void UpdateCharacter(int selectedOptionP1)
    {
        Character character = characterDB.GetCharacter(selectedOptionP1);
        GameObject Prefab = character.Prefab;
        NewPrefab = Instantiate(Prefab, transform.position, Quaternion.identity);

        Debug.Log("P1: " + selectedOptionP1);
    }

    private void Load()
    {
        selectedOptionP1 = PlayerPrefs.GetInt("selectedOptionP1");
    }
}