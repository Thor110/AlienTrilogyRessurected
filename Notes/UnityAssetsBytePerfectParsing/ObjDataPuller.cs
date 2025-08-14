using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Diagnostics.Contracts;

public class BndSection
{
    public string Name { get; set; } = "";
    public byte[] Data { get; set; } = Array.Empty<byte>();
}

[System.Serializable] // Makes the class visible in the Inspector
public class CollisionNode
{
    public string Name;
    public GameObject obj;
    public int unknown1;
    public int unknown2;
    public int unknown3;
    public int unknown4;
    public int unknown5;
    public int unknown6;
    public int unknown7;
    public int unknown8;
    public int ceilingFog;
    public int floorFog;
    public int ceilingHeight;
    public int floorHeight;
    public int unknown13;
    public int unknown14;
    public int Lighting;
    public int Action;
}

[System.Serializable] // Makes the class visible in the Inspector
public class PathNode
{
    public string Name;
    public GameObject obj;
    public int X;
    public int Y;
    public int unused;
    public int AlternateNode;
    public int NodeA;
    public int NodeB;
    public int NodeC;
    public int NodeD;
}

[System.Serializable] // Makes the class visible in the Inspector
public class Monster
{
    public string Name;
    public GameObject spawnedObj;
    public int Type;
    public int X;
    public int Y;
    public int Z;
    public int Rotation;
	public int Health;
	public int Drop;
    public int unknown2;
    public int Difficulty;
    public int unknown4;
    public int unknown5;
    public int unknown6;  
    public int unknown7;
    public int unknown8;
    public int Speed;
    public int unknown9;
    public int unknown10;
    public int unknown11;
    public int unknown12;
    public int unknown13;
}

[System.Serializable] // Makes the class visible in the Inspector
public class Crate
{
    public string Name;
    public GameObject spawnedObject;
    public int X;
    public int Y;
    public int Type;
    public int Drop;
	public int unknown1;
    public int unknown2;
    public int Drop1;
    public int Drop2;
    public int unknown3;
    public int unknown4;
    public int unknown5;
    public int unknown6;
    public int unknown7;
    public int unknown8;
    public int Rotation;
    public int unknown10;
}

[System.Serializable] // Makes the class visible in the Inspector
public class Pickup
{
    public string Name;
    public GameObject spawnedObject;
    public int X;
    public int Y;
    public int Type;
    public int Amount;
    public int Multiplier;
    public int unknown1;
    public int Z;
    public int unknown2;
}

[System.Serializable] // Makes the class visible in the Inspector
public class Door
{
    public string name;
    public GameObject spawnedObject;
    public int X;
    public int Y;
    public int unknown;
    public int Time;
    public int Tag;
    public int unknown2;
    public int Rotation;
    public int Index;
}

[System.Serializable] // Makes the class visible in the Inspector
public class Lifts
{
    public string Name;
    public GameObject spawnedObject;
    public byte X;
    public byte Y;
    public byte Z;
    public byte unknown1;
    public byte unknown2;
    public byte unknown3;
    public byte unknown4;
    public byte unknown5;
    public byte unknown6;
    public byte unknown7;
    public byte unknown8;
    public byte unknown9;
    public byte unknown10;
    public byte unknown11;
    public byte unknown12;
    public byte unknown13;
}

public class ObjDataPuller : MonoBehaviour
{
    [Header("Settings")]
    public bool generateCSV;

    [Header("Object Lists")]
	public List<PathNode> pathNodes = new();
	public List<CollisionNode> collisions = new();
	public List<Monster> monsters = new();
	public List<Crate> boxes = new();
	public List<Pickup> pickups = new();
	public List<Door> doors = new();
	public List<Lifts> lifts = new();
    //[Header("paths")]
    private string levelPath = ""; // path to the .MAP file

    [Header("Map Strings")]
    public string unknownMapBytes1;
    public string unknownMapBytes2;
    public string unknownMapBytes3;
    //Map strings
    public string mapLengthString, mapWidthString, playerStartXString, playerStartYString, monsterCountString, pickupCountString, boxCountString, doorCountString, playerStartAngleString, liftCountString;


    public Byte[] remainderBytes;

    public static ObjDataPuller objectPuller;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(objectPuller == null)
        {
            objectPuller = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [ContextMenu("Pull Object Data")]
    // Update is called once per frame
    public void Initiate(string levelToLoad)
    {
        levelPath = levelToLoad;
        byte[] mapFileData = File.ReadAllBytes(levelPath);
        Debug.Log("Map geometry bytes: " + mapFileData.Length);
        GetData(mapFileData);
    }

    public void GetData(byte[] data)
    {
        // Read MAP0 section
        using var br = new BinaryReader(new MemoryStream(LoadSection(File.ReadAllBytes(levelPath), "MAP0")[0].Data));
        // Read number of vertices
        ushort vertCount = br.ReadUInt16();             // vertex count
        Debug.Log("Number of vertices: " + vertCount);  // display vertex count
        ushort quadCount = br.ReadUInt16();             // quad count
        Debug.Log("Number of quads: " + quadCount);     // display quad count
        ushort mapLength = br.ReadUInt16();             // map length
        mapLengthString = mapLength.ToString();         // display map length
        ushort mapWidth = br.ReadUInt16();              // map width
        mapWidthString = mapWidth.ToString();           // display map width
        ushort playerStartX = br.ReadUInt16();          // player start X coordinate
        playerStartXString = playerStartX.ToString();   // display player start X coordinate
        ushort playerStartY = br.ReadUInt16();          // player start Y coordinate
        playerStartYString = playerStartY.ToString();   // display player start Y coordinate
        byte pathCount = br.ReadByte();                 // path count
        br.ReadByte();                                  // UNKNOWN 0 ( unused? 128 on all levels ) - possibly lighting related             
        ushort monsterCount = br.ReadUInt16();          // monster count
        monsterCountString = monsterCount.ToString();   // display monster count
        ushort pickupCount = br.ReadUInt16();           // pickup count
        pickupCountString = pickupCount.ToString();     // display pickup count
        ushort boxCount = br.ReadUInt16();              // object count
        boxCountString = boxCount.ToString();           // display object count
        ushort doorCount = br.ReadUInt16();             // door count
        doorCountString = doorCount.ToString();         // display door count
        ushort liftCount = br.ReadUInt16();             // lift count
        liftCountString = liftCount.ToString();         // display lift count
        ushort playerStart = br.ReadUInt16();           // player start angle
        playerStartAngleString = playerStart.ToString();// display player start angle
        // unknown bytes
        ushort unknown1 = br.ReadUInt16();              // unknown 1
        unknownMapBytes1 = unknown1.ToString();         // display unknown 1
        br.ReadBytes(2);                                // always 0x4040    ( unknown )
        ushort unknown2 = br.ReadUInt16();              // unknown2
        unknownMapBytes2 = unknown2.ToString();         // display unknown 1
        ushort enemyTypes = br.ReadUInt16();            // Available Enemy Types
        // Chapter 1 ( enemyTypes ) - 16/17/18/19 are likely not a part of the enemyTypes ( example : first two levels )
        // L111LEV - 22 00 // 2 / 6
        // L112LEV - 22 00 // 2 / 6 / 16
        // L113LEV - 00 00 // null
        // L122LEV - 22 04 // 2 / 6 / 11 / 16
        // L131LEV - 26 04 // 2 / 6 / 11 / 3
        // L114LEV - 00 00 // null
        // L141LEV - 23 00 // 6 / 1 / 2
        // L115LEV - 00 00 // null
        // L154LEV - 23 10 // 6 / 1 / 2 / 13 / 16 / 17 / 19
        // L155LEV - 0C 00 // 18 / 16 / 19
        // L161LEV - A7 02 // 6 / 1 / 2 / 8 / 10 / 3
        // L162LEV - 43 00 // 7 / 1 / 2
        // Chapter 2 ( enemyTypes )
        // L211LEV - 0E 08 // 4 / 12 / 2 / 3
        // L212LEV - 0A 08 // 4 / 2 / 12
        // L213LEV - 02 08 // 2 / 12
        // L222LEV - 0B 08 // 1 / 2 / 12 / 4 / 17 / 19
        // L242LEV - 00 00 // null
        // L231LEV - 14 21 // 14 / 5 / 3 / 9
        // L232LEV - 13 10 // 1 / 2 / 5 / 13 / 16 / 17
        // L243LEV - 00 00 // null
        // L262LEV - 17 02 // 5 / 1 / 2 / 10 / 3
        // L263LEV - 43 00 // 7 / 1 / 2
        // Chapter 3 ( enemyTypes )
        // L311LEV - 10 20 // 5 / 14
        // L321LEV - 02 00 // 2
        // L331LEV - 12 21 // 5 / 14 / 2 / 9
        // L322LEV - 08 00 // 4
        // L351LEV - 24 12 // 6 / 13 / 10 / 3
        // L352LEV - 00 00 // null
        // L323LEV - 10 00 // 5
        // L371LEV - 20 10 // 6 / 13
        // L353LEV - 00 00 // null
        // L324LEV - 22 00 // 2 / 6
        // L381LEV - 23 00 // 6 / 1 / 2
        // L325LEV - 36 00 // 3 / 5 / 2 / 6
        // L391LEV - 43 00 // 7 / 1 / 2
        // Multiplayer Levels ( enemyTypes )
        // All = 00 10
        ushort unknown3 = br.ReadUInt16();              // unknown3
        unknownMapBytes3 = unknown3.ToString();         // display unknown 1
        // vertice formula - multiply the value of these two bytes by 8 - (6 bytes for 3 points + 2 bytes zeros)
        br.BaseStream.Seek(vertCount * 8, SeekOrigin.Current);
        // quad formula - the value of these 2 bytes multiply by 20 - (16 bytes dot indices and 4 bytes info)
        br.BaseStream.Seek(quadCount * 20, SeekOrigin.Current);
        int collisionBlockCount = mapLength * mapWidth;
        // collision block formula = multiply length by width - (16 bytes per collision node.)
        for (int i = 0; i < collisionBlockCount; i++)
        {
            CollisionNode node = new CollisionNode
            {
                unknown1 = br.ReadByte(),       // all values exist from 0-255                      ( 256 possible values )
                unknown2 = br.ReadByte(),       // all values exist from 0-55 and 255               ( 57 possible values )
                unknown3 = br.ReadByte(),       // only ever 255 or 0 across all levels in the game ( 255 = wall / 0 == traversable )
                unknown4 = br.ReadByte(),       // only ever 255 or 0 across all levels in the game ( 255 = wall / 0 == traversable )
                unknown5 = br.ReadByte(),       // only ever 0 across every level in the game       ( 1 possible value )
                unknown6 = br.ReadByte(),       // only ever 0-21 across every level in the game    ( 22 possible values )
                unknown7 = br.ReadByte(),       // only ever 0-95 across every level in the game    ( 96 possible values )
                unknown8 = br.ReadByte(),       // only ever 0 across every level in the game       ( 1 possible value )
                ceilingFog = br.ReadByte(),     // a range of different values                      ( 43 possible values )
                floorFog = br.ReadByte(),       // a range of different values                      ( 40 possible values )
                ceilingHeight = br.ReadByte(),  // a range of different values                      ( 30 possible values )
                floorHeight = br.ReadByte(),    // a range of different values                      ( 206 possible values )
                unknown13 = br.ReadByte(),      // a range of different values                      ( 26 possible values )
                unknown14 = br.ReadByte(),      // a range of different values                      ( 167 possible values )
                Lighting = br.ReadByte(),       // a range of different values                      ( 120 possible values )
                Action = br.ReadByte(),         // only ever 0-41 across every level in the game    ( 42 possible values )
                // action
                // 0 - nothing space
                // 1 - starting door open space
                // 2 - door open space 0
                // 3 - switch open space 1
                // 4 - door open space 1
                // 5 - switch open space 0
                // 6 - unknown square space
                // 7 - door open space 3
                // 8 - battery switch 1
                // 9 - end level space
                // 10 - possibly stairs?
                // 11 - unknown lines
                // 12 - barricade line
                // 13 - door or door open space?
                // 14-41 - not used in level 1
                Name = "Collision Node " + i
            };
            collisions.Add(node);
            if (generateCSV)
            {
                string filePath = Application.dataPath + "/" + MapFinder.finder.levelNumber + "ExportedData.csv";
                ExportToCSV(collisions, filePath);
                Debug.Log("CSV file exported to: " + filePath);
            }
        }
        // path node formula = number of elements multiplied by 8 - (8 bytes per path node)
        for (int i = 0; i < pathCount; i++)
        {
            PathNode obj = new PathNode
            {
                X = br.ReadByte(),              // x coordinate of the pathing object
                Y = br.ReadByte(),              // y coordinate of the pathing object
                unused = br.ReadByte(),         // only ever 0 across every level in the game
                AlternateNode = br.ReadByte(),  // alternate node of the pathing object for special behaviours
                NodeA = br.ReadByte(),          // node A of the pathing object
                NodeB = br.ReadByte(),          // node B of the pathing object
                NodeC = br.ReadByte(),          // node C of the pathing object
                NodeD = br.ReadByte(),          // node D of the pathing object
            };
            pathNodes.Add(obj);
        }
        // monster formula = number of elements multiplied by 20 - (20 bytes per monster)
        for (int i = 0; i < monsterCount; i++)
        {
            Monster monster = new Monster
            {
                Type = br.ReadByte(),           // type of the monster
                // Monster Types (0x)
                // 01 - Egg
                // 02 - Face Hugger
                // 03 - Chest Burster
                // 04 - Bambi
                // 05 - Dog Alien
                // 06 - Warrior Drone
                // 07 - Queen
                // 08 - Ceiling Warrior Drone
                // 09 - Ceiling Dog Alien
                // 0A - Colonist
                // 0B - Guard
                // 0C - Soldier
                // 0D - Synthetic
                // 0E - Handler
                // 0F - Value not used in any level ( possibly the player )
                // 10 - Horizontal Steam Vent
                // 11 - Horizontal Flame Vent
                // 12 - Vertical Steam Vent
                // 13 - Vertical Flame Vent
                X = br.ReadByte(),              // x coordinate of the monster
                Y = br.ReadByte(),              // y coordinate of the monster
                Z = br.ReadByte(),              // z coordinate of the monster
                Rotation = br.ReadByte(),       // Byte Direction  Facing
                                                // 00 - North       // Y+
                                                // 01 - North East  // X+ Y+
                                                // 02 - East        // X+
                                                // 03 - South East  // X+ Y-
                                                // 04 - South       // Y-
                                                // 05 - South West  // X- Y-
                                                // 06 - West        // X-
                                                // 07 - North West  // X- Y+
                Health = br.ReadByte(),         // health of the monster
                Drop = br.ReadByte(),           // index of object to be dropped
                unknown2 = br.ReadByte(),       // UNKNOWN
                Difficulty = br.ReadByte(),     // 0 - Easy, 1 - Medium, 2 - Hard
                unknown4 = br.ReadByte(),       // UNKNOWN
                unknown5 = br.ReadByte(),       // UNKNOWN
                unknown6 = br.ReadByte(),       // UNKNOWN
                unknown7 = br.ReadByte(),       // UNKNOWN
                unknown8 = br.ReadByte(),       // UNKNOWN
                Speed = br.ReadByte(),          // speed of the monster
                unknown9 = br.ReadByte(),       // only ever 0 across every level in the game
                unknown10 = br.ReadByte(),      // UNKNOWN
                unknown11 = br.ReadByte(),      // UNKNOWN
                unknown12 = br.ReadByte(),      // UNKNOWN
                unknown13 = br.ReadByte(),      // UNKNOWN
                Name = "Monster Spawn " + i,
            };
            monsters.Add(monster);
        }
        // pickup formula = number of elements multiplied by 8 - (8 bytes per pickup)
        for (int i = 0; i < pickupCount; i++)
        {
            Pickup pickup = new Pickup
            {
                X = br.ReadByte(),              // x coordinate of the pickup
                Y = br.ReadByte(),              // y coordinate of the pickup
                Type = br.ReadByte(),           // pickup type
                // Pickup Types (0x)
                // 00 - Pistol
                // 01 - Shotgun
                // 02 - Pulse Rifle
                // 03 - Flame Thrower
                // 04 - Smartgun
                // 05 - Nothing / Unused
                // 06 - Seismic Charge
                // 07 - Battery
                // 08 - Night Vision Goggles
                // 09 - Pistol Clip
                // 0A - Shotgun Cartridge
                // 0B - Pulse Rifle Clip
                // 0C - Grenades
                // 0D - Flamethrower Fuel
                // 0E - Smartgun Ammunition
                // 0F - Identity Tag
                // 10 - Auto Mapper
                // 11 - Hypo Pack
                // 12 - Acid Vest
                // 13 - Body Suit
                // 14 - Medi Kit
                // 15 - Derm Patch
                // 16 - Protective Boots
                // 17 - Adrenaline Burst
                // 18 - Derm Patch
                // 19 - Shoulder Lamp
                // 1A - Shotgun Cartridge       ( Cannot be picked up )
                // 1B - Grenades                ( Cannot be picked up )
                Amount = br.ReadByte(),         // amount of the pickup
                Multiplier = br.ReadByte(),     // multiplier for the pickup
                unknown1 = br.ReadByte(),       // only ever 0 across every level in the game
                Z = br.ReadByte(),              // only ever 0 or 1 across every level in the game
                unknown2 = br.ReadByte(),       // UNKNOWN - unk2 is always the same as amount for ammunition
                Name = "Pickup " + i,
            };
            pickups.Add(pickup);
        }
        // boxes formula = number of elements multiplied by 16 - (16 bytes per box)
        for (int i = 0; i < boxCount; i++)
        {
            Crate box = new Crate
            {
                X = br.ReadByte(),
                Y = br.ReadByte(),
                Type = br.ReadByte(),
                // My Object Types (int) - indented = unused across all levels                                          Object Status       Minimum Damage  Drop
                // // // less than 20 - a box that cannot be blown up                                                   [INDESTRUCTIBLE]
                // 20 - a regular box that can be blown up ( or an egg husk if in chapter 3 )                           [DESTRUCTIBLE]      PISTOL          YES
                // 21 - destructible walls                                                                              [DESTRUCTIBLE]      GRENADE         YES
                // 22 - another small switch, the difference is at the bottom of the model ( lightning is drawn )       [INDESTRUCTIBLE]
                // 23 - barrel explodes.                                                                                [DESTRUCTIBLE]      SHOTGUN         NO
                // 24 - switch with animation ( small switch )                                                          [INDESTRUCTIBLE]
                // 25 - double stacked boxes ( two boxes on top of each other that can be blown up )                    [DESTRUCTIBLE]      PISTOL          YES
                // 26 - wide switch with zipper                                                                         [INDESTRUCTIBLE]
                // 27 - wide switch without zipper                                                                      [INDESTRUCTIBLE]
                // 28 - an empty object that can be shot                                                                [DESTRUCTIBLE]      PISTOL          NO
                // 29 - an empty object that can be shot through, something will spawn on death                         [DESTRUCTIBLE]      PISTOL          YES
                // // // 30 - a regular box that can be blown up                                                        [DESTRUCTIBLE]      PISTOL          YES
                // // // 31 - a regular box that can be blown up                                                        [DESTRUCTIBLE]      PISTOL          YES
                // 32 - Strange Little Yellow Square                                                                    [INDESTRUCTIBLE]
                // 33 - Steel Coil                                                                                      [INDESTRUCTIBLE]
                // // // 34 - Strange Unused Shape                                                                      [INDESTRUCTIBLE]
                // // // 35 - Light Pylon With No Texture, Completely Red...                                            [INDESTRUCTIBLE]
                // // // 36 - Strange Tall Square ( improperly textured )                                               [INDESTRUCTIBLE]
                // // // 37 - Egg Husk Shape ( untextured )                                                             [INDESTRUCTIBLE]
                // // // greater than 37 - a regular box that can be blown up                                           [DESTRUCTIBLE]      PISTOL          YES
                Drop = br.ReadByte(),           // 0 = Pickup 2 = Enemy
                unknown1 = br.ReadByte(),       // UNKNOWN
                unknown2 = br.ReadByte(),       // UNKNOWN - only ever 0 or 10 across every level in the game
                Drop1 = br.ReadByte(),          // index of first pickup dropped
                Drop2 = br.ReadByte(),          // index of second pickup dropped
                unknown3 = br.ReadByte(),       // UNKNOWN
                unknown4 = br.ReadByte(),       // UNKNOWN
                unknown5 = br.ReadByte(),       // UNKNOWN
                unknown6 = br.ReadByte(),       // only ever 0 across every level in the game
                unknown7 = br.ReadByte(),       // UNKNOWN
                unknown8 = br.ReadByte(),       // only ever 0 across every level in the game
                Rotation = br.ReadByte(),       // Byte Direction  Facing
                                                // 00 - North   // Y+
                                                // 02 - East    // X+
                                                // 04 - South   // Y-
                                                // 06 - West    // X-
                unknown10 = br.ReadByte()       // only ever 0 across every level in the game
            };
            boxes.Add(box);
        }
        // doors formula = value multiplied by 8 - (8 bytes one element)
        for (int i = 0; i < doorCount; i++)
        {
            Door door = new Door
            {
                X = br.ReadByte(),              // x coordinate of the door
                Y = br.ReadByte(),              // y coordinate of the door
                unknown = br.ReadByte(),        // UNKNOWN - only ever 64 or 0 across every level in the game
                Time = br.ReadByte(),           // door open time
                Tag = br.ReadByte(),            // door tag
                unknown2 = br.ReadByte(),       // only ever 0 across every level in the game
                Rotation = br.ReadByte(),       // Byte Direction  Facing
                                                // 00 - North   // Y+
                                                // 02 - East    // X+
                                                // 04 - South   // Y-
                                                // 06 - West    // X-
                Index = br.ReadByte()           // index of the door model in the BND file
            };
            doors.Add(door);
        }
        // lifts formula = value multiplied by 16 - (16 bytes one element)
        for (int i = 0; i < liftCount; i++)
		{
            Lifts lift = new Lifts
            {
                X = br.ReadByte(),              // x coordinate of the lift
                Y = br.ReadByte(),              // y coordinate of the lift
                Z = br.ReadByte(),              // z coordinate of the lift
                unknown1 = br.ReadByte(),       // this byte is always 0, 1, 2, 3, 4, 5 or 6 across every level in the game
                unknown2 = br.ReadByte(),       // this byte is always 0 across every level in the game
                unknown3 = br.ReadByte(),       // this byte is always 24, 27, 31, 32, 43, 44, 46, 47, 48, 50, 52, 56, 63, 64, 68, 79, 80, 84, 95 or 224 across every level in the game
                unknown4 = br.ReadByte(),       // this byte is always 1 across every level in the game
                unknown5 = br.ReadByte(),       // this byte is always 1, 4, 5 or 17 across every level in the game
                unknown6 = br.ReadByte(),       // this byte is always 0, 1 or 60 across every level in the game
                unknown7 = br.ReadByte(),       // this byte is always 0, 30, 50, 60, 90, 120, 150, 190, 210, 240 or 255 across every level in the game
                unknown8 = br.ReadByte(),       // this byte is always 1, 2, 3, 4, 5, 6, 7, 10, 25 or 35  across every level in the game
                unknown9 = br.ReadByte(),       // this byte is always 0 across every level in the game
                unknown10 = br.ReadByte(),      // this byte is always 0, 1, 2, 3, 4, 5, 6, 7, 8 or 9 across every level in the game
                unknown11 = br.ReadByte(),      // this byte is always 0, 1, 2, 3, 4, 5, 6, 7 or 8 across every level in the game ( these three bytes always match )
                unknown12 = br.ReadByte(),      // this byte is always 0, 1, 2, 3, 4, 5, 6, 7 or 8 across every level in the game ( these three bytes always match )
                unknown13 = br.ReadByte(),      // this byte is always 0, 1, 2, 3, 4, 5, 6, 7 or 8 across every level in the game ( these three bytes always match )
            };
            lifts.Add(lift);
		}
        long remainingBytes = br.BaseStream.Length - br.BaseStream.Position;
        remainderBytes = br.ReadBytes((int)remainingBytes);
    }
    // Parse BND file sections from a byte array
    private List<BndSection> LoadSection(byte[] bnd, string section)
    {
        var sections = new List<BndSection>();
        using var br = new BinaryReader(new MemoryStream(bnd));
        string formTag = Encoding.ASCII.GetString(br.ReadBytes(4)); // Read FORM header
        if (formTag != "FORM") { throw new Exception("Invalid BND file: missing FORM header."); }
        int formSize = BitConverter.ToInt32(br.ReadBytes(4).Reverse().ToArray(), 0);
        string platform = Encoding.ASCII.GetString(br.ReadBytes(4)); // e.g., "PSXT"
        while (br.BaseStream.Position + 8 <= br.BaseStream.Length) // Parse chunks
        {
            string chunkName = Encoding.ASCII.GetString(br.ReadBytes(4));
            int chunkSize = BitConverter.ToInt32(br.ReadBytes(4).Reverse().ToArray(), 0);
            if (br.BaseStream.Position + chunkSize > br.BaseStream.Length) { break; }
            byte[] chunkData = br.ReadBytes(chunkSize);
            if (chunkName.StartsWith(section)) { sections.Add(new BndSection { Name = chunkName, Data = chunkData }); }
            if ((chunkSize % 2) != 0) { br.BaseStream.Seek(1, SeekOrigin.Current); } // IFF padding to 2-byte alignment
        }
        return sections;
    }
    // Export to CSV
    private void ExportToCSV(List<CollisionNode> data, string filePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (var row in data)
            {
                string line = string.Join(",", row.Name,row.unknown1,row.unknown5,row.unknown6, row.unknown7, row.unknown8, row.ceilingFog, row.floorFog, row.ceilingHeight, row.floorHeight, row.unknown13, row.unknown14, row.Lighting, row.Action);
                writer.WriteLine(line);
            }
        }
    }
}
