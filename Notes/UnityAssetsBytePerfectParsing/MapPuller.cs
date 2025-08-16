using System;
using System.IO;
using UnityEngine;


// Before using these scripts, ensure you have created a "Legacy Transparent Cutout" Material,
// and a sphere gameobject and assigned these to the Map Loader and Object Spawner scripts.
public class MapPuller : MonoBehaviour
{
    public string levelNumber;

    public string gameDirectory = "C:\\Program Files (x86)\\Collection Chamber\\Alien Trilogy\\";
    string levelPath1 = "";
    string levelPath2 = "";
    string levelPath3 = "";
    string levelPath4 = "";
    string levelPath5 = "";
    string levelPath6 = "";
    string levelPath7 = "";
    string fileDirectory;

    public static MapPuller finder;


    void Start()
    {
        if (finder == null)
        {
            finder = this;
        }
        else { Destroy(gameObject); }

    }

    public void CheckDirectory()
    {
        if (File.Exists(gameDirectory + "Run.exe")) { gameDirectory = gameDirectory + "HDD\\TRILOGY\\CD\\"; }
        else if (File.Exists(gameDirectory + "TRILOGY.EXE")) { gameDirectory = gameDirectory + "CD\\"; }
        levelPath1 = gameDirectory + "SECT11\\";
        levelPath2 = gameDirectory + "SECT12\\";
        levelPath3 = gameDirectory + "SECT21\\";
        levelPath4 = gameDirectory + "SECT22\\";
        levelPath5 = gameDirectory + "SECT31\\";
        levelPath6 = gameDirectory + "SECT32\\";
        levelPath7 = gameDirectory + "SECT90\\";
        fileDirectory = levelNumber.Substring(0, 2) switch
        {
            "11" or "12" or "13" => levelPath1,
            "14" or "15" or "16" => levelPath2,
            "21" or "22" or "23" => levelPath3,
            "24" or "26" => levelPath4,
            "31" or "32" or "33" => levelPath5,
            "35" or "36" or "37" or "38" or "39" => levelPath6,
            "90" => levelPath7,
            _ => throw new Exception("Unknown section selected!")
        };
    }

    [ContextMenu("Create Level and Spawn Map Only")]
    public void GeneratateMap()
    {
        if (levelNumber != "")
        {
            CheckDirectory();
            AlienTrilogyMapLoader.loader.Initiate(fileDirectory + "L" + levelNumber + "LEV.MAP", fileDirectory + levelNumber + "GFX.B16");
        }
        else { throw new Exception("Level number required in inspector"); }
    }

    [ContextMenu("Create Level and Generate Object Data")]
    public void GeneratateObjects()
    {
        if (levelNumber != "")
        {
            CheckDirectory();
            AlienTrilogyMapLoader.loader.Initiate(fileDirectory + "L" + levelNumber + "LEV.MAP", fileDirectory + levelNumber + "GFX.B16");
            ObjectSpawner.spawner.SpawnAll();
        }
        else { throw new Exception("Level number required in inspector"); }
    }
}