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
    public class Forward
    {
        private WindowsDriver<WindowsElement> session = null;

        [TestMethod]
        public void NavigateForward_Browser()
        {
            session = Utility.CreateNewSession(CommonTestSettings.EdgeAppId);
            Assert.IsNotNull(session);

            session.FindElementByAccessibilityId("addressEditBox").SendKeys(CommonTestSettings.EdgeAboutFlagsURL + Keys.Enter);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            var originalTitle = session.Title;
            Assert.AreNotEqual(string.Empty, originalTitle);

            // Navigate to different URLs
            session.FindElementByAccessibilityId("addressEditBox").SendKeys(CommonTestSettings.EdgeAboutTabsURL + Keys.Enter);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            var newTitle = session.Title;
            Assert.AreNotEqual(originalTitle, newTitle);

            // Navigate back to original URL
            session.Navigate().Back();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Assert.AreEqual(originalTitle, session.Title);

            // Navigate forward to original URL
            session.Navigate().Forward();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Assert.AreEqual(newTitle, session.Title);

            session.Quit();
        }

        [TestMethod]
        public void NavigateForward_SystemApp()
        {
            session = Utility.CreateNewSession(CommonTestSettings.ExplorerAppId);
            Assert.IsNotNull(session);

            var originalTitle = session.Title;
            Assert.AreNotEqual(string.Empty, originalTitle);

            // Navigate Windows Explorer to change folder
            session.Keyboard.SendKeys(Keys.Alt + "d" + Keys.Alt + CommonTestSettings.TestFolderLocation + Keys.Enter);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            var newTitle = session.Title;
            Assert.AreNotEqual(originalTitle, newTitle);

            // Navigate back to the original folder
            session.Navigate().Back();
            Assert.AreEqual(originalTitle, session.Title);

            // Navigate forward to the target folder
            session.Navigate().Forward();
            Assert.AreEqual(newTitle, session.Title);

            session.Quit();
        }

        [TestMethod]
        public void NavigateForwardError_NoSuchWindow()
        {
            try
            {
                Utility.GetOrphanedSession().Navigate().Forward();
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }
    }
}
