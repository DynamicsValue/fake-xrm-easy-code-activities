using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;

namespace FakeXrmEasy.CodeActivities.Tests.CodeActivitiesForTesting
{
    public class ReturnCodeActivityContextActivity : CodeActivity
    {
        public IWorkflowContext WorkflowContext { get; set; } 
        public IServiceEndpointNotificationService ServiceEndpointNotificationService { get; set; }

        /// <summary>
        /// Performs the addition of two summands
        /// </summary>
        protected override void Execute(CodeActivityContext executionContext)
        {
            WorkflowContext = executionContext.GetExtension<IWorkflowContext>();
            ServiceEndpointNotificationService = executionContext.GetExtension<IServiceEndpointNotificationService>();
        }
    }
}