﻿// Hapa Project, CC
// Created @2012 08 24 09:25
// Last Updated  by Huang, Jien @2012 08 24 09:25

#region

using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Script.Services;
using System.Web.Services;
using System.Xml.Linq;
using AutoX.Basic;

#endregion

namespace AutoX.Web
{
    /// <summary>
    ///   Summary description for Service
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [ScriptService]
    public class Service : WebService
    {
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Action(string input)
        {
            throw new NotImplementedException();
        }

        [WebMethod]
        public string Hello(string input)
        {
            return "You sent us :" + input;
        }

        /// <summary>
        ///   It only accept the XML format command
        /// </summary>
        /// <param name="xmlFormatCommand"> </param>
        /// <returns> </returns>
        [WebMethod]
        public string Command(string xmlFormatCommand)
        {
            Log.Debug(xmlFormatCommand);
            var xe = XElement.Parse(xmlFormatCommand);
            var name = xe.GetAttributeValue("Action");

            var wf = new ServiceFlow();
            //ActivityLib.MainWorkflow wf = new ActivityLib.MainWorkflow();
            IDictionary<string, object> input = new Dictionary<string, object>();
            input.Add("command", xmlFormatCommand);

            if (name != null)
                input.Add("name", name);

            var result = WorkflowInvoker.Invoke(wf, input, new TimeSpan(0, 10, 10));

            var outValue = (string) result["returnMessage"];
            Log.Debug(outValue);
            return outValue;
        }
    }
}