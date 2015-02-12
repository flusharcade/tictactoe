using System;
using System.Threading.Tasks;

namespace TicTacToeLab
{
	public interface IFileDownloader
	{
		Task<byte[]> GetFile(string url);
	}
}

