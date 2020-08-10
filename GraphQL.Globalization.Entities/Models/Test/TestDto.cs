using GraphQL.Globalization.Entities.Extensions;
using GraphQL.Globalization.Entities.Models.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using static GraphQL.Globalization.Entities.Models.Enums;

namespace GraphQL.Globalization.Entities.Models.Test
{

    public class TestResponse
    {
        public string Code { get; set; }

    }

    //[AllOrAtLeastOnePropertyRequiredIf(AllOrAtLeastOneRequiredType.AtLeastOneRequired,   nameof(Test), new Object[] { 1 }, nameof(Fake), nameof(FakeOther), ErrorMessage = "required_atleastone")]
    //[AllOrAtLeastOnePropertyRequired(AllOrAtLeastOneRequiredType.AtLeastOneRequired, nameof(Fake), nameof(FakeOther), ErrorMessage = "required_atleastone")]
    public class TestRequest
    {
        [Required(ErrorMessage = "required_field")] //is used resources localization
        public string Fake { get; set; }

        [RequiredIf(nameof(Fake), "CheckOther", ErrorMessage = "required_field")] //in this case required is activated on FakeOther prop if Fake prop has value "CheckOther". This is a conditional required
        public string FakeOther { get; set; }

        public int Test { get; set; }

        [GreaterThan(nameof(Test), false, ErrorMessage = "greather_than")]
        public int TestOther { get; set; }

        [ValidEnum(ErrorMessage = "validenum_field")] //validate input enum
        public FakeEnum FakeEnum { get; set; }

    }
}
