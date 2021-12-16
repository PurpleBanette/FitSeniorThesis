using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public enum OffMeshLinkMoveMethod
{
    Teleport,
    NormalSpeed,
    Parabola,
    Curve
}

[RequireComponent(typeof(NavMeshAgent))]
public class AgentLinkMoverObsidian : MonoBehaviour
{
    public OffMeshLinkMoveMethod m_Method = OffMeshLinkMoveMethod.Parabola;
    public AnimationCurve m_Curve = new AnimationCurve();
    public delegate void LinkEvent();
    public LinkEvent OnLinkStart;
    public LinkEvent OnLinkEnd;
    [SerializeField] float jumpCurveSpeed;

    IEnumerator Start()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        bossAiObsidian bossReference = GetComponent<bossAiObsidian>();
        agent.autoTraverseOffMeshLink = false;
        while (true)
        {
            if (agent.isOnOffMeshLink && bossReference.bossIsAttacking == false && bossReference.phaseChanging == false)
            {
                OnLinkStart?.Invoke();
                if (m_Method == OffMeshLinkMoveMethod.NormalSpeed)
                {
                    yield return StartCoroutine(NormalSpeed(agent));
                }
                else if (m_Method == OffMeshLinkMoveMethod.Parabola)
                {
                    yield return StartCoroutine(Parabola(agent, 2.0f, 0.5f));
                }
                else if (m_Method == OffMeshLinkMoveMethod.Curve)
                {
                    yield return StartCoroutine(Curve(agent, jumpCurveSpeed));
                }
                agent.CompleteOffMeshLink();
                OnLinkEnd?.Invoke();
            }
            if (agent.isOnOffMeshLink && bossReference.obsidianIsLeaping == true && bossReference.phaseChanging == false)
            {
                OnLinkStart?.Invoke();
                yield return StartCoroutine(NormalSpeed(agent));
                agent.CompleteOffMeshLink();
                OnLinkEnd?.Invoke();
            }
            yield return null;
        }
    }

    IEnumerator NormalSpeed(NavMeshAgent agent)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        while (agent.transform.position != endPos)
        {
            agent.transform.position = Vector3.MoveTowards(agent.transform.position, endPos, agent.speed * Time.deltaTime * 10);
            yield return null;
        }
    }

    IEnumerator Parabola(NavMeshAgent agent, float height, float duration)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        float normalizedTime = 0.0f;
        while (normalizedTime < 1.0f)
        {
            float yOffset = height * 4.0f * (normalizedTime - normalizedTime * normalizedTime);
            agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
    }

    IEnumerator Curve(NavMeshAgent agent, float duration)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        float normalizedTime = 0.0f;
        while (normalizedTime < 1.0f)
        {
            float yOffset = m_Curve.Evaluate(normalizedTime);
            agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
    }
}