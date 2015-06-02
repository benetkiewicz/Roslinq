namespace Roslinq.Core
{
    public static class Modifiers
    {
        /// <summary>
        /// Class modifiers constants.
        /// </summary>
        public static class Class
        {
            public const int Public = 1;
            public const int Protected = 2;
            public const int Private = 3;
            public const int Internal = 4;
            public const int Static = 5;
            public const int Sealed = 6;
            public const int Abstract = 7;
        }
        
        /// <summary>
        /// Method modifiers constants.
        /// </summary>
        public static class Method
        {
            public const int Public = 1;
            public const int Protected = 2;
            public const int Private = 3;
            public const int Internal = 4;
            public const int Static = 5;
            public const int Abstract = 7;
            public const int Virtual = 8;
        }
    }
}