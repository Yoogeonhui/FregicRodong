using UnityEngine;
using System.Collections;

public class Roomnumber : MonoBehaviour {
    public static int roomnumber = 0;
    public static int[,] isfirstroom=new int[100,100];
    public static bool move(int angle,int roomnum)
    {
        int[] roomposs = nowroompos(roomnum);
        int pointx = roomposs[0],pointz=roomposs[1];
        //input: 0,1,2,3/
        switch(angle)
        {
            case 0:
                pointx++;
                break;
            case 1:
                pointz--;
                break;
            case 2:
                pointx--;
                break;
            case 3:
                pointz++;
                break;
        }
        //-1인 경우 안만들어진거이니까 true로 반환?
        bool result = (isfirstroom[pointx, pointz]==-1);
        if(result)
            isfirstroom[pointx, pointz] = roomnumber+1;
        return result;
    }
    public static int[] nowroompos(int roomnum)
    {
        int[] result = new int[2] { -1, -1 };
        bool found = false;
        int i=-1, j=-1;
        for (i = 0; i < 100; i++)
        {
            for (j = 0; j < 100; j++)
                if (isfirstroom[i, j] == roomnum)
                {
                    found = true;
                    break;
                }
            if (found) break;
        }
        if (found)
            result = new int[2] { i, j };
        Debug.Log(result[0]+","+result[1]);

        return result;
    }
    void Start()
    {
        for (int i = 0; i < 100; i++)
            for (int j = 0; j < 100; j++)
                isfirstroom[i, j] = -1;
        isfirstroom[50, 50] = 0;
    }
}
