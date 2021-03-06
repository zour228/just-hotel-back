using CommandRunner.Abstraction;
using CommandRunner.Handler;
using FluentMigrator.Runner;

namespace Cli.Commands.Util
{
    public class MigrateCommand : AbstractCommand
    {
        private const string DirectionUp = "--up";
        private const string DirectionDown = "--down";

        public MigrateCommand(IMigrationRunner runner)
        {
            Runner = runner;
        }

        public IMigrationRunner Runner { get; }

        public override void Execute(ArgumentProvider provider)
        {
            var version = provider.NextAsLong();
            var direction = DirectionUp;

            if (provider.HasNext())
            {
                direction = provider.NextAsString(new[] { DirectionUp, DirectionDown });
            }

            if (direction.Equals(DirectionUp))
            {
                Runner.MigrateUp(version);
            }
            else
            {
                Runner.MigrateDown(version);
            }
        }

        public override string GetName()
        {
            return "migrations:execute";
        }

        public override string? GetDescription()
        {
            return "Накатит или откатит конкретную миграцию.";
        }
    }
}