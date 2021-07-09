using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject support_zone;
    public GameObject support_zone_1;
    public GameObject support_zone_2;
    public GameObject support_zone_3;
    public GameObject mid_zone;
    public GameObject mid_zone_1;
    public GameObject mid_zone_2;
    public GameObject mid_zone_3;
    public GameObject front_zone;
    public GameObject front_zone_1;
    public GameObject front_zone_2;
    public GameObject front_zone_3;

    public GameObject getZoneByPos(int[] zone)
    {
        //col
        switch (zone[1])
        {
            case 0: 
                switch (zone[0])
                {
                    case 0: return front_zone_1;
                        break;
                    case 1: return front_zone_2;
                        break;
                    case 2: return front_zone_3;
                        break;
                    default:
                        break;
                }
                break;
            case 1:
                switch (zone[0])
                {
                    case 0:
                        return mid_zone_1;
                        break;
                    case 1:
                        return mid_zone_2;
                        break;
                    case 2:
                        return mid_zone_3;
                        break;
                    default:
                        break;
                }
                break;
            case 2:
                switch (zone[0])
                {
                    case 0:
                        return support_zone_1;
                        break;
                    case 1:
                        return support_zone_2;
                        break;
                    case 2:
                        return support_zone_3;
                        break;
                    default:
                        break;
                }
                break;
            default: return null;
                break;
                
        }
        return null;
    }
}
