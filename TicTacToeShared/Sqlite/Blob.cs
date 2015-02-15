using System;

namespace TicTacToeShared.Sqlite
{
    public class Blob
    {
        [PrimaryKey]
        public string Key { get; set; }

        public byte[] Data { get; set; }
    }
}