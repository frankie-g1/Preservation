using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEditor;

public class SaveManager : MonoBehaviour
{

    public static SaveManager instance;
    private GameManager gm;

    private void Awake()
    {
        if (instance == null) {
            instance = this;
        }
        gm = GameObject.FindObjectOfType<GameManager>();

        
    }
        
    public void save(string ID, Data data)
    {
        //Create or open file to save to.
        FileStream file = new FileStream(Application.persistentDataPath + "/Save" + ID + ".dat", FileMode.OpenOrCreate);

        try
        {
            //Binary Formatter to allow writing data to the file.
            BinaryFormatter formatter = new BinaryFormatter();
            //Serialization method to write to the file.
            formatter.Serialize(file, data);
        }
        catch (SerializationException e)
        {
            Debug.Log("Failed to Serialize Data: " + e.Message);
        }
        finally
        {
            file.Close();
        }
    }

    public void save(string ID)
    {
        save(ID, gm.currData);
    }

    public void save() {
        save(gm.currData.saveID);
    }

    public static void save_Static() {
        instance.save();
    }

    public static void save_Static(string ID)
    {
        instance.save(ID);
    }

    public static void save_Static(string ID, Data data)
    {
        instance.save(ID, data); ;
    }

    public void load(string ID)
    {
        FileStream file = new FileStream(Application.persistentDataPath + "/Save" + ID + ".dat", FileMode.Open);

        try
        {
            
            BinaryFormatter formatter = new BinaryFormatter();
            gm.currData = (Data)formatter.Deserialize(file);
            
        }
        catch (SerializationException e)
        {
            Debug.Log("Failed to Load Data: " + e.Message);
        }
        finally {
            file.Close();
        }
        
    }

    public static bool doesExist(string ID) {
        string filePath = Application.persistentDataPath + "/Save" + ID + ".dat";
        if (System.IO.File.Exists(filePath)) {
            return true;
        }
        return false;
    }

    public static bool doesAnyExist() {
        return (allSaveIDs_Static().Length != 0);
    }

    public string[] allSaveIDs() {
        
        List<string> idList = new List<string>();
        string directory = Application.persistentDataPath;
        List<string> fileList = new List<string>(Directory.GetFiles(directory));
        for (int i = 0; i < fileList.Count; i++) {
            string fileName = Path.GetFileName(fileList[i]);
            if (fileName.Substring(fileName.Length - 4).Equals(".dat") && fileName.Substring(0,4).Equals("Save")) {
                idList.Add(fileName.Substring(4,fileName.Length-8));
            }
        }
        return idList.ToArray();
    }

    public static string[] allSaveIDs_Static() {
        return instance.allSaveIDs();
    }
    public static void load_Static(string ID)
    {
        instance.load(ID);
    }

    public void saveDelete(string ID)
    {
        if(doesExist(ID))
            File.Delete(Application.persistentDataPath + "/Save" + ID + ".dat");
        AssetDatabase.Refresh();
    }

    public void saveDelete()
    {
        saveDelete(gm.currData.saveID);
    }

    public static void saveDelete_Static(string saveID)
    {
        instance.saveDelete(saveID);
    }

    public static void saveDelete_Static()
    {
        instance.saveDelete();
    }
}
