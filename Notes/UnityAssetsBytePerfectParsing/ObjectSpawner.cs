using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using static UnityEngine.Rendering.DebugUI.MessageBox;
using static UnityEngine.Rendering.ProbeAdjustmentVolume;

public class ObjectSpawner : MonoBehaviour
{
    //3D models to instantiate
    //[Header("3D Objects")]
    private GameObject dummyObj;
    public GameObject colObj;
    //public Material switchMaterial;
    //public Mesh smallCrate, largeCrate, barrel, smallSwitch;
    //OBJ3D Meshes
    //0 - Explosive Barrel
    //1 - Single Crate
    //2 - Double Crate
    //3 - Switch Red Left Light
    //4 - Switch Red Right Light
    //5 - Switch Both Lights Off
    //6 - Switch Both Lights Yellow
    //7 - Large Switch Red Left Light
    //8 - Large Switch Red Right Light
    //9 - Large Switch Both Lights Off
    //10 - Large Switch Both Lights Yellow
    //11 - Switch Battery Red Left Light
    //12 - Switch Battery Red Right Light
    //13 - Switch Battery Both Lights Off
    //14 - Switch Battery Both Lights Yellow
    //15 - Large Switch Battery Red Left Light
    //16 - Large Switch Battery Red Right Light
    //17 - Large Switch Battery Both Lights Off
    //18 - Large Switch Battery Both Lights Yellow
    //19 - Boneship Switch Red Left Light
    //20 - Boneship Switch Red Right Light
    //21 - Boneship Switch Both Lights Off
    //22 - Boneship Switch Both Lights Yellow
    //23 - Boneship Switch Red Left Light
    //24 - Boneship Switch Red Right Light
    //25 - Boneship Switch Both Lights Off
    //26 - Boneship Switch Both Lights Yellow
    //27 - Boneship Switch Red Left Light
    //28 - Boneship Switch Red Right Light
    //29 - Boneship Switch Both Lights Off
    //30 - Boneship Switch Both Lights Yellow
    //31 - Boneship Switch Red Left Light
    //32 - Boneship Switch Red Right Light
    //33 - Boneship Switch Both Lights Off
    //34 - Boneship Switch Both Lights Yellow
    //35 - Steel Coil
    //36 - Unused Shape
    //37 - Pylon(Unused )
    //38 - Computer(Unused? )
    //39 - Egg Husk Shape Untextured
    //40 - Stasis Pod Cover
    //41 - Egg Husk
    //public Mesh pistolClip, shotgunShell, pistol, shotgun, dermPatch,autoMap, healthPack, battery;
    //PICKMOD Meshes
    //0 - Pistol
    //1 - Shotgun
    //2 - Pulse Rifle
    //3 - Flamethrower
    //4 - Smart Gun
    //5 - Seismic Charge
    //6 - Battery
    //7 - Night Vision Goggles
    //8 - Pistol Clip
    //9 - Shotgun Shell
    //10 - Pulse Rifle Clip
    //11 - Pulse Rifle Grenade
    //12 - Flamethrower Fuel
    //13 - Smart Gun Ammunition
    //14 - ID Badge
    //15 - Auto Mapper
    //16 - Hypo Pack
    //17 - Acid Vest
    //18 - Body Suit
    //19 - Medi Kit
    //20 - Dermpatch
    //21 - Boots
    //22 - Adrenaline Burst
    //23 - Shoulder Lamp
    //24 - Shotgun Ammunition
    //25 - Pistol Shell
    //public Mesh menuJoystick, menuCamera;
    //OPTOBJ Meshes
    //0 - Joystick
    //1 - Camera
    //2 - Gamepad
    //3 - Multitap?
    //4 - Hard Drive Saving<-
    //5 - Hard Drive Loading ->
    //6 - Camera Crossed Out
    //7 - Keyboard
    //8 - Mouse
    //9 - Computer, Monitor and Keyboard
    //10 - Two Linked Computers, Monitors and Keyboards
    //11 - Speaker(Disc )
    //12 - Speaker(Music )
    //13 - Headphones
    private Material objMaterial;
    public static ObjectSpawner spawner;
    private GameObject colFrame, pathCover, crateCover, mobCover, pickupCover, liftCover, doorCover;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
		if (spawner == null)
        {
            spawner = this;
        }
        else
        {
            Destroy(gameObject);
        }
        dummyObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        objMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));

    }
	
    [ContextMenu("SpawnAll")]
    public void SpawnAll()
    {
        SpawnPaths();
		SpawnObjects();
		SpawnMobs();
		SpawnPickups();
		SpawnLifts();
        SpawnDoors();
        AlienTrilogyMapLoader.loader.pathNodes.Clear();
        AlienTrilogyMapLoader.loader.boxes.Clear();
        AlienTrilogyMapLoader.loader.monsters.Clear();
        AlienTrilogyMapLoader.loader.pickups.Clear();
        AlienTrilogyMapLoader.loader.lifts.Clear();
        AlienTrilogyMapLoader.loader.doors.Clear();
    }

    [ContextMenu ("ClearAll")]
    public void ClearAll()
    {
        Destroy(pathCover);
        Destroy(crateCover);
        Destroy(mobCover);
        Destroy(pickupCover);
        Destroy(doorCover);
        Destroy(liftCover);
    }

    [ContextMenu("Spawn Collisions")]
    public void SpawnCollisions()
    {
        GameObject colCover = Instantiate(new GameObject(), new Vector3(0,0,0), transform.rotation);
        colCover.transform.name = "Collision Nodes";
        int index = 0;
        int xCount = 1;
        int yCount = 0;
        foreach (CollisionNode col in AlienTrilogyMapLoader.loader.collisions)
        {
            Vector3 pos = new Vector3(xCount+.5f, -10, yCount);
            GameObject newObj = Instantiate(colObj, pos, transform.rotation, colCover.transform);
            newObj.transform.localPosition = pos;
            RaycastHit hit;
            newObj.name = "PathNode " + index;
            col.obj= newObj;
            if (Physics.Raycast(newObj.transform.position, newObj.transform.up * 15, out hit))
            {
                if (hit.collider != null)
                {
                    newObj.transform.position = hit.point;
                }
            }
            xCount--;
            if (xCount < -int.Parse(AlienTrilogyMapLoader.loader.mapLengthString)+1)
            {
                yCount++;
                xCount = 0;
            }
            index++;
        }
    }

    [ContextMenu("Spawn Paths")]
    public void SpawnPaths()
    {
        pathCover = Instantiate(new GameObject(), new Vector3(0,0,0), transform.rotation);
        pathCover.transform.name = "Path Nodes";
        int index = 0;
        dummyObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Material spawnMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        spawnMaterial.color = Color.red;
        foreach (PathNode obj in AlienTrilogyMapLoader.loader.pathNodes)
        {
            Vector3 pos = new Vector3(-obj.X, 0 - 10, obj.Y);
            GameObject newObj = GameObject.Instantiate(dummyObj, pos, transform.rotation, pathCover.transform); 
            newObj.transform.localPosition = pos;
            newObj.GetComponent<MeshRenderer>().material = spawnMaterial;
            RaycastHit hit;
            newObj.name = "PathNode " + index;
            obj.obj = newObj;
            if (Physics.Raycast(newObj.transform.position, newObj.transform.up, out hit))
            {
                if (hit.collider != null)
                {
                    newObj.transform.position = hit.point;
                }
            }
            index++;
        }
    }

    [ContextMenu("Spawn Objects")]
    public void SpawnObjects()
    {
        crateCover = Instantiate(new GameObject(), new Vector3(0,0,0), transform.rotation);
        crateCover.transform.name = "Objects";
        int index = 0;
        dummyObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Material crateMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        crateMaterial.color = Color.cyan;
        foreach (Crate crate in AlienTrilogyMapLoader.loader.boxes)
        {
            switch (crate.Type)
            {
                case 20: crate.Name = "Crate " + index; break;
                case 21: crate.Name = "Destructible Wall " + index; break;
                case 22: crate.Name = "Small Switch " + index; break;
                case 23: crate.Name = "Explosive Barrel " + index; break;
                case 24: crate.Name = "Animated Switch " + index; break;
                case 25: crate.Name = "Double Crates " + index; break;
                case 26: crate.Name = "Wide Switch " + index; break;
                case 27: crate.Name = "Wide Battery Switch " + index; break;
                //
                case 32: crate.Name = "Strange Little Yellow Square   " + index; break;
                case 33: crate.Name = "Steel Coil       " + index; break;
            }
            Vector3 pos = new Vector3(-crate.X-.5f, 0 - 10, crate.Y);
            GameObject newObj = GameObject.Instantiate(dummyObj, pos, transform.rotation, crateCover.transform);
            newObj.GetComponent<MeshRenderer>().material = crateMaterial;
            newObj.transform.localPosition = pos;
            newObj.name = crate.Name;
            RaycastHit hit;
            crate.spawnedObject = newObj;

            if (Physics.Raycast(newObj.transform.position, newObj.transform.up, out hit))
            {
                if (hit.collider != null)
                {
                    newObj.transform.position = hit.point;
                }
            }
            index++;
        }
    }

    [ContextMenu("Spawn Mobs")]
    public void SpawnMobs()
    {
        mobCover = Instantiate(new GameObject(), new Vector3(0, 0, 0), transform.rotation);
        mobCover.transform.name = "Monsters";
        int index = 0;
        dummyObj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        Material mobMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        mobMaterial.color = Color.yellow;
        foreach (Monster monster in AlienTrilogyMapLoader.loader.monsters)
        {
            switch (monster.Type)
            {
                case 1: monster.Name = "Egg " + index; break;
                case 2: monster.Name = "Facehugger " + index; break;
                case 3: monster.Name = "ChestBurster " + index; break;
                case 4: monster.Name = "Dog Alien " + index; break;
                case 5: monster.Name = "ChestBurster " + index; break;
                case 6: monster.Name = "Warrior Drone " + index; break;
                case 7: monster.Name = "Queen " + index; break;
                case 8: monster.Name = "Ceiling Warrior Drone " + index; break;
                case 9: monster.Name = "Ceiling Dog Alien " + index; break;
                case 10: monster.Name = "Colonist " + index; break;
                case 11: monster.Name = "Security Guard " + index; break;
                case 12: monster.Name = "Soldier " + index; break;
                case 13: monster.Name = "Synthetic " + index; break;
                case 14: monster.Name = "Handler " + index; break;
                //case 15: monster.Name = "Player " + index; break;                 //unused possibly player?
                case 16: monster.Name = "Horizontal Steam Vent " + index; break;
                case 17: monster.Name = "Horizontal Flame Vent " + index; break;
                case 18: monster.Name = "Vertical Steam Vent " + index; break;
                case 19: monster.Name = "Vertical Flame Vent " + index; break;
            }
            Vector3 pos = new Vector3(-monster.X-.5f, 0 , monster.Y);
            GameObject newObj = GameObject.Instantiate(dummyObj, pos, transform.rotation, mobCover.transform);
            newObj.GetComponent<MeshRenderer>().material = mobMaterial;
            newObj.transform.localPosition = pos;
            newObj.name = monster.Name;
            monster.spawnedObj = newObj;
            RaycastHit hit;

            if (Physics.Raycast(newObj.transform.position, newObj.transform.up, out hit))
            {
                if (hit.collider != null)
                {
                    newObj.transform.position = hit.point;
                }
            }
            index++;
        }
    }

    [ContextMenu("Spawn Pickups")]
    public void SpawnPickups()
    {
        pickupCover = Instantiate(new GameObject(), new Vector3(0, 0, 0), transform.rotation);
        pickupCover.transform.name = "Pickups";
        int index = 0;
        dummyObj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        Material pickupMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        pickupMaterial.color = Color.blue;
        foreach (Pickup pickup in AlienTrilogyMapLoader.loader.pickups)
        {
            switch (pickup.Type)
            {
                case 0: pickup.Name = "Pistol " + index; break;
                case 1: pickup.Name = "Shotgun " + index; break;
                case 2: pickup.Name = "Pulse Rifle " + index; break;
                case 3: pickup.Name = "Flame Thrower " + index; break;
                case 4: pickup.Name = "Smartgun " + index; break;
                //case 5: pickup.Name = "Unused " + index; break;
                case 6: pickup.Name = "Seismic Charge " + index; break;
                case 7: pickup.Name = "Battery " + index; break;
                case 8: pickup.Name = "Night Vision Goggles " + index; break;
                case 9: pickup.Name = "Pistol Clip " + index; break;
                case 10: pickup.Name = "Shotgun Cartridge " + index; break;
                case 11: pickup.Name = "Pulse Rifle Clip " + index; break;
                case 12: pickup.Name = "Grenades " + index; break;
                case 13: pickup.Name = "Flamethrower Fuel " + index; break;
                case 14: pickup.Name = "Smartgun Ammunition " + index; break;
                case 15: pickup.Name = "Identity Tag " + index; break;
                case 16: pickup.Name = "Auto Mapper " + index; break;
                case 17: pickup.Name = "Hypo Pack " + index; break;
                case 18: pickup.Name = "Acid Vest " + index; break;
                case 19: pickup.Name = "Body Suit " + index; break;
                case 20: pickup.Name = "Medi Kit " + index; break;
                case 21: pickup.Name = "Derm Patch " + index; break;
                case 22: pickup.Name = "Protective Boots " + index; break;
                case 23: pickup.Name = "Adrenaline Burst " + index; break;
                case 24: pickup.Name = "Derm Patch " + index; break;
                case 25: pickup.Name = "Shoulder Lamp " + index; break;
            }
            Vector3 pos = new Vector3(-pickup.X, 0, pickup.Y);
            GameObject newObj = GameObject.Instantiate(dummyObj, pos, transform.rotation, pickupCover.transform);
            newObj.GetComponent<MeshRenderer>().material = pickupMaterial;
            newObj.transform.localPosition = pos;
            newObj.name = pickup.Name;
            pickup.spawnedObject = newObj;
            RaycastHit hit;

            if (Physics.Raycast(newObj.transform.position, newObj.transform.up, out hit))
            {
                if (hit.collider != null)
                {
                    newObj.transform.position = hit.point;
                }
            }
            index++;
        }
    }

    [ContextMenu("Spawn Lifts")]
    public void SpawnLifts()
    {
        liftCover = Instantiate(new GameObject(), new Vector3(0, 0, 0), transform.rotation);
        liftCover.name = "Lifts";
        dummyObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Material liftMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        liftMaterial.color = Color.yellow;
        int index = 0;

        foreach (Lifts obj in AlienTrilogyMapLoader.loader.lifts)
        {
            Vector3 pos = new Vector3(-obj.X, 0 - 10, obj.Y);
            GameObject newObj = GameObject.Instantiate(dummyObj, pos, transform.rotation, liftCover.transform);
            newObj.GetComponent<MeshRenderer>().material = liftMaterial;
            newObj.transform.localPosition = pos;
            newObj.name = "Lift " + index;
            obj.spawnedObject = newObj;
            RaycastHit hit;

            if (Physics.Raycast(newObj.transform.position, newObj.transform.up, out hit))
            {
                if (hit.collider != null)
                {
                    newObj.transform.position = hit.point;
                }
            }
            index++;
        }
    }

    [ContextMenu("Spawn Doors")]
    public void SpawnDoors()
    {
        doorCover = Instantiate(new GameObject(), new Vector3(0, 0, 0), transform.rotation);
        doorCover.name = "Doors";
        dummyObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
        Material doorMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        doorMaterial.color = Color.white;
        int index = 0;
        foreach (Door obj in AlienTrilogyMapLoader.loader.doors)
        {
            Vector3 pos = new Vector3(-obj.X, 0 - 10, obj.Y);
            GameObject newObj = GameObject.Instantiate(dummyObj, pos, transform.rotation, doorCover.transform);
            newObj.GetComponent<MeshRenderer>().material = doorMaterial;
            newObj.transform.localPosition = pos;
            newObj.name = "Door " + index;
            obj.spawnedObject = newObj;
            RaycastHit hit;

            if (Physics.Raycast(newObj.transform.position, newObj.transform.up, out hit))
            {
                if (hit.collider != null)
                {
                    newObj.transform.position = hit.point;
                }
            }
            index++;
        }
    }
}
