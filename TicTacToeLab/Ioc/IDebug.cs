namespace Bodyshop
{
	public interface IDebug
	{
		void WriteLine(string message);

		void WriteLineTime (string message, params object[] args);
	}
}