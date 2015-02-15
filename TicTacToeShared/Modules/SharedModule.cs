// --------------------------------------------------------------------------------------------------
//  <copyright file="DroidFrameworkModule.cs" company="DNS Technology Pty Ltd.">
//    Copyright (c) 2014 DNS Technology Pty Ltd. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------
using System.IO;
using System;

using TicTacToeShared.State;
using TicTacToeShared.Sqlite;
using TicTacToeShared.Web;
using TicTacToeLab;
using TicTacToeLab.Interfaces;

namespace TicTacToeShared.Modules
{
	using BodyshopWindows.Ioc;

	/// <summary>
	/// </summary>
	public sealed class SharedFrameworkModule : IModule
	{
		#region Public Methods and Operators

		/// <summary>
		/// </summary>
		/// <param name="builder">
		/// </param>
		public void Register (IocBuilder builder)
		{
			builder.RegisterType<AppState> ().As<IAppState> ();
			builder.RegisterType<FileDownloader> ().As<IFileDownloader> ();
			builder.RegisterType<SqlLite3BlobRepository> ().As<IBlobRepository> ();
			var databasePath = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "tictactoe.db3");
			builder.RegisterInstance (databasePath).Named<string> (RegisteredNames.DatabasePath.ToString ());
		}

		#endregion
	}
}