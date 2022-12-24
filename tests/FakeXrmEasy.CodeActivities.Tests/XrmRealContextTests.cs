using Crm;
using FakeXrmEasy.CodeActivities.Tests.CodeActivitiesForTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FakeXrmEasy.CodeActivities.Tests
{
    public class XrmRealContextTests: FakeXrmEasyTestsBase
    {
        private readonly XrmRealContext _realContext;
        private readonly Account _account;

        public XrmRealContextTests() : base()
        {
            _realContext = new XrmRealContext(_service);

            _account = new Account() { Id = Guid.NewGuid() };
        }

        [Fact]
        public void Should_execute_code_activity_against_a_dummy_real_context()
        {
            var inputs = new Dictionary<string, object>()
            {
                { "inputEntity" , _account.ToEntityReference()}
            };

            var wfContext = _realContext.GetDefaultWorkflowContext();
            var outputs = _realContext.ExecuteCodeActivity<CreateTaskActivity>(wfContext, inputs);

            var tasks = _context.CreateQuery<Task>().ToList();
            Assert.Single(tasks);

            Assert.Equal(_account.Id, tasks[0].RegardingObjectId.Id);
        }
    }
}
