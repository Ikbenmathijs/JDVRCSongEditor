using System;

public static class Util
{
    public static String TimeToString(float time)
    {
        int minutes = (int)time / 60;
        int seconds = (int)time % 60;
        int milliseconds = (int)((time - (int)time) * 10000);
        return minutes + ":" + seconds.ToString("00") + "." + milliseconds.ToString("00");
    }
}