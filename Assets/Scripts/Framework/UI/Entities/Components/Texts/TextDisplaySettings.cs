using System;

namespace Framework.UI.Components
{
    public enum DisplayMode
    {
        Everything = 0,
        LineByLine = 1,
        WordByWord = 2,
        LetterByLetter = 3
    }

    [Serializable]
    public class TextDisplaySettings
    {
        public DisplayMode DisplayMode = DisplayMode.Everything;

        public float DisplayIntervall = 0f;
    }
}