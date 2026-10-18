using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MultipleChoiceChallenge : MonoBehaviour, ISignChallenge
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] RawImage videoDisplay;                 // NEW: so we can hide stale frames
    [SerializeField] Button[] answerButtons;
    [SerializeField] float feedbackSeconds = 0.8f;
    [SerializeField] float prepareTimeout = 3f;             // NEW: never wait forever
    [SerializeField] Color correctColor = new(0.3f, 1f, 0.4f);
    [SerializeField] Color wrongColor = new(1f, 0.35f, 0.35f);

    public event Action<bool> Answered;

    readonly List<SignData> choices = new();
    SignData target;
    bool acceptingAnswers;                                  // replaces `locked`; true only when everything is showing

    void Awake()
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;
            answerButtons[i].onClick.AddListener(() => OnChoice(index));
        }

        videoPlayer.errorReceived += (vp, message) =>
            Debug.LogError($"Video error: {message}", this);
    }

    public void Present(SignData target, IReadOnlyList<SignData> pool)
    {
        StopAllCoroutines();          // cancel anything left over from the previous sign
        this.target = target;
        acceptingAnswers = false;

        BuildChoices(pool);
        StartCoroutine(LoadThenShow());
    }

    IEnumerator LoadThenShow()
    {
        // 1. Hide everything so an old video and new answers never appear together
        videoDisplay.enabled = false;
        foreach (Button b in answerButtons) b.gameObject.SetActive(false);

        // 2. prep clip, with a timeout
        videoPlayer.Stop();
        videoPlayer.clip = target.video;
        videoPlayer.Prepare();

        float waited = 0f;
        while (!videoPlayer.isPrepared && waited < prepareTimeout)
        {
            waited += Time.unscaledDeltaTime;
            yield return null;        // check again next frame
        }

        if (!videoPlayer.isPrepared)
            Debug.LogWarning($"Video for '{target.word}' wasn't ready after {prepareTimeout}s; continuing anyway.", this);

        // 3. reveal video and answers
        videoPlayer.Play();
        videoDisplay.enabled = true;
        ShowChoices();

        // 4. now you can answer
        acceptingAnswers = true;
    }

    void BuildChoices(IReadOnlyList<SignData> pool)
    {
        choices.Clear();
        choices.Add(target);
        var others = new List<SignData>(pool);
        others.Remove(target);
        Shuffle(others);
        for (int i = 0; i < others.Count && choices.Count < answerButtons.Length; i++)
            choices.Add(others[i]);
        Shuffle(choices);
    }

    void ShowChoices()
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            bool used = i < choices.Count;
            answerButtons[i].gameObject.SetActive(used);
            if (!used) continue;

            answerButtons[i].GetComponentInChildren<TMP_Text>().text = choices[i].word;
            answerButtons[i].image.color = Color.white;
        }
    }

    void OnChoice(int index)
    {
        if (!acceptingAnswers) return;
        acceptingAnswers = false;

        bool correct = choices[index] == target;
        answerButtons[index].image.color = correct ? correctColor : wrongColor;

        if (!correct)
            answerButtons[choices.IndexOf(target)].image.color = correctColor;

        StartCoroutine(FinishAfterDelay(correct));
    }

    IEnumerator FinishAfterDelay(bool correct)
    {
        yield return new WaitForSeconds(feedbackSeconds);
        Answered?.Invoke(correct);
    }

    static void Shuffle<T>(IList<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}