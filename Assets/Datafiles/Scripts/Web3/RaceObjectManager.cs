using System.Collections;
using System.Collections.Generic;
using Aptos.Unity.Rest;
using Aptos.Unity.Rest.Model;
using SimpleJSON;
using UnityEngine;

public class RaceObjectManager : MonoBehaviour
{
    public Race raceGo;
    public Transform raceSpawn;

    private List<Race> races = new();

    public IEnumerator GetRaceDataAsync()
    {
        ResponseInfo responseInfo = new();
        string data = "";

        ViewRequest viewRequest = new()
        {
            Function = "0x3e79e6c4f4d55299f09b3aef9a8ba33a2ba0f53d081336c3811c3e4712a8d48b::aptos_horses_game::get_all_races_metadata",
            TypeArguments = new string[] { },
            Arguments = new string[] { }
        };

        Coroutine getUser = StartCoroutine(RestClient.Instance.View((_data, _responseInfo) =>
        {
            if (_data != null) data = _data;
            responseInfo = _responseInfo;

        }, viewRequest));

        yield return getUser;

        if (responseInfo.status == ResponseInfo.Status.Failed) Debug.LogError("Error: Fetching data failed!");
        else AssignData(data);
    }

    private void AssignData(string data)
    {
        JSONNode node = JSON.Parse(data);

        for (int i = 0; i < node[0].Count; i++)
        {
            if (races.Count < i + 1) races.Add(Instantiate(raceGo, raceSpawn));
            races[i].SetupRace(
                node[0][i]["race_id"].AsULong,
                node[0][i]["name"].Value,
                node[0][i]["price"].AsInt,
                node[0][i]["laps"].AsInt,
                node[0][i]["started"].AsBool,
                node[0][i]["players"]
            );
        }
    }
}