using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    public Vector3 playerPos;
    public Vector3 playerRot;

    public List<int> invenArrayNumber = new List<int>();
    public List<string> invenItemName = new List<string>();
    public List<int> invenItemCount = new List<int>();
}


public class SaveAndLoad : MonoBehaviour
{
    private SaveData saveData = new SaveData();

    private string SAVE_DATA_DIRECTORY;
    private string SAVE_FILE_NAME = "/SaveFile.text";

    private PlayerController thePlayerController;
    private Inventory theInventory;

    private void Start()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves/";

        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
        {
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);
        }
    }

    //JSON을 이용해서 데이터를 저장하고 불러온다.
    public void SaveData()
    {
        thePlayerController = FindObjectOfType<PlayerController>();
        theInventory = FindObjectOfType<Inventory>();

        saveData.playerPos = thePlayerController.transform.position;
        saveData.playerPos = thePlayerController.transform.eulerAngles;

        Slot[] slots = theInventory.GetSlots();
        for (int i = 0; i < slots.Length; i++)
        {
            if(slots[i].item != null)
            {
                saveData.invenArrayNumber.Add(i);
                saveData.invenItemName.Add(slots[i].item.itemName);
                saveData.invenItemCount.Add(slots[i].itemCount);
            }
        }

        //최근 유니티에 추가되었다고 한다.
        string json = JsonUtility.ToJson(saveData);

        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILE_NAME, json);
        Debug.Log("저장 완료");
        Debug.Log(json);
    }

    public void LoadData()
    {
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILE_NAME))
        {
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILE_NAME);

            //제이슨을 풀어서 세이브 데이터로 다시집어넣는다?
            saveData = JsonUtility.FromJson<SaveData>(loadJson);


            thePlayerController = FindObjectOfType<PlayerController>();
            theInventory = FindObjectOfType<Inventory>();


            thePlayerController.transform.position = saveData.playerPos;
            thePlayerController.transform.eulerAngles = saveData.playerRot;

            for (int i = 0; i < saveData.invenItemName.Count; i++)
            {
                theInventory.LoadToInven(saveData.invenArrayNumber[i]
                                        , saveData.invenItemName[i]
                                        , saveData.invenItemCount[i]);
            }
            Debug.Log("로드 발생");
        }
        else
        {
            Debug.Log("세이브파일이 없습니다.");
        }
    }

}
