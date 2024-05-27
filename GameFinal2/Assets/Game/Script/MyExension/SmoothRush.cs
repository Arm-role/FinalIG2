using UnityEngine;

public class SmoothRush
{
    public float SmoothFloat(float floatInput, float floatOutput, float TimeLine)
    {
        if (floatInput < floatOutput)
        {
            if (floatInput <= floatOutput)
            {
                floatInput += TimeLine * Time.deltaTime;
                if (floatInput >= floatOutput) floatInput = floatOutput;
            }
            return floatInput;
        }
        else if (floatInput > floatOutput)
        {
            if (floatInput >= floatOutput)
            {
                floatInput -= TimeLine * Time.deltaTime;
                if (floatInput <= floatOutput) floatInput = floatOutput;
            }
            return floatInput;
        }
        return floatInput;
    }
}
