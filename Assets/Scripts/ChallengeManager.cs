using UnityEngine;

public class ChallengeManager : MonoBehaviour
{
    [SerializeField] SignData[] signs;
    [SerializeField] MonoBehaviour challengeSource;
    [SerializeField] SignRail rail;
    [SerializeField] RunManager run;

    ISignChallenge challenge;
    SignData lastSign;

    void OnValidate()
    {
        if (challengeSource != null && challengeSource is not ISignChallenge)
        {
            Debug.LogError($"{challengeSource.name} doesn't implement ISignChallenge.", this);
            challengeSource = null;
        }
    }

    void Awake()     => challenge = challengeSource as ISignChallenge;
    void OnEnable()  => challenge.Answered += OnAnswered;
    void OnDisable() => challenge.Answered -= OnAnswered;
    void Start()     => NextSign();

    void OnAnswered(bool correct)
    {
        if (correct) rail.TryAddCell();
        if (run.IsRunning) NextSign();
    }

    void NextSign()
    {
        SignData next;
        do { next = signs[Random.Range(0, signs.Length)]; }
        while (signs.Length > 1 && next == lastSign);   // never the same sign twice in a row

        lastSign = next;
        challenge.Present(next, signs);
    }
}