using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(bossAiObsidian))]
public class FieldOfViewEditorObsidian : Editor
{

    private void OnSceneGUI()
    {
        //References the boss script
        bossAiObsidian fov = (bossAiObsidian)target;

        //Visuals for the field of view
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.fovRadius);

        //Some scary math to calculate the visuals for the POV angles
        Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.fovAngle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.fovAngle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.fovRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.fovRadius);

        if(fov.playerInLos)
        {
            Handles.color = Color.green;
            Handles.DrawLine(fov.transform.position, fov.player.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        //Some scary math to calculate the visuals for the POV angles
        angleInDegrees += eulerY;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

}
