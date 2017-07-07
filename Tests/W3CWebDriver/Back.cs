﻿//******************************************************************************
//
// Copyright (c) 2016 Microsoft Corporation. All rights reserved.
//
// This code is licensed under the MIT License (MIT).
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//******************************************************************************

using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Threading;

namespace W3CWebDriver
{
    [TestClass]
    public class Back
    {
        private WindowsDriver<WindowsElement> session = null;

        [TestMethod]
        public void NavigateBack_Browser()
        {
            session = Utility.CreateNewSession(CommonTestSettings.EdgeAppId);
            Assert.IsNotNull(session);

            session.FindElementByAccessibilityId("addressEditBox").SendKeys(CommonTestSettings.EdgeAboutFlagsURL + Keys.Enter);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            var originalTitle = session.Title;
            Assert.AreNotEqual(string.Empty, originalTitle);

            // Navigate to different URLs
            var addressEditBox = session.FindElementByAccessibilityId("addressEditBox");
            addressEditBox.SendKeys(CommonTestSettings.EdgeAboutBlankURL + Keys.Enter);
            Assert.AreNotEqual(originalTitle, session.Title);

            // Navigate back to original URL
            session.Navigate().Back();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Assert.AreEqual(originalTitle, session.Title);

            session.Quit();
            session = null;
        }

        [TestMethod]
        public void NavigateBack_ModernApp()
        {
            session = Utility.CreateNewSession(CommonTestSettings.AlarmClockAppId);
            Assert.IsNotNull(session);

            // Navigate to New Alarm view
            session.FindElementByAccessibilityId("AlarmPivotItem").Click();
            session.FindElementByAccessibilityId("AddAlarmButton").Click();
            Assert.IsNotNull(session.FindElementByAccessibilityId("EditAlarmHeader"));

            // Navigate back to the original view
            session.Navigate().Back();
            Assert.IsNotNull(session.FindElementByAccessibilityId("AlarmPivotItem"));

            session.Quit();
            session = null;
        }

        [TestMethod]
        public void NavigateBack_SystemApp()
        {
            session = Utility.CreateNewSession(CommonTestSettings.ExplorerAppId);
            Assert.IsNotNull(session);

            Thread.Sleep(TimeSpan.FromSeconds(1));
            var originalTitle = session.Title;
            Assert.AreNotEqual(string.Empty, originalTitle);

            // Navigate Windows Explorer to change folder
            session.Keyboard.SendKeys(Keys.Alt + "d" + Keys.Alt + CommonTestSettings.TestFolderLocation + Keys.Enter);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Assert.AreNotEqual(originalTitle, session.Title);

            // Navigate back to the original folder
            session.Navigate().Back();
            Assert.AreEqual(originalTitle, session.Title);

            session.Quit();
            session = null;
        }

        [TestMethod]
        public void NavigateBackError_NoSuchWindow()
        {
            try
            {
                Utility.GetOrphanedSession().Navigate().Back();
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }
    }
}
