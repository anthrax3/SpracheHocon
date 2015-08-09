using System.Collections.Generic;

namespace SpracheHocon.Config
{
    public class ConfigObject : ConfigValue
    {
        private Dictionary<string,ConfigValue> _pairs = new Dictionary<string, ConfigValue>();
    }
}
