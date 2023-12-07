using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public MapDatabase MapDB;

    public int buildIndex;
    public TextMeshProUGUI nameText;
    public SpriteRenderer artworkSprite;

    private int selectedMap = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("selectedMap")){
            selectedMap = 0;
        }
        else{
            Load();
        }
        UpdateMap(selectedMap);
    }

    public void NextOption(){
        selectedMap++;

        if(selectedMap >= MapDB.MapCount){
            selectedMap = 0;
        }

        UpdateMap(selectedMap);
        Save();
    }

    public void BackOption(){
        selectedMap--;

        if(selectedMap < 0){
            selectedMap = MapDB.MapCount - 1;
        }

        UpdateMap(selectedMap);
        Save();
    }


    private void UpdateMap(int selectedMap){
        Map map = MapDB.GetMap(selectedMap);
        artworkSprite.sprite = map.mapSprite;
        nameText.text = map.mapName;
        buildIndex = map.buildIndex;
    }

    private void Load(){
        selectedMap = PlayerPrefs.GetInt("selectedMap");
    }

    private void Save(){
        PlayerPrefs.SetInt("selectedMap", selectedMap);
    }

    public void ChangeScene(){
        SceneManager.LoadScene(buildIndex);
    }
}
