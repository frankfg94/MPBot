using Bot_Test.MP;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bot_Test.MP.Scripts.Discord
{

    public static class Translator
    {
        /// <summary>
        /// Set de caracteres unicodes permettant de créer des emotes discord avec la classe Emote
        /// </summary>
        public static List<string> alphabetUnicode = new List<string>() { A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z};
        internal static Dictionary<EntityPartType, string> endOfShootingSentence = new Dictionary<EntityPartType, string>
        {
            {EntityPartType.Windshield," dans la vitre du véhicule" },
            {EntityPartType.Tail," dans la queue" },
            {EntityPartType.RightWing," dans l'aile droite de l'avion" },
            {EntityPartType.LeftWing," dans l'aile gauche de l'aéronef" },
            {EntityPartType.EquippedWeapon," dans son arme" },
            {EntityPartType.Head," dans sa tête" },
            {EntityPartType.Chest," dans son torse" },
            {EntityPartType.Leg," dans ses jambes" },
            {EntityPartType.Hand," sur ses mains" },
            {EntityPartType.Balls," dans ses couilles" },
            {EntityPartType.Heart," dans son coeur" },
            {EntityPartType.Foot," vers ses pieds" },
            {EntityPartType.Arm," dans ses bras" },
            {EntityPartType.Neck," dans son cou" }
        };

        
        public const string A = "🇦";
        public const string B = "🇧";
        public const string C = "🇨";
        public const string D = "🇩";
        public const string E = "🇪";
        public const string F = "🇫";
        public const string G = "🇬";
        public const string H = "🇭";
        public const string I = "🇮";
        public const string J = "🇯";
        public const string K = "";
        public const string L = "";
        public const string M = "";
        public const string N = "";
        public const string O = "";
        public const string P = "";
        public const string Q = "";
        public const string R = "";
        public const string S = "";
        public const string T = "";
        public const string U = "";
        public const string V = "";
        public const string W = "";
        public const string X = "";
        public const string Y = "";
        public const string Z = "";
    }

}
