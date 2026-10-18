using System;
using System.Collections.Generic;

public interface ISignChallenge
{
    event Action<bool> Answered;                                  // true = correct
    void Present(SignData target, IReadOnlyList<SignData> pool);  // pool = all signs, for distractors
}