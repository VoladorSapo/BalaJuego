using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class cameraController : MonoBehaviour
{
    CinemachineConfiner2D confiner;

  [SerializeField]  PolygonCollider2D cameraConfinerCollider;

 [SerializeField]   Vector2[] Startvectors;
    // Start is called before the first frame update
    void Start()
    {
       ServiceLocator.Instance.Get<ILevelController>().subscribeToAreaEnd(endArea);
        ServiceLocator.Instance.Get<ILevelController>().subscribeToAreaStart(startArea);
        ServiceLocator.Instance.Get<ILevelController>().subscribeToRestart(restart);

        confiner = GetComponent<CinemachineConfiner2D>();
        Startvectors = cameraConfinerCollider.points;
    }
    void restart()
    {
        cameraConfinerCollider.SetPath(0,Startvectors);

        confiner.InvalidateCache();
    }
    // Update is called once per frame
    void Update()
    { 
    }
    public void changeRestartConfiners(Vector2[] newPoints)
    {
        Startvectors = newPoints;
    }
    void endArea(object sender, AreaData data)
    {
        float upY = Mathf.Max(data.nextArea.endCollider.transform.position.y + data.nextArea.endCollider.size.y / 2, data.endArea.startCollider.transform.position.y + data.endArea.startCollider.size.y / 2);
        float downY = Mathf.Min(data.nextArea.endCollider.transform.position.y - data.nextArea.endCollider.size.y / 2, data.endArea.startCollider.transform.position.y - data.endArea.startCollider.size.y / 2);
        float leftX = Mathf.Min(data.nextArea.endCollider.transform.position.x , data.endArea.startCollider.transform.position.x);
        float RightX = Mathf.Max(data.nextArea.endCollider.transform.position.x, data.endArea.startCollider.transform.position.x);


        cameraConfinerCollider.SetPath(0, new[] {
            new Vector2(leftX, upY),
               new Vector2(leftX, downY),
                  new Vector2(RightX, downY),
   new Vector2(RightX, upY), });

        confiner.InvalidateCache();
    }
    void startArea(object sender, LevelAreaController data)
    {
        print("cambio "+data.name + " " + data.endCollider.name);
        print(data.endCollider.transform.position.x + " + " + data.transform.position.x + " = " + (data.endCollider.transform.position.x + data.transform.position.x));
        cameraConfinerCollider.SetPath(0, new[] {
            new Vector2(data.startCollider.transform.position.x , data.startCollider.transform.position.y+ data.startCollider.size.y / 2),
            new Vector2(data.startCollider.transform.position.x , data.endCollider.transform.position.y  - data.startCollider.size.y / 2),
            new Vector2(data.endCollider.transform.position.x, data.startCollider.transform.position.y - data.startCollider.size.y / 2),
            new Vector2(data.endCollider.transform.position.x , data.endCollider.transform.position.y+ data.startCollider.size.y / 2) });
        confiner.InvalidateCache();


    }

}
