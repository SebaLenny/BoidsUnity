using System;

[Serializable]
public class RuleParameters
{
    public bool isActive = true;
    public bool considerOtherGroups = false;
    public float range = 2.0f;
    public float strength = 1.0f;
}
