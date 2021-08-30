using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace SerialProtocolAbstraction
{
    public class Command
    {
        public Type CommandType { get; private set; }
        public CommonCommandAttribute CommandData { get; private set; }
        public ArgumentAttribute[] Arguments { get; private set; }
        public OptionAttribute[] Options { get; private set; }
        internal ParameterAttribute[] Parameters { get; private set; }

        internal Command(Type type)
        {
            CommandType = type;
            CommandData = GetCommandData(type);
            Parameters = GetParameters(type);

            GetArgsAndOptions(Parameters, type);
        }

        private ParameterAttribute[] GetParameters(Type type)
        {
            ParameterAttribute[] args = (type.GetCustomAttributes(typeof(ParameterAttribute), false) as ParameterAttribute[]);

            return args;
        }

        private CommonCommandAttribute GetCommandData(Type type)
        {
            return Attribute.GetCustomAttribute(CommandType, typeof(CommonCommandAttribute)) as CommonCommandAttribute;
        }

        private void GetArgsAndOptions(ParameterAttribute[] args, Type type)
        {
            List<ArgumentAttribute> arguments = new List<ArgumentAttribute>();
            List<OptionAttribute> options = new List<OptionAttribute>();
            foreach (var arg in args) 
            {
                if (arg is ArgumentAttribute)
                    arguments.Add((ArgumentAttribute)arg);
                else // (arg is OptionAttribute)
                    options.Add((OptionAttribute)arg);
            }

            Arguments = arguments.ToArray();
            CheckArgumentOrder(Arguments, type);
            Array.Sort(Arguments);

            Options = options.ToArray();
        }

        private void CheckArgumentOrder(ArgumentAttribute[] args, Type type)
        {
            try
            {
                Dictionary<int, ArgumentAttribute> dic = args.ToDictionary(i => i._internalOrder);
            }
            catch (ArgumentException) 
            {
                throw new ArgumentWithoutDefinedOrderException(type);
            }
        }

    }
}
