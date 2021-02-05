// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Microsoft.DotNet.Docker.Tests
{
    public class DotNetTheoryDiscoverer : TheoryDiscoverer
    {
        public DotNetTheoryDiscoverer(IMessageSink diagnosticMessageSink) : base(diagnosticMessageSink)
        {
        }

        public override IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo theoryAttribute)
        {
            IEnumerable<IXunitTestCase> results = base.Discover(discoveryOptions, testMethod, theoryAttribute);

            if (results.Count() == 1 &&
                results.First() is ExecutionErrorTestCase errorTestCase &&
                errorTestCase.ErrorMessage.StartsWith("No data found for"))
            {
                return CreateTestCasesForSkippedDataRow(discoveryOptions, testMethod, theoryAttribute, Array.Empty<object>(), errorTestCase.ErrorMessage);
            }

            return results;
        }
    }
}
