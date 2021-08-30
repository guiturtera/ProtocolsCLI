using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace SerialProtocolAbstraction
{
    public class CommandNotFoundException : Exception
    {
        public CommandNotFoundException(string command, string commandNotFound)
            : base(String.Format("Command failed at {0}.\nCommand not found -> {1}", command, commandNotFound)) { }
    }

    public class CommandWithSubcommandsException : Exception
    {
        public CommandWithSubcommandsException(string command, string wrongCommand)
            : base(String.Format("Command can not be called. To add subcommands, use the Attribute [Group(...)]"
            + "\nFull command -> {0}\nError Command -> {1}"
            , command, wrongCommand)) { }

        public CommandWithSubcommandsException(Type type)
            : base(String.Format("Command can not be called. To add subcommands, use the Attribute [Group(...)]"
            + "\nType -> {0}"
            , type.Name)) { }
    }

    public class CommandRepeatedException : Exception
    {
        public CommandRepeatedException(string command)
            : base(String.Format("There is two or more {0} commands!"
            , command)) { }
    }

    public class CommandGroupTogetherException : Exception
    {
        public CommandGroupTogetherException(Type type)
            : base(String.Format("You must specify only one attribute in a class. Either Group or Command!\nAt class {0}"
            , type.Name)) { }
    }

    public class ICommandWithoutCommandAttributeException : Exception
    {
        public ICommandWithoutCommandAttributeException(Type type)
            : base(String.Format("You must specify [Group] or [Command] attribute for ICommand implementations!!\nAt class {0}"
            , type.Name)) { }
    }

    public class FactoryCommandTreeNotCreatedException : Exception
    {
        public FactoryCommandTreeNotCreatedException()
            : base("Make sure that you have successfully initialized CommandsFactory.CreateCommandTree(Assembly assembly, string nameSpace)!") { }
    }

    public class TooManyArgumentsException : Exception
    {
        public TooManyArgumentsException(string fullCommand)
            : base(String.Format("You have given more arguments than needed!\nCommand -> {0}", fullCommand)) { }
    }

    public class NotEnoughArgumentsException : Exception
    {
        public NotEnoughArgumentsException(string fullCommand)
            : base(String.Format("Not enough parameters sent!\nCommand -> {0}", fullCommand)) { }
    }

    public class ArgumentWithoutDefinedOrderException : Exception
    {
        public ArgumentWithoutDefinedOrderException(Type type)
            : base(String.Format("Type {0} has repeated arguments order!",
        type.Name)) { }
    }

    public class ParameterRepeatedException : Exception
    {
        public ParameterRepeatedException(string parameterKey)
            : base(String.Format("Repeated parameter Option/Argument in the same command",
        parameterKey)) { }
    }

    public class MissingArgumentException : Exception
    {
        public MissingArgumentException(string argument)
            : base(String.Format("Missing argument <{0}>",
        argument)) { }
    }

    public class OutsideSCPIFormatException : Exception
    {
        public OutsideSCPIFormatException(Command message)
            : base(String.Format("SCPI command names must have lenght of 1 or 2, and the biggest string must contain the lower one." +
                "\nExample: CONF, CONFigure\nError at {0}", message.CommandType.FullName))
        { }
    }

    public class InvalidParameterException : Exception 
    {
        public InvalidParameterException(string key)
            : base(String.Format("Invalid parameter sent: {0}", key))
        { } 
    }

    /*
    public class NotPrimitiveTypeException : Exception
    {
        public NotPrimitiveTypeException(Type type)
            : base(String.Format("You must specify only primitive types in Arguments!\nType -> {0}", type.Name)) { }
    }*/
}

