using System.Collections.Generic;

namespace MrovLib.Definitions
{
	public abstract class CommandNode(string Name)
	{
		public string Name = Name.ToLowerInvariant();
		public CommandType Type = CommandType.SubCommand;

		public string CommandArgument;

		private List<CommandNode> _subcommands = [];

		public virtual List<CommandNode> Subcommands
		{
			get
			{
				if (Type == CommandType.SubCommand)
				{
					return [];
				}

				return _subcommands;
			}
			set { _subcommands = value; }
		}

		public override string ToString()
		{
			return Name;
		}
	}

	public enum CommandType
	{
		Command,
		SubCommand,
	}
}
