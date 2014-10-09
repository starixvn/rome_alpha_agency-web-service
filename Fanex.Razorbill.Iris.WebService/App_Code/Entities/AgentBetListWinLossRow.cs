// --------------------------------------------------------------------------
// <copyright file="AgentBetListWinLossRow.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Razorbill.Iris.Webservice
{
    /// <summary>
    /// Agent Bet List Win Loss Row.
    /// </summary>
    public class AgentBetListWinLossRow
    {
        public string MemberId { get; set; }

        public long BetId { get; set; }

        public string TransTime { get; set; }

        public string BetType { get; set; }

        public string Sport { get; set; }

        public int GameId { get; set; }

        public string Home { get; set; }

        public string Away { get; set; }

        public string League { get; set; }

        public string Choice { get; set; }

        public string Handicap { get; set; }

        public string OddsType { get; set; }

        public double Odds { get; set; }

        public double Stake { get; set; }

        public int LiveAScore { get; set; }

        public int LiveBScore { get; set; }

        public float MemWL { get; set; }

        public float MemComm { get; set; }

        public float AgentWL { get; set; }

        public float AgentComm { get; set; }
    }
}