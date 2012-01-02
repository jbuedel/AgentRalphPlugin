namespace Ralph.Test.Project
{
    enum MyEnum { First, Second, Third, Fourth }

    class NotEnum { }

    class MakeEnumComparisonTypeSafeTestClass
    {
        // Simple case.
        public void Match1(MyEnum e, NotEnum ne)
        {
            if (e.ToString() == "First") { }
        }

        // ToLower, ToUpper, and Trim (with no params) are all ok on the left hand side.
        public void Match2(MyEnum e)
        {
            if (e.ToString().ToLower() == "first") { }
        }

        // The enum value string need not case match.
		// Technically, this ought to be a "statement is always false" as lowercase first
		// will never match any MyEnum values.
        public void Match3(MyEnum e)
        {
            if (e.ToString() == "first") { }
        }

        // The call is already correct
        public void NotMatch1(MyEnum e)
        {
            if (e == MyEnum.First) { }
        }

        // The string is not one of the known enum values.
        public void NotMatch2(MyEnum e)
        {
            if(e.ToString() == "StringThatDoesn'tMatchTheEnumValue") { }
        }
    }
}
