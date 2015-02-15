using System.Threading.Tasks;

namespace TicTacToeLab.Interfaces
{
	public interface IBlobRepository
	{
		Task SaveAsync<T> (T item) where T : IBlobStorable, new();

		Task<T> LoadAsync<T> (string key) where T : IBlobStorable, new();

	    Task SetupDatabase();
	}
}