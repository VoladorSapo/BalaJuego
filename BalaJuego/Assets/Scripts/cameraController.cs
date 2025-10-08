using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class cameraController : MonoBehaviour
{
    CinemachineConfiner2D confiner;

  [SerializeField]  PolygonCollider2D cameraConfinerCollider;

 [SerializeField]   Vector2[] Startvectors;

    [SerializeField] Vector2[] fromPoints;
    [SerializeField] Vector2[] currentPoints;

    [SerializeField] Vector2[] toPoints;
    [SerializeField] float transitionTime = 10;
    [SerializeField] float transitionTimePassed = 2;

    // Start is called before the first frame update
    void Start()
    {
       ServiceLocator.Instance.Get<ILevelController>().subscribeToAreaEnd(endArea);
        ServiceLocator.Instance.Get<ILevelController>().subscribeToAreaStart(startArea);
        ServiceLocator.Instance.Get<ILevelController>().subscribeToRestart(restart);
        confiner = GetComponent<CinemachineConfiner2D>();
        Startvectors = cameraConfinerCollider.points;
        fromPoints = cameraConfinerCollider.points;
        currentPoints = cameraConfinerCollider.points;
        toPoints = cameraConfinerCollider.points;
        transitionTimePassed = transitionTime;
    }
    void restart()
    {
        transitionTimePassed = transitionTime + 1;
        cameraConfinerCollider.SetPath(0,Startvectors);

        confiner.InvalidateCache();

    }
    // Update is called once per frame
    void Update()
    {
        if (transitionTimePassed <= transitionTime)
        {
            transitionTimePassed += Time.deltaTime;
            for (int i = 0; i < fromPoints.Length; i++)
            {
                currentPoints[i] = Vector2.Lerp(fromPoints[i], toPoints[i], transitionTimePassed / transitionTime);
            }

            cameraConfinerCollider.SetPath(0,currentPoints);

            confiner.InvalidateCache();
        }
    }
    public void changeRestartConfiners(Vector2[] newPoints)
    {
        Startvectors = newPoints;
    }
    void endArea(object sender, AreaData data)
    {
        fromPoints = cameraConfinerCollider.points;
        float upY = Mathf.Max(data.nextArea.endCollider.transform.position.y + data.nextArea.endCollider.size.y / 2, data.endArea.startCollider.transform.position.y + data.endArea.startCollider.size.y / 2);
        float downY = Mathf.Min(data.nextArea.endCollider.transform.position.y - data.nextArea.endCollider.size.y / 2, data.endArea.startCollider.transform.position.y - data.endArea.startCollider.size.y / 2);
        float leftX = Mathf.Min(data.nextArea.endCollider.transform.position.x , data.endArea.startCollider.transform.position.x);
        float RightX = Mathf.Max(data.nextArea.endCollider.transform.position.x, data.endArea.startCollider.transform.position.x);

       
        print("Rightx" + RightX);
        toPoints[0] = new Vector2(leftX, upY);
        toPoints[1] = new Vector2(leftX, downY);
        toPoints[2] = new Vector2(RightX, downY);
        toPoints[3] = new Vector2(RightX, upY);
        transitionTimePassed = 0;

   //     cameraConfinerCollider.SetPath(0, new[] {
   //         new Vector2(leftX, upY),
   //            new Vector2(leftX, downY),
   //               new Vector2(RightX, downY),
   //new Vector2(RightX, upY), });

   //     confiner.InvalidateCache();
    }
    
    void startArea(object sender, LevelAreaController data)
    {
        print("cambio "+data.name + " " + data.endCollider.name);
        print(data.endCollider.transform.position.x + " + " + data.transform.position.x + " = " + (data.endCollider.transform.position.x + data.transform.position.x));
        fromPoints = cameraConfinerCollider.points;

        toPoints[0] = new Vector2(data.startCollider.transform.position.x, data.startCollider.transform.position.y + data.startCollider.size.y / 2);
        toPoints[1] = new Vector2(data.startCollider.transform.position.x, data.endCollider.transform.position.y - data.startCollider.size.y / 2);
        toPoints[2] = new Vector2(data.endCollider.transform.position.x, data.startCollider.transform.position.y - data.startCollider.size.y / 2);
        toPoints[3] = new Vector2(data.endCollider.transform.position.x, data.endCollider.transform.position.y + data.startCollider.size.y / 2);
        transitionTimePassed = 0;

        //cameraConfinerCollider.SetPath(0, new[] {
        //    new Vector2(data.startCollider.transform.position.x , data.startCollider.transform.position.y+ data.startCollider.size.y / 2),
        //    new Vector2(data.startCollider.transform.position.x , data.endCollider.transform.position.y  - data.startCollider.size.y / 2),
        //    new Vector2(data.endCollider.transform.position.x, data.startCollider.transform.position.y - data.startCollider.size.y / 2),
        //    new Vector2(data.endCollider.transform.position.x , data.endCollider.transform.position.y+ data.startCollider.size.y / 2) });
        //confiner.InvalidateCache();


    }

}
