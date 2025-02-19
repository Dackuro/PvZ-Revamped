using UnityEngine;

public interface IBuildingState
{
    void EndState();
    void OnAction(Vector3Int gridPos, AudioClip errorClip);
    void UpdateState(Vector3Int gridPos);
}