namespace Source.Codebase.Common
{
    public static class Extension
    {
        public static float ToPercent(this int value) => 
            value / 100f;
    }
}