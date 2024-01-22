using System.Collections;
using System.Collections.Generic;

using WebSocketSharp;
using Newtonsoft.Json;
using UnityEngine;
using System;
using System.Globalization;

public static class P_RequestHandler
{
    public static bool Processing = false;
    public static WebSocket WS;
    public static P_NetworkManager NP;
    public static S_MainUiHandler SMUIH;

    public static string parseOutgoing(Dictionary<string, string> _a)
    {
        return JsonConvert.SerializeObject(_a);
    }

    public static Dictionary<string, string> parseIncoming(string _a)
    {
        return JsonConvert.DeserializeObject<Dictionary<string, string>>(_a);
    }

    public static string Serialize_Vec3_Str(Vector3 _v)
    {
        string serialized = $"{Math.Round(_v.x, 2, MidpointRounding.ToEven).ToString(CultureInfo.InvariantCulture.NumberFormat)},{Math.Round(_v.y, 2, MidpointRounding.ToEven).ToString(CultureInfo.InvariantCulture.NumberFormat)},{Math.Round(_v.z, 2, MidpointRounding.ToEven).ToString(CultureInfo.InvariantCulture.NumberFormat)}";
        return serialized;
    }

    public static int clientIndexByID(int _id)
    {
        int ToRet = -1;
        for (int i = 0; i < NP.clients.Count; i++)
        {
            if(NP.clients[i].clientID == _id)
            {
                ToRet = i;
                break;
            }
        }
        return ToRet;
    }

    public static void cRequestAUTH(string _name)
    {
        Dictionary<string, string> _obj = new Dictionary<string, string>{
            {"cmd", "AUTH"},
            {"name", _name}
        };

        WS.Send(parseOutgoing(_obj));
    }

    public static void cRequestSENDCHAT(string _c)
    {
        Dictionary<string, string> _obj = new Dictionary<string, string>{
            {"cmd", "Chat"},
            {"chat", $"[{NP.CLIENT_NAME}]: {_c}"}
        };

        WS.Send(parseOutgoing(_obj));
    }

    public static void cRequestMOVING(Vector3 _pos, Vector3 _rot)
    {
        Dictionary<string, string> _obj = new Dictionary<string, string>{
            {"cmd", "Move"},
            {"id", NP.clients[0].clientID.ToString()},
            {"pos", Serialize_Vec3_Str(_pos)},
            {"rot", Serialize_Vec3_Str(_rot)}
        };

        WS.Send(parseOutgoing(_obj));
    }

    /* Request Router */

    public static void ReqRoute(Dictionary<string, string> _incoming)
    {
        Processing = true;
        switch(_incoming["cmd"])
        {
            case "Join":
                int join_ID = Convert.ToInt32(_incoming["id"]);
                int join_NEW = (_incoming["new"] == "False") ? 0 : 1;
                string[] join_POS_split = _incoming["pos"].Split(",");
                Vector3 join_POS = new Vector3(
                    (float)Convert.ToDecimal(join_POS_split[0], CultureInfo.InvariantCulture.NumberFormat),
                    (float)Convert.ToDecimal(join_POS_split[1], CultureInfo.InvariantCulture.NumberFormat),
                    (float)Convert.ToDecimal(join_POS_split[2], CultureInfo.InvariantCulture.NumberFormat)
                );

                NP.SpawnClientPlayer(join_POS, join_ID);
                NP.loadedServer = true;
                break;
            case "New":
                int new_ID = Convert.ToInt32(_incoming["id"]);
                string new_Name = _incoming["name"];
                string[] new_POS_split = _incoming["pos"].Split(",");
                Vector3 new_POS = new Vector3(
                    (float)Convert.ToDecimal(new_POS_split[0], CultureInfo.InvariantCulture.NumberFormat),
                    (float)Convert.ToDecimal(new_POS_split[1], CultureInfo.InvariantCulture.NumberFormat),
                    (float)Convert.ToDecimal(new_POS_split[2], CultureInfo.InvariantCulture.NumberFormat)
                );

                NP.SpawnClientOther(new_POS, new_ID, new_Name);
                break;
            case "Move":
                //NP.printERROR("Move recieved");
                int move_ID = Convert.ToInt32(_incoming["id"]);

                NP.printERROR(_incoming["pos"]);
                string[] move_POS_split = _incoming["pos"].Split(",");
                Vector3 move_POS = new Vector3(
                    (float)Convert.ToDecimal(move_POS_split[0], CultureInfo.InvariantCulture.NumberFormat),
                    (float)Convert.ToDecimal(move_POS_split[1], CultureInfo.InvariantCulture.NumberFormat),
                    (float)Convert.ToDecimal(move_POS_split[2], CultureInfo.InvariantCulture.NumberFormat)
                );
                NP.printERROR("Move POS");

                string[] move_ROT_split = _incoming["rot"].Split(",");
                Vector3 move_ROT = new Vector3(
                    (float)Convert.ToDecimal(move_ROT_split[0], CultureInfo.InvariantCulture.NumberFormat),
                    (float)Convert.ToDecimal(move_ROT_split[1], CultureInfo.InvariantCulture.NumberFormat),
                    (float)Convert.ToDecimal(move_ROT_split[2], CultureInfo.InvariantCulture.NumberFormat)
                );
                //NP.printERROR("Move rot");

                NP.UpdateClientOtherPosRot(move_POS, move_ROT, move_ID);
                //NP.printERROR("Moved");
                break;
            case "Chat":
                SMUIH.AddToChat(_incoming["chat"]);
                break;
            default:
                NP.printERROR("DEFAULTING");
                break;
        }
        Processing = false;
    }
}
