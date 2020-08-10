using System;
using System.Collections.Generic;
using System.Text;

namespace GraphQL.Globalization.Entities.Models.Generic
{
    public class KeyValueDto
    {
        public dynamic Key { get; set; }
        public dynamic Value { get; set; }

        public KeyValueDto()
        {
        }

        public KeyValueDto(dynamic key, dynamic value)
        {
            Key = key;
            Value = value;
        }
        public override string ToString()
        {
            var ret = Key.ToString() + ":" + (Value == null ? string.Empty : Value.ToString().Trim());
            return ret;
        }
    }

    public class KeyValueStringDto
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public KeyValueStringDto()
        {
        }

        public KeyValueStringDto(string key, string value)
        {
            Key = key;
            Value = value;
        }
        public override string ToString()
        {
            var ret = Key + ":" + (Value == null ? string.Empty : Value.Trim());
            return ret;
        }
    }

    public class KeyValueIntDto
    {
        public int Key { get; set; }
        public string Value { get; set; }

        public KeyValueIntDto()
        {
        }

        public KeyValueIntDto(int key, string value)
        {
            Key = key;
            Value = value;
        }
        public override string ToString()
        {
            var ret = Key.ToString() + ":" + (Value == null ? string.Empty : Value.Trim());
            return ret;
        }
    }
}
