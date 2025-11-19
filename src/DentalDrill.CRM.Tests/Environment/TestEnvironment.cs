using System;
using System.Collections.Generic;
using System.Text;
using DevGuild.AspNetCore.Testing;

namespace DentalDrill.CRM.Tests.Environment
{
    public class TestEnvironment : MockEnvironmentBase
    {
        public static IMockEnvironment Create()
        {
            return MockEnvironment.Create<TestEnvironment>(new TestEnvironmentConfiguration());
        }
    }
}
