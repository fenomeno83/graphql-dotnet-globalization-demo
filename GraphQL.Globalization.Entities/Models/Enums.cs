using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GraphQL.Globalization.Entities.Models
{
    public class Enums
    {
        public enum LogLevelL4N
        {
            DEBUG = 1,
            ERROR,
            FATAL,
            INFO,
            WARN,
            TRACE,

        }
        public enum AllOrAtLeastOneRequiredType
        {
            AtLeastOneRequired = 0,
            All = 1,
            AllIfOneIsNotNull = 2

        }

        public enum FakeEnum
        {
            //example of display name if you want get localized description using extension methods. Use enumname_propertyname as resource convention
            //with _enumsManager.GetDisplayValue enum extension mehod you can get localized description; with _enumsManager.ToList<FakeEnum> you can convert enum into a KeyValue list with elements made by Key=numeric id, Value=localized description
            [Display(Name = "FakeEnum_FirstFake")]
            FirstFake = 100,
            [Display(Name = "FakeEnum_SecondFake")]
            SecondFake = 200
        }
    }
}
