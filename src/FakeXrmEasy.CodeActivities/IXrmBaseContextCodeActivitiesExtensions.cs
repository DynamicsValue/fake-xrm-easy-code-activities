﻿using FakeItEasy;
using FakeXrmEasy.Abstractions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;

namespace FakeXrmEasy.CodeActivities
{
    /// <summary>
    /// Adds extensions to execute code activities against a IXrmBaseContext
    /// </summary>
    public static class IXrmBaseContextCodeActivitiesExtensions
    {
        /// <summary>
        /// Gets a default workflow context with default values already populated
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static XrmFakedWorkflowContext GetDefaultWorkflowContext(this IXrmBaseContext context)
        {
            var userId = context.CallerProperties?.CallerId?.Id ?? Guid.NewGuid();
            Guid businessUnitId = context.CallerProperties?.BusinessUnitId?.Id ?? Guid.NewGuid();

            return new XrmFakedWorkflowContext
            {
                Depth = 1,
                IsExecutingOffline = false,
                MessageName = "Create",
                UserId = userId,
                BusinessUnitId = businessUnitId,
                InitiatingUserId = userId,
                InputParameters = new ParameterCollection(),
                OutputParameters = new ParameterCollection(),
                SharedVariables = new ParameterCollection(),
                PreEntityImages = new EntityImageCollection(),
                PostEntityImages = new EntityImageCollection()
            };
        }

        /// <summary>
        /// Executes a codeactivity with a default workflow context
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="inputs"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static IDictionary<string, object> ExecuteCodeActivity<T>(this IXrmBaseContext context, Dictionary<string, object> inputs, T instance = null)
            where T : CodeActivity, new()
        {
            var wfContext = context.GetDefaultWorkflowContext();
            return context.ExecuteCodeActivity(wfContext, inputs, instance);
        }

        /// <summary>
        /// Executes a codeactivity with a custom workflow default context
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static IDictionary<string, object> ExecuteCodeActivity<T>(this IXrmBaseContext context, XrmFakedWorkflowContext wfContext, Dictionary<string, object> inputs = null, T instance = null)
            where T : CodeActivity, new()
        {
            var debugText = "";
            try
            {
                debugText = "Creating instance..." + Environment.NewLine;
                if (instance == null)
                {
                    instance = new T();
                }
                var invoker = new WorkflowInvoker(instance);
                debugText += "Invoker created" + Environment.NewLine;
                debugText += "Adding extensions..." + Environment.NewLine;
                invoker.Extensions.Add<ITracingService>(() => context.GetTracingService());
                invoker.Extensions.Add<IWorkflowContext>(() => wfContext);
                invoker.Extensions.Add(() =>
                {
                    var fakedServiceFactory = A.Fake<IOrganizationServiceFactory>();
                    A.CallTo(() => fakedServiceFactory.CreateOrganizationService(A<Guid?>._)).ReturnsLazily((Guid? g) => context.GetOrganizationService());
                    return fakedServiceFactory;
                });
                invoker.Extensions.Add(() => wfContext.ServiceEndpointNotificationService);

                debugText += "Adding extensions...ok." + Environment.NewLine;
                debugText += "Invoking activity..." + Environment.NewLine;

                if (inputs == null)
                {
                    inputs = new Dictionary<string, object>();
                }

                return invoker.Invoke(inputs);
            }
            catch (TypeLoadException exception)
            {
                var typeName = exception.TypeName != null ? exception.TypeName : "(null)";
                throw new TypeLoadException($"When loading type: {typeName}.{exception.Message}in domain directory: {AppDomain.CurrentDomain.BaseDirectory}\nDebug={debugText}");
            }
        }
    }
}

