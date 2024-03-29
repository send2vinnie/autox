﻿using System;
using System.Xml.Linq;
using AutoX.Basic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutoX.Client.Core
{
    internal class VerifyValue : AbstractAction
    {
        public override XElement Act()
        {
            var sr = new StepResult(this);
            if (UIObject == null || UIObject.Count == 0)
            {
                sr.Error("Expected UI Object is not found!");
            }
            else
            {
                var target = UIObject[0];
                //Data should look like text=>value
                if (!string.IsNullOrEmpty(Data))
                {
                    sr.Error("Please define variable and attribute.");
                    return sr.GetResult();
                }
                if (Data.Contains("=>"))
                {
                    var pos = Data.IndexOf("=>", StringComparison.Ordinal);
                    try
                    {
                        var attr = Data.Substring(0, pos);
                        var expected = Data.Substring(pos + 2);
                        var actual = target.GetAttribute(attr);
                        if(!expected.Trim().Equals(actual.Trim()))
                            sr.Error("Expected["+expected+"]<>Actual["+actual+"]");
                    }
                    catch (Exception ex)
                    {
                        sr.Error(ExceptionHelper.FormatStackTrace("Get attribute Value Failed:", ex));
                    }
                }
                else
                {
                    VerifyWebElement(Data, target);
                }
                

            }
            return sr.GetResult();
        }

        public static bool VerifyWebElement(String value, IWebElement webElement)
        {
            String valueOnScreen;

            // if the object is a drop-down list we need to use getText()
            var objectTagName = webElement.TagName;

            switch (objectTagName)
            {
                case "select":
                    var select = new SelectElement(webElement);
                    return select.SelectedOption.Text.Trim().Equals(value);
                // if the object is a checkbox
                case "checkbox":
                    valueOnScreen = webElement.Selected ? "true" : "false";
                    return valueOnScreen.Equals(value.ToLower().Trim());
                   
                // otherwise we can get the value attribute
                default:
                    valueOnScreen = webElement.GetAttribute("value");
                    return valueOnScreen.Equals(value.ToLower().Trim());
                    
            }
           
        }
    }
}
