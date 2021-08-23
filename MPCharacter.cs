using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Bot_Test
{
    public class MPCharacter
    {
        string nom;
        public SocketGuildUser utilisateurDiscord;
        public ConditionPhysique capacitePhysique = ConditionPhysique.normale;
        public EtatDeSante etatDeSante = EtatDeSante.normal;
        public bool isBot;
        public MPCharacter(SocketGuildUser user = null)
        {
            utilisateurDiscord = user;
        }
    }

    public enum Metier
    {

    }

    public enum EtatDeSante
    {
        mort = 0,
        agonisant = 1,
        grave = 2,
        serieux = 3,
        blesse = 4,
        malEnPoint = 5,
        legeremmentBlesse = 6,
        normal = 7,             // par défaut
        pleineForme = 8         // bonus de santé
    }

    public enum ConditionPhysique // définit la résistance et la puissance du personnage
    {
        aucune = 0,         // par exemple pour des personnes transparentes
        faiblard = 1,       // par exemple pour quelqu'un de très affaibli
        faible = 2,         // personne maigre, personne âgée, femme
        normale = 3,        // femme, homme de corpulence normale
        bonne = 4,          // personne sportive, assez musclée, policier, militaire
        tresBonne = 5,      // militaire vétéran (rare), policier d'élite, boxeur, gros bras, commando
        excellente = 6,     // boxeur, unité Silver, combattant au mental d'acier, brain classe I
        extreme = 7,        // brain Classe II, brain Classe III, unité Silver d'élite, soldat avec augmentation psychique
        surhumaine = 8,     // brain Classe III, soldat avec augmentation psychique
        classifiee = 9      // M{|[~# D|\^[~[pP{ -- #
    }


    public class Interaction 
    {
        int probaReussite { get; set; }
        int probaNeutre { get; set; }
        int probaEchec { get; set; }
        public class InteractionFilter : Interaction
        {

            public  InteractionFilter(string phraseJ, MPCharacter player, MPCharacter player2) : base(player,player2)
            {
                phrase = phraseJ;
            }
            public Interaction DetectPhraseAndReturnCorrectClass()
            {
                phrase = phrase.Replace("  "," ");  // On enlève les espaces en trop
                Interaction interaction = new CoupDePoing(p1, p2);
                Interaction interaction2 = new CoupDePied(p1, p2);
                Interaction interaction3 = new CoupDeTete(p1, p2);
                 
                foreach (var motCle in interaction.motsCles)
                {
                    if (phrase.Trim().ToLower().Contains(motCle))
                    return interaction;
                }
                foreach(var motCle in interaction2.motsCles)
                {
                    if (phrase.Trim().ToLower().Contains(motCle))
                        return interaction2;
                }
                foreach (var motCle in interaction3.motsCles)
                {
                    if (phrase.Trim().ToLower().Contains(motCle))
                        return interaction3;
                }
                Console.WriteLine("Pas du vocabulaire MP");
                return null;
            }
        }

        protected Random r = new Random();
        protected MPCharacter p1;
        protected MPCharacter p2;
        protected List<String> motsCles;
        public List<String> jeRepondsReussi;
        public List<String> jeRepondsNeutre;
        public List<String> jeRepondsEchec;
        protected string phrase;
        CoupDePoing InteractionPoing;
        private Interaction interaction;

        public Interaction( MPCharacter player, MPCharacter player2)
        {
            p1 = player;
            p2 = player2;
        }

        public string ObtenirEtatSante(MPCharacter joueur)
        {
            string description;
            
            if(joueur.etatDeSante == EtatDeSante.mort)
            {
                description = "L'ennemi est mort!";
                joueur = null;
            }
            else if(joueur.etatDeSante <= EtatDeSante.grave)
            {
                description = "L'ennemi est gravement blessé! Il semble ne plus en avoir pour longtemps" ;
            }
            else if (joueur.etatDeSante  <= EtatDeSante.blesse)
            {   
                description = "Il est blessé au torse";
            }
            else if (joueur.etatDeSante <= EtatDeSante.legeremmentBlesse)
            {
                description= "L'ennemi semble avoir une côté fracturée"; 
            }
            else
            {
                description = "L'ennemi est indemne";
            }
            return description;
        }

        public virtual string CeQuiSePasseQuandJeReponds(string phraseJ)    
        {
            phrase = phraseJ;
            return phrase;
        }

    }

    public class Attaque : Interaction
    {
        public Attaque( MPCharacter player, MPCharacter player2) : base(  player,  player2)
        {
            
        }
    }

    public class CoupDePied : Attaque
    {
        private readonly List<string> attaqueReussieEnnemi;

        public CoupDePied(MPCharacter player, MPCharacter player2) : base (player, player2)
        {
            motsCles = new List<string>(new string[]
           {
                "coup de pied",
                "cou de pied",
                "je le shoote",
                "semelle dans la figure",
                "semelle dans la face",
                "pied dans la figure",
                "le frappe avec ma jambe",
                "le projette avec ma jambe",
                "je le defonce avec ma jambe",
                "je lui envoie un coup de jambe",
                "coup avec ma jambe"
           });


            attaqueReussieEnnemi = new List<string>(new string[]
               {
                "Il te frappe du poing gauche dans le torse, tu recules, mal en point",
                "Il t'envoie plusieurs coups de poing dans le torse et t'enchaînes",
                "Il s'avance et te frappe, tu fais quelques pas en arriere, avec un main sur ton ventre"
               });

            jeRepondsReussi = new List<string>(new string[]
               {
                "Il se prend ton coup de pied et se fait projeter contre un mur",
                "Ta jambe se propulse et frappe violemment son torse, il recule, déstabilisé",
                "Tu le cognes avec ta jambe, il recule en se heurtant à un meuble",
                "Tu fais un demi-cercle et lui fauches l'épaule avec ton pied,  il se tord et te repousse",
                "Tu t'avances et lui fais un coup de pied dans l'abdomen avant qu'il n'ait eu le temps de parer",
               });

            jeRepondsNeutre = new List<string>(new string[]
          {
                "Il pare ton coup de pied à deux mains mais tombe à moitié au sol",
                "Ta jambe lui frôle le torse, il se décale, le regard surpris",
                "Il pare ton coup de pied avec difficulté et en cognant le mur d'à gauche",
                "Il dévie ta frappe de jambe tout en se prenant un léger impact dans le bras",
                "L'ennemi pare ton coup de pied en tombant vers l'arrière, il se relève ensuite"
          });

            jeRepondsEchec = new List<string>(new string[]
              {
                "Il attrape ta jambe en plein vol avec un bras, et s'apprête à te frapper tout de suite avec l'autre bras!",
                "Il se baisse rapidement et tu le manques de peu",
                "Il aggrippe ta jambe, t'envoies un coup d'épaules et te fais renverser la table derrière toi",
                "Il pare ton coup de pied en reculant légeremment",
                "Il fait une roulade vers l'arrière et esquive ton coup habilement",
                "Il se décale et ta jambe tape dans un meuble",
                "L'ennemi pare ton coup de pied avec son bras gauche et avance"
              });
        }
        public override string CeQuiSePasseQuandJeReponds(string phraseJoueur)
        {
            Console.WriteLine("ACTION::CeQuiSePasseQuandJeReponds()");
            int jetDeDesSur100 = r.Next(100);
            //if ((int)p2.capacitePhysique > (int)ConditionPhysique.normale)
            //{
            if (jetDeDesSur100 < 50)
            {
                phraseJoueur = jeRepondsEchec[r.Next(0, jeRepondsEchec.Count - 1)];
            }
            else if (jetDeDesSur100 < 60)
            {
                phraseJoueur = jeRepondsNeutre[r.Next(0, jeRepondsNeutre.Count - 1)];
            }
            else
            {
                phraseJoueur = jeRepondsReussi[r.Next(0, jeRepondsReussi.Count - 1)];
                try
                {
                    int state = ((int)p2.etatDeSante) - 1;
                    p2.etatDeSante = (EtatDeSante)Enum.Parse(typeof(EtatDeSante), (state).ToString());
                    Console.WriteLine("ETAT : " + state);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            //}
            Console.WriteLine(phraseJoueur);
            return phraseJoueur;
        }
    }

    public class CoupDePoing : Attaque
    {
        public CoupDePoing(MPCharacter player, MPCharacter player2) : base(player, player2)
        {
            motsCles = new List<string>
           {
                "coup de poing",
                "cou de poing",
                "je le tape",
                "je le frappe",
                "je lui donne un poing",
                "je lui envoie un poing",
                "je lui envoi un poing",
                "je le cogne"
           };

            jeRepondsReussi = new List<string>
           {
                "tu le frappes dans le ventre, il recule",
                "Il se le prend et recule",
                "Il se plie sous l'impact et te repousse",
                "Arg! ( Il recule en se préparant à attaquer)",
                "tu le frappes à l'épaule, il décale ton bras en emettant un grognement",
                "Il se fait propulser légeremment par l'impact",
           };

            jeRepondsNeutre = new List<string>
           {
                "tu vises son ventre mais il attrape ton bras en l'enlaçant",
                "Il recule et esquive de justesse",
                "Il pare ton coup maladroitement",
                 "Il pare ton coup en perdant l'equilibre!",
                "Il te pousse en se prenant une partie du poing dans le ventre",
                "Il fait une esquive sur la droite un peu trop tard et prend ton coup de poing dans le bras"
           };

            jeRepondsEchec = new List<string>
           {
                "Il se baisse et esquive ton coup",
                "Il esquive sur la droite",
                "Il fait une roulade tout en esquivant ton coup de poing",
                "Il pare ton coup et bloque ton bras",
                "Il met ses deux bras devant lui et pare ton coup",
                "Il fait une feinte et esquive ton coup"
           };           
}

        public override string CeQuiSePasseQuandJeReponds(string phraseJoueur)
        {
            Console.WriteLine("ACTION::CeQuiSePasseQuandJeReponds()");
            int jetDeDesSur100 = r.Next(100);
            //if ((int)p2.capacitePhysique > (int)ConditionPhysique.normale)
            //{
                if(jetDeDesSur100 < 40)
                {
                phraseJoueur = jeRepondsEchec[r.Next(0,jeRepondsEchec.Count-1)];
                }
                else if (jetDeDesSur100 < 50)
                {
                phraseJoueur = jeRepondsNeutre[r.Next(0, jeRepondsNeutre.Count - 1)];
                }
                else
                {
                phraseJoueur = jeRepondsReussi[r.Next(0, jeRepondsReussi.Count - 1)];

                try
                {
                    int state = ((int)p2.etatDeSante)-1;
                    p2.etatDeSante = (EtatDeSante)Enum.Parse(typeof(EtatDeSante),(state).ToString());
                    Console.WriteLine("ETAT : "+ state);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                }
            //}
                Console.WriteLine(phraseJoueur);
                return phraseJoueur;
        }
    }

    public class CoupDeTete : Attaque
    {
        public CoupDeTete(MPCharacter player, MPCharacter player2) : base(player, player2)
        {
            motsCles = new List<string>
           {
                "coup de tete",
                "coup de tête",
                "cou de tete",
                "coup de boule",
                "cogne avec la tete",
                "cogne avec ma tete",
                "cou de boule"
           };

            jeRepondsReussi = new List<string>
           {
                "tu le frappes au visage avec la tête,  ARG (il est maintenant etourdi)",
                "tu percutes ta tête à son visage, il se dégage en mettant une main sur son nez",
                "tu cognes ta tête contre le sienne, il tombe à genoux et s'appuie pour se relever"
           };

            jeRepondsEchec = new List<string>
           {
                "Il amortit le choc de ton crâne avec ses bras",
                "Tu veux lui envoyer un coup de tête mais il te repousse brusquement contre le mur"
            };
        }

        public override string CeQuiSePasseQuandJeReponds(string phraseJoueur)
        {
            Console.WriteLine("ACTION::CeQuiSePasseQuandJeReponds()");
            int jetDeDesSur100 = r.Next(100);
            //if ((int)p2.capacitePhysique > (int)ConditionPhysique.normale)
            //{
            if (jetDeDesSur100 < 40)
            {
                phraseJoueur = jeRepondsEchec[r.Next(0, jeRepondsEchec.Count - 1)];
            }   
            else if (jetDeDesSur100 < 50)
            {
                phraseJoueur = jeRepondsNeutre[r.Next(0, jeRepondsNeutre.Count - 1)];
            }
            else
            {
                phraseJoueur = jeRepondsReussi[r.Next(0, jeRepondsReussi.Count - 1)];

                try
                {
                    int state = ((int)p2.etatDeSante) - 1;
                    p2.etatDeSante = (EtatDeSante)Enum.Parse(typeof(EtatDeSante), (state).ToString());
                    Console.WriteLine("ETAT : " + state);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            //}
            Console.WriteLine(phraseJoueur);
            return phraseJoueur;
        }
    }
}

