using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace BT.MP.Scripts.Discord
{

    /// <summary>
    /// Indique si l'on peut répondre à un message afin de continuer à exécuter un script comme par exemple choisir la cible sur laquelle tirer à l'aide des émoticones
    /// </summary>
    internal interface IAnswerable
    {
        /// <summary>
        /// Analyse les données fournies par le bot et continue le script
        /// </summary>

        void ParseAnswer(Cacheable<IUserMessage, ulong> msg, ISocketMessageChannel channel, SocketReaction reaction);

        /// <summary>
        /// Indique les identifiants de messages auquels on a déjà répondu, cela permet d'éviter de faire en sorte que l'on puisse continuer le script en cliquant sur un message 3 pages au dessus
        /// </summary>
        List<ulong> AnsweredMessagesIds { get; set; }
    }
}
