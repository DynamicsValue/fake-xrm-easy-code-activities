using FakeXrmEasy.CodeActivities.Tests.CodeActivitiesForTesting;
using Microsoft.Xrm.Sdk.Workflow;
using Xunit;

namespace FakeXrmEasy.CodeActivities.Tests
{
    public class GetDefaultWorkflowContextTests : FakeXrmEasyTestsBase
    {
        [Fact]
        public void Should_populate_default_workflow_context_properties()
        {
            var wfContext = _context.GetDefaultWorkflowContext();
            var codeActivity = new ReturnCodeActivityContextActivity();
            var results = _context.ExecuteCodeActivity(wfContext, null, codeActivity);

            Assert.Equal(wfContext, codeActivity.WorkflowContext);
            Assert.Equal(wfContext.ServiceEndpointNotificationService, codeActivity.ServiceEndpointNotificationService);
        }
    }
}
