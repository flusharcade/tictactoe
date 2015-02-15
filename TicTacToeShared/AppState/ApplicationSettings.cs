//--------------------------------------------------------------------------------------------------
// <copyright file="CompressedHttpClient.cs" company="DNS Technology Pty Ltd.">
//   Copyright (c) 2014 DNS Technology Pty Ltd. All rights reserved.
// </copyright>
//--------------------------------------------------------------------------------------------------
using TicTacToeLab;
using TicTacToeShared.Sqlite;
using TicTacToeLab.Interfaces;

namespace TicTacToeShared.State
{
	public class ApplicationSettings : IBlobStorable
    {
        public ApplicationSettings()
        {
            this.Key = BlobKeys.ApplicationSettings.ToString();
        }

		public int PlayerTurn;

		public int[] MarkedTypes = new int[9];

		public bool[] MarkedItems = new bool[9];

        public string Key { get; set; }
    }
}