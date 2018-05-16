using UnityEngine;
using System.Collections.Generic;               //Use this so you can manipulate Lists

public class GUIRadar : MonoBehaviour
{
    public Camera cam;
    public Texture2D targetIcon;                //The target icon that will be placed in front of targets
    public float maxTargetIconSize = 300f;      //The maximum size of icons when close to the target
    public float minTargetIconSize = 0f;        //The minimum size of icons when they appear (far from the target)
    public float maxDistanceDisplay = 10f;      //The maximum distance from where you can see the icons appearing
    public float minDistanceDisplay = 2f;       //The minimum distance from where you see the icons disappearing when too close
    public float smoothGrowingParameter = 25f;  //The speed of the growing effect on icons
    public float smoothMovingParameter = 25f;   //The moving speed of items : high values means the icon is "attached" to the item
    public bool directViewEnabled = true;       //If you want to allow icon display even if targets aren't in direct view for the player

    public struct TargetStruct                  //Structure that contain every icon informations
    {
        public GameObject item;                 //-the item the icon is linked to
        public Vector3 screenPos;               //-the screen position of the icon (!= world position)
        public float xSize, ySize;              //-the current size of the icon on the screen
        public float xPos, yPos;                //-the current coordinates of the screen position
        public float xTargetSize, yTargetSize;  //-the coordinates you want the icon to reach
        public float xTargetPos, yTargetPos;    //-the size you want the icon to reach on the screen
        public float distance;                  //-the distance between player and the item linked to the icon
        public bool directView;                 //-tells you if the item is in direct view or not
    }

    public List<TargetStruct> TargetList = new List<TargetStruct>();//Your ICONS LIST
    public GameObject[] Targets;                                    //The GameObjects considered as targets

    private int targetsCount;                                       //Number of targets in the scene

    void Awake()
    {
        Targets = GameObject.FindGameObjectsWithTag("Target");     //Get all the potential targets in the scene (just replace it by your own tag : "MyTag")

        foreach (GameObject target in Targets)                                   //Put every detected GameObject into your ICONS LIST
        {
            TargetStruct newTarget = new TargetStruct();
            newTarget.item = target;                                            //and attach each icon its GameObject
            TargetList.Add(newTarget);
        }

        targetsCount = TargetList.Count;                                        //Count the number of target in the scene
    }

    void Update()
    {
        for (int i = 0; i < targetsCount; i++)                                                         //You have to repeat it for every icons : be aware that if you have too much that could use a lot of ressoures
        {
            TargetStruct target = new TargetStruct();                                               //You have to create a temporary TargetStruct since you can't access a variable of an element in a list directly
            target = TargetList[i];                                                                 //You take the item attached to the icon

            target.screenPos = cam.WorldToScreenPoint(target.item.transform.position);           //Convert world coordinates of the item into screen ones
            target.distance = Vector3.Distance(target.item.transform.position, transform.position); //Get the distance between item and player

            if (target.distance > maxDistanceDisplay || target.distance < minDistanceDisplay)            //If the item is too far or too close
            {
                target.xTargetSize = minTargetIconSize;                                             //you want it to disappear
                target.yTargetSize = minTargetIconSize;                                             //or at least to be in its smaller size
            }
            else
            {
                target.xTargetSize = maxTargetIconSize / (target.distance);                           //Else you get its size with the
                target.yTargetSize = maxTargetIconSize / (target.distance);                           //distance : far<=>small / close<=>big

            }

            if (target.distance > maxDistanceDisplay)                                                  //If the item is too far, you set its screen position : (this way it seems as if the icon was coming away from the screen to focus your target)
            {
                if (target.screenPos.x < Screen.width / 2)                                               //-if it's under the center of the view field
                    target.xTargetPos = 0;                                                          //to the bottom of the screen
                else                                                                                //-else
                    target.xTargetPos = Screen.width;                                               //to the top of the screen

                if (target.screenPos.y < Screen.height / 2)                                              //-if it's on the right of the view field
                    target.yTargetPos = Screen.height;                                              //to the right of the screen
                else                                                                                //-else
                    target.yTargetPos = 0;                                                          //to the left of the screen
            }
            else                                                                                    //If the item is NOT too far, you set its screen position :
            {
                target.xTargetPos = target.screenPos.x - target.xSize / 2;                              //in x-axis to the item's x-position minus half of the icon's size
                target.yTargetPos = Screen.height - target.screenPos.y - target.ySize / 2;                //in y-axis to the item's y-position minus half of the icon's size
            }

            target.xSize = Mathf.Lerp(target.xSize, target.xTargetSize, smoothGrowingParameter * Time.deltaTime); //You do lerps on your icons size so you can adjust
            target.ySize = Mathf.Lerp(target.xSize, target.yTargetSize, smoothGrowingParameter * Time.deltaTime); //the speed of their resizing

            target.xPos = Mathf.Lerp(target.xPos, target.xTargetPos, smoothMovingParameter * Time.deltaTime);     //You do lerps on your icons position so you can adjust
            target.yPos = Mathf.Lerp(target.yPos, target.yTargetPos, smoothMovingParameter * Time.deltaTime);     //their moving speed

            RaycastHit hitInfos = new RaycastHit();                                                                 //You create a new variable to stock the information of the coming raycast
            Physics.Raycast(transform.position, target.item.transform.position - transform.position, out hitInfos);   //and you RayCast from the player's position to the item's position

            if (hitInfos.collider.gameObject.layer == 8)                                                               //HERE IS A BIT TRICKY : you have to creat new layers (I called them "Interactive Items" and "Obstacles") and to apply them to your various items.
                target.directView = true;                                                                             //Then you select EVERY items in your scene and set their layer to "Ignore Raycast". After that you select your interactive items biggest shape (if you have big trigger colliders on them select the item that hold it),
            else                                                                                                    //and set their layers to "Interactive Items". Last part is setting every potential obstacle item layer to "Obstacles".
                target.directView = false;                                                                            //NOTE : Here my "Interactive Items" layer number is 8

            TargetList[i] = target;                                                                 //You apply all the variables to your index-i icon in the ICONS LIST
        }
    }

    void OnGUI()
    {
        for (int i = 0; i < targetsCount; i++)                                                                                             //For every icon
        {
            if (TargetList[i].screenPos.z > 0) 
         {//If the icon is in front of you and all the required conditions are okay
            GUI.DrawTexture(new Rect(TargetList[i].xPos, TargetList[i].yPos, TargetList[i].xSize, TargetList[i].ySize), targetIcon);//you display the icon with it's size and position
            }
    }
  }
}
