using System;
using System.Threading.Tasks;

namespace TicTacToeLab.Interfaces
{
	public interface IFileDownloader
	{
		Task<byte[]> GetFile(string url);
	}
}

