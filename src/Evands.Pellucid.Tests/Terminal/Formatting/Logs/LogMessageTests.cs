using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Evands.Pellucid.Terminal.Formatting.Logs
{
    [TestClass]
    public class LogMessageTests
    {
        [TestInitialize]
        public void Initialize()
        {
        }

        [TestCleanup]
        public void Cleanup()
        {
        }

        [TestMethod]
        public void TryParse_With_Exception_Returns_False()
        {
            LogMessage msg = null;
            var msgText = GetMessage(1);
            var dt = DateTime.Now;
            dt = dt.AddSeconds(-dt.Second);
            dt = dt.AddMilliseconds(-dt.Millisecond);
            var origin = "SomeRandomApp.exe";

            var result = LogMessage.TryParse("1234567890987654321. " + GetFullMessage("Warning", origin, dt, msgText.Replace("\r\n", "\n")), 110, out msg);

            Assert.IsFalse(result);
        }

        #region Warnings

        [TestMethod]
        public void TryParse_With_Warning_No_LineBreaks_No_Number_Gets_Message()
        {
            LogMessage msg = null;
            var msgText = GetMessage(0);
            var dt = DateTime.Now;
            dt = dt.AddSeconds(-dt.Second);
            dt = dt.AddMilliseconds(-dt.Millisecond);
            var origin = "SomeRandomApp.exe";
            var result = LogMessage.TryParse(GetFullMessage("Warning", origin, dt, msgText), 110, out msg);

            Assert.IsTrue(result);
            Assert.IsNotNull(msg);
            Assert.AreEqual(msgText, msg.Message);
            Assert.AreEqual(dt.ToLongDateString(), msg.TimeStamp.ToLongDateString());
            Assert.AreEqual(dt.ToLongTimeString(), msg.TimeStamp.ToLongTimeString());
            Assert.AreEqual(110, msg.Number);
            Assert.AreEqual("Warning", msg.MessageType);
            Assert.AreEqual(origin, msg.Origination);
        }

        [TestMethod]
        public void TryParse_With_Warning_LineBreaks_No_Number_Gets_Message()
        {
            LogMessage msg = null;
            var msgText = GetMessage(1);
            var dt = DateTime.Now;
            dt = dt.AddSeconds(-dt.Second);
            dt = dt.AddMilliseconds(-dt.Millisecond);
            var origin = "SomeRandomApp.exe";
            var result = LogMessage.TryParse(GetFullMessage("Warning", origin, dt, msgText), 110, out msg);

            Assert.IsTrue(result);
            Assert.IsNotNull(msg);
            Assert.AreEqual(msgText, msg.Message);
            Assert.AreEqual(dt.ToLongDateString(), msg.TimeStamp.ToLongDateString());
            Assert.AreEqual(dt.ToLongTimeString(), msg.TimeStamp.ToLongTimeString());
            Assert.AreEqual(110, msg.Number);
            Assert.AreEqual("Warning", msg.MessageType);
            Assert.AreEqual(origin, msg.Origination);
        }

        [TestMethod]
        public void TryParse_With_Warning_LongMessage_With_LineBreaks_No_Number_Gets_Message()
        {
            LogMessage msg = null;
            var msgText = GetMessage(2);
            var dt = DateTime.Now;
            dt = dt.AddSeconds(-dt.Second);
            dt = dt.AddMilliseconds(-dt.Millisecond);
            var origin = "SomeRandomApp.exe";
            var result = LogMessage.TryParse(GetFullMessage("Warning", origin, dt, msgText), 110, out msg);

            Assert.IsTrue(result);
            Assert.IsNotNull(msg);
            Assert.AreEqual(msgText, msg.Message);
            Assert.AreEqual(dt.ToLongDateString(), msg.TimeStamp.ToLongDateString());
            Assert.AreEqual(dt.ToLongTimeString(), msg.TimeStamp.ToLongTimeString());
            Assert.AreEqual(110, msg.Number);
            Assert.AreEqual("Warning", msg.MessageType);
            Assert.AreEqual(origin, msg.Origination);
        }

        [TestMethod]
        public void TryParse_With_Warning_No_LineBreaks_With_Number_Gets_Message()
        {
            LogMessage msg = null;
            var msgText = GetMessage(0);
            var dt = DateTime.Now;
            dt = dt.AddSeconds(-dt.Second);
            dt = dt.AddMilliseconds(-dt.Millisecond);
            var origin = "SomeRandomApp.exe";
            var result = LogMessage.TryParse("111. " + GetFullMessage("Warning", origin, dt, msgText), 110, out msg);

            Assert.IsTrue(result);
            Assert.IsNotNull(msg);
            Assert.AreEqual(msgText, msg.Message);
            Assert.AreEqual(dt.ToLongDateString(), msg.TimeStamp.ToLongDateString());
            Assert.AreEqual(dt.ToLongTimeString(), msg.TimeStamp.ToLongTimeString());
            Assert.AreEqual(111, msg.Number);
            Assert.AreEqual("Warning", msg.MessageType);
            Assert.AreEqual(origin, msg.Origination);
        }

        [TestMethod]
        public void TryParse_With_Warning_LineBreaks_With_Number_Gets_Message()
        {
            LogMessage msg = null;
            var msgText = GetMessage(1);
            var dt = DateTime.Now;
            dt = dt.AddSeconds(-dt.Second);
            dt = dt.AddMilliseconds(-dt.Millisecond);
            var origin = "SomeRandomApp.exe";
            var result = LogMessage.TryParse("111. " + GetFullMessage("Warning", origin, dt, msgText), 110, out msg);

            Assert.IsTrue(result);
            Assert.IsNotNull(msg);
            Assert.AreEqual(msgText, msg.Message);
            Assert.AreEqual(dt.ToLongDateString(), msg.TimeStamp.ToLongDateString());
            Assert.AreEqual(dt.ToLongTimeString(), msg.TimeStamp.ToLongTimeString());
            Assert.AreEqual(111, msg.Number);
            Assert.AreEqual("Warning", msg.MessageType);
            Assert.AreEqual(origin, msg.Origination);
        }

        [TestMethod]
        public void TryParse_With_Warning_LongMessage_With_LineBreaks_With_Number_Gets_Message()
        {
            LogMessage msg = null;
            var msgText = GetMessage(2);
            var dt = DateTime.Now;
            dt = dt.AddSeconds(-dt.Second);
            dt = dt.AddMilliseconds(-dt.Millisecond);
            var origin = "SomeRandomApp.exe";
            var result = LogMessage.TryParse("111. " + GetFullMessage("Warning", origin, dt, msgText), 110, out msg);

            Assert.IsTrue(result);
            Assert.IsNotNull(msg);
            Assert.AreEqual(msgText, msg.Message);
            Assert.AreEqual(dt.ToLongDateString(), msg.TimeStamp.ToLongDateString());
            Assert.AreEqual(dt.ToLongTimeString(), msg.TimeStamp.ToLongTimeString());
            Assert.AreEqual(111, msg.Number);
            Assert.AreEqual("Warning", msg.MessageType);
            Assert.AreEqual(origin, msg.Origination);
        }

        #endregion

        #region Notices

        [TestMethod]
        public void TryParse_With_Notice_No_LineBreaks_No_Number_Gets_Message()
        {
            LogMessage msg = null;
            var msgText = GetMessage(0);
            var dt = DateTime.Now;
            dt = dt.AddSeconds(-dt.Second);
            dt = dt.AddMilliseconds(-dt.Millisecond);
            var origin = "SomeRandomApp.exe";
            var result = LogMessage.TryParse(GetFullMessage("Notice", origin, dt, msgText), 110, out msg);

            Assert.IsTrue(result);
            Assert.IsNotNull(msg);
            Assert.AreEqual(msgText, msg.Message);
            Assert.AreEqual(dt.ToLongDateString(), msg.TimeStamp.ToLongDateString());
            Assert.AreEqual(dt.ToLongTimeString(), msg.TimeStamp.ToLongTimeString());
            Assert.AreEqual(110, msg.Number);
            Assert.AreEqual("Notice", msg.MessageType);
            Assert.AreEqual(origin, msg.Origination);
        }

        [TestMethod]
        public void TryParse_With_Notice_LineBreaks_No_Number_Gets_Message()
        {
            LogMessage msg = null;
            var msgText = GetMessage(1);
            var dt = DateTime.Now;
            dt = dt.AddSeconds(-dt.Second);
            dt = dt.AddMilliseconds(-dt.Millisecond);
            var origin = "SomeRandomApp.exe";
            var result = LogMessage.TryParse(GetFullMessage("Notice", origin, dt, msgText), 110, out msg);

            Assert.IsTrue(result);
            Assert.IsNotNull(msg);
            Assert.AreEqual(msgText, msg.Message);
            Assert.AreEqual(dt.ToLongDateString(), msg.TimeStamp.ToLongDateString());
            Assert.AreEqual(dt.ToLongTimeString(), msg.TimeStamp.ToLongTimeString());
            Assert.AreEqual(110, msg.Number);
            Assert.AreEqual("Notice", msg.MessageType);
            Assert.AreEqual(origin, msg.Origination);
        }

        [TestMethod]
        public void TryParse_With_Notice_LongMessage_With_LineBreaks_No_Number_Gets_Message()
        {
            LogMessage msg = null;
            var msgText = GetMessage(2);
            var dt = DateTime.Now;
            dt = dt.AddSeconds(-dt.Second);
            dt = dt.AddMilliseconds(-dt.Millisecond);
            var origin = "SomeRandomApp.exe";
            var result = LogMessage.TryParse(GetFullMessage("Notice", origin, dt, msgText), 110, out msg);

            Assert.IsTrue(result);
            Assert.IsNotNull(msg);
            Assert.AreEqual(msgText, msg.Message);
            Assert.AreEqual(dt.ToLongDateString(), msg.TimeStamp.ToLongDateString());
            Assert.AreEqual(dt.ToLongTimeString(), msg.TimeStamp.ToLongTimeString());
            Assert.AreEqual(110, msg.Number);
            Assert.AreEqual("Notice", msg.MessageType);
            Assert.AreEqual(origin, msg.Origination);
        }

        [TestMethod]
        public void TryParse_With_Notice_No_LineBreaks_With_Number_Gets_Message()
        {
            LogMessage msg = null;
            var msgText = GetMessage(0);
            var dt = DateTime.Now;
            dt = dt.AddSeconds(-dt.Second);
            dt = dt.AddMilliseconds(-dt.Millisecond);
            var origin = "SomeRandomApp.exe";
            var result = LogMessage.TryParse("111. " + GetFullMessage("Notice", origin, dt, msgText), 110, out msg);

            Assert.IsTrue(result);
            Assert.IsNotNull(msg);
            Assert.AreEqual(msgText, msg.Message);
            Assert.AreEqual(dt.ToLongDateString(), msg.TimeStamp.ToLongDateString());
            Assert.AreEqual(dt.ToLongTimeString(), msg.TimeStamp.ToLongTimeString());
            Assert.AreEqual(111, msg.Number);
            Assert.AreEqual("Notice", msg.MessageType);
            Assert.AreEqual(origin, msg.Origination);
        }

        [TestMethod]
        public void TryParse_With_Notice_LineBreaks_With_Number_Gets_Message()
        {
            LogMessage msg = null;
            var msgText = GetMessage(1);
            var dt = DateTime.Now;
            dt = dt.AddSeconds(-dt.Second);
            dt = dt.AddMilliseconds(-dt.Millisecond);
            var origin = "SomeRandomApp.exe";
            var result = LogMessage.TryParse("111. " + GetFullMessage("Notice", origin, dt, msgText), 110, out msg);

            Assert.IsTrue(result);
            Assert.IsNotNull(msg);
            Assert.AreEqual(msgText, msg.Message);
            Assert.AreEqual(dt.ToLongDateString(), msg.TimeStamp.ToLongDateString());
            Assert.AreEqual(dt.ToLongTimeString(), msg.TimeStamp.ToLongTimeString());
            Assert.AreEqual(111, msg.Number);
            Assert.AreEqual("Notice", msg.MessageType);
            Assert.AreEqual(origin, msg.Origination);
        }

        [TestMethod]
        public void TryParse_With_Notice_LongMessage_With_LineBreaks_With_Number_Gets_Message()
        {
            LogMessage msg = null;
            var msgText = GetMessage(2);
            var dt = DateTime.Now;
            dt = dt.AddSeconds(-dt.Second);
            dt = dt.AddMilliseconds(-dt.Millisecond);
            var origin = "SomeRandomApp.exe";
            var result = LogMessage.TryParse("111. " + GetFullMessage("Notice", origin, dt, msgText), 110, out msg);

            Assert.IsTrue(result);
            Assert.IsNotNull(msg);
            Assert.AreEqual(msgText, msg.Message);
            Assert.AreEqual(dt.ToLongDateString(), msg.TimeStamp.ToLongDateString());
            Assert.AreEqual(dt.ToLongTimeString(), msg.TimeStamp.ToLongTimeString());
            Assert.AreEqual(111, msg.Number);
            Assert.AreEqual("Notice", msg.MessageType);
            Assert.AreEqual(origin, msg.Origination);
        }

        #endregion

        #region Errors

        [TestMethod]
        public void TryParse_With_Error_No_LineBreaks_No_Number_Gets_Message()
        {
            LogMessage msg = null;
            var msgText = GetMessage(0);
            var dt = DateTime.Now;
            dt = dt.AddSeconds(-dt.Second);
            dt = dt.AddMilliseconds(-dt.Millisecond);
            var origin = "SomeRandomApp.exe";
            var result = LogMessage.TryParse(GetFullMessage("Error", origin, dt, msgText), 110, out msg);

            Assert.IsTrue(result);
            Assert.IsNotNull(msg);
            Assert.AreEqual(msgText, msg.Message);
            Assert.AreEqual(dt.ToLongDateString(), msg.TimeStamp.ToLongDateString());
            Assert.AreEqual(dt.ToLongTimeString(), msg.TimeStamp.ToLongTimeString());
            Assert.AreEqual(110, msg.Number);
            Assert.AreEqual("Error", msg.MessageType);
            Assert.AreEqual(origin, msg.Origination);
        }

        [TestMethod]
        public void TryParse_With_Error_LineBreaks_No_Number_Gets_Message()
        {
            LogMessage msg = null;
            var msgText = GetMessage(1);
            var dt = DateTime.Now;
            dt = dt.AddSeconds(-dt.Second);
            dt = dt.AddMilliseconds(-dt.Millisecond);
            var origin = "SomeRandomApp.exe";
            var result = LogMessage.TryParse(GetFullMessage("Error", origin, dt, msgText), 110, out msg);

            Assert.IsTrue(result);
            Assert.IsNotNull(msg);
            Assert.AreEqual(msgText, msg.Message);
            Assert.AreEqual(dt.ToLongDateString(), msg.TimeStamp.ToLongDateString());
            Assert.AreEqual(dt.ToLongTimeString(), msg.TimeStamp.ToLongTimeString());
            Assert.AreEqual(110, msg.Number);
            Assert.AreEqual("Error", msg.MessageType);
            Assert.AreEqual(origin, msg.Origination);
        }

        [TestMethod]
        public void TryParse_With_Error_LongMessage_With_LineBreaks_No_Number_Gets_Message()
        {
            LogMessage msg = null;
            var msgText = GetMessage(2);
            var dt = DateTime.Now;
            dt = dt.AddSeconds(-dt.Second);
            dt = dt.AddMilliseconds(-dt.Millisecond);
            var origin = "SomeRandomApp.exe";
            var result = LogMessage.TryParse(GetFullMessage("Error", origin, dt, msgText), 110, out msg);

            Assert.IsTrue(result);
            Assert.IsNotNull(msg);
            Assert.AreEqual(msgText, msg.Message);
            Assert.AreEqual(dt.ToLongDateString(), msg.TimeStamp.ToLongDateString());
            Assert.AreEqual(dt.ToLongTimeString(), msg.TimeStamp.ToLongTimeString());
            Assert.AreEqual(110, msg.Number);
            Assert.AreEqual("Error", msg.MessageType);
            Assert.AreEqual(origin, msg.Origination);
        }

        [TestMethod]
        public void TryParse_With_Error_No_LineBreaks_With_Number_Gets_Message()
        {
            LogMessage msg = null;
            var msgText = GetMessage(0);
            var dt = DateTime.Now;
            dt = dt.AddSeconds(-dt.Second);
            dt = dt.AddMilliseconds(-dt.Millisecond);
            var origin = "SomeRandomApp.exe";
            var result = LogMessage.TryParse("111. " + GetFullMessage("Error", origin, dt, msgText), 110, out msg);

            Assert.IsTrue(result);
            Assert.IsNotNull(msg);
            Assert.AreEqual(msgText, msg.Message);
            Assert.AreEqual(dt.ToLongDateString(), msg.TimeStamp.ToLongDateString());
            Assert.AreEqual(dt.ToLongTimeString(), msg.TimeStamp.ToLongTimeString());
            Assert.AreEqual(111, msg.Number);
            Assert.AreEqual("Error", msg.MessageType);
            Assert.AreEqual(origin, msg.Origination);
        }

        [TestMethod]
        public void TryParse_With_Error_LineBreaks_With_Number_Gets_Message()
        {
            LogMessage msg = null;
            var msgText = GetMessage(1);
            var dt = DateTime.Now;
            dt = dt.AddSeconds(-dt.Second);
            dt = dt.AddMilliseconds(-dt.Millisecond);
            var origin = "SomeRandomApp.exe";
            var result = LogMessage.TryParse("111. " + GetFullMessage("Error", origin, dt, msgText), 110, out msg);

            Assert.IsTrue(result);
            Assert.IsNotNull(msg);
            Assert.AreEqual(msgText, msg.Message);
            Assert.AreEqual(dt.ToLongDateString(), msg.TimeStamp.ToLongDateString());
            Assert.AreEqual(dt.ToLongTimeString(), msg.TimeStamp.ToLongTimeString());
            Assert.AreEqual(111, msg.Number);
            Assert.AreEqual("Error", msg.MessageType);
            Assert.AreEqual(origin, msg.Origination);
        }

        [TestMethod]
        public void TryParse_With_Error_LongMessage_With_LineBreaks_With_Number_Gets_Message()
        {
            LogMessage msg = null;
            var msgText = GetMessage(2);
            var dt = DateTime.Now;
            dt = dt.AddSeconds(-dt.Second);
            dt = dt.AddMilliseconds(-dt.Millisecond);
            var origin = "SomeRandomApp.exe";
            var result = LogMessage.TryParse("111. " + GetFullMessage("Error", origin, dt, msgText), 110, out msg);

            Assert.IsTrue(result);
            Assert.IsNotNull(msg);
            Assert.AreEqual(msgText, msg.Message);
            Assert.AreEqual(dt.ToLongDateString(), msg.TimeStamp.ToLongDateString());
            Assert.AreEqual(dt.ToLongTimeString(), msg.TimeStamp.ToLongTimeString());
            Assert.AreEqual(111, msg.Number);
            Assert.AreEqual("Error", msg.MessageType);
            Assert.AreEqual(origin, msg.Origination);
        }

        #endregion

        #region Info

        [TestMethod]
        public void TryParse_With_Info_No_LineBreaks_No_Number_Gets_Message()
        {
            LogMessage msg = null;
            var msgText = GetMessage(0);
            var dt = DateTime.Now;
            dt = dt.AddSeconds(-dt.Second);
            dt = dt.AddMilliseconds(-dt.Millisecond);
            var origin = "SomeRandomApp.exe";
            var result = LogMessage.TryParse(GetFullMessage("Info", origin, dt, msgText), 110, out msg);

            Assert.IsTrue(result);
            Assert.IsNotNull(msg);
            Assert.AreEqual(msgText, msg.Message);
            Assert.AreEqual(dt.ToLongDateString(), msg.TimeStamp.ToLongDateString());
            Assert.AreEqual(dt.ToLongTimeString(), msg.TimeStamp.ToLongTimeString());
            Assert.AreEqual(110, msg.Number);
            Assert.AreEqual("Info", msg.MessageType);
            Assert.AreEqual(origin, msg.Origination);
        }

        [TestMethod]
        public void TryParse_With_Info_LineBreaks_No_Number_Gets_Message()
        {
            LogMessage msg = null;
            var msgText = GetMessage(1);
            var dt = DateTime.Now;
            dt = dt.AddSeconds(-dt.Second);
            dt = dt.AddMilliseconds(-dt.Millisecond);
            var origin = "SomeRandomApp.exe";
            var result = LogMessage.TryParse(GetFullMessage("Info", origin, dt, msgText), 110, out msg);

            Assert.IsTrue(result);
            Assert.IsNotNull(msg);
            Assert.AreEqual(msgText, msg.Message);
            Assert.AreEqual(dt.ToLongDateString(), msg.TimeStamp.ToLongDateString());
            Assert.AreEqual(dt.ToLongTimeString(), msg.TimeStamp.ToLongTimeString());
            Assert.AreEqual(110, msg.Number);
            Assert.AreEqual("Info", msg.MessageType);
            Assert.AreEqual(origin, msg.Origination);
        }

        [TestMethod]
        public void TryParse_With_Info_LongMessage_With_LineBreaks_No_Number_Gets_Message()
        {
            LogMessage msg = null;
            var msgText = GetMessage(2);
            var dt = DateTime.Now;
            dt = dt.AddSeconds(-dt.Second);
            dt = dt.AddMilliseconds(-dt.Millisecond);
            var origin = "SomeRandomApp.exe";
            var result = LogMessage.TryParse(GetFullMessage("Info", origin, dt, msgText), 110, out msg);

            Assert.IsTrue(result);
            Assert.IsNotNull(msg);
            Assert.AreEqual(msgText, msg.Message);
            Assert.AreEqual(dt.ToLongDateString(), msg.TimeStamp.ToLongDateString());
            Assert.AreEqual(dt.ToLongTimeString(), msg.TimeStamp.ToLongTimeString());
            Assert.AreEqual(110, msg.Number);
            Assert.AreEqual("Info", msg.MessageType);
            Assert.AreEqual(origin, msg.Origination);
        }

        [TestMethod]
        public void TryParse_With_Info_No_LineBreaks_With_Number_Gets_Message()
        {
            LogMessage msg = null;
            var msgText = GetMessage(0);
            var dt = DateTime.Now;
            dt = dt.AddSeconds(-dt.Second);
            dt = dt.AddMilliseconds(-dt.Millisecond);
            var origin = "SomeRandomApp.exe";
            var result = LogMessage.TryParse("111. " + GetFullMessage("Info", origin, dt, msgText), 110, out msg);

            Assert.IsTrue(result);
            Assert.IsNotNull(msg);
            Assert.AreEqual(msgText, msg.Message);
            Assert.AreEqual(dt.ToLongDateString(), msg.TimeStamp.ToLongDateString());
            Assert.AreEqual(dt.ToLongTimeString(), msg.TimeStamp.ToLongTimeString());
            Assert.AreEqual(111, msg.Number);
            Assert.AreEqual("Info", msg.MessageType);
            Assert.AreEqual(origin, msg.Origination);
        }

        [TestMethod]
        public void TryParse_With_Info_LineBreaks_With_Number_Gets_Message()
        {
            LogMessage msg = null;
            var msgText = GetMessage(1);
            var dt = DateTime.Now;
            dt = dt.AddSeconds(-dt.Second);
            dt = dt.AddMilliseconds(-dt.Millisecond);
            var origin = "SomeRandomApp.exe";
            var result = LogMessage.TryParse("111. " + GetFullMessage("Info", origin, dt, msgText), 110, out msg);

            Assert.IsTrue(result);
            Assert.IsNotNull(msg);
            Assert.AreEqual(msgText, msg.Message);
            Assert.AreEqual(dt.ToLongDateString(), msg.TimeStamp.ToLongDateString());
            Assert.AreEqual(dt.ToLongTimeString(), msg.TimeStamp.ToLongTimeString());
            Assert.AreEqual(111, msg.Number);
            Assert.AreEqual("Info", msg.MessageType);
            Assert.AreEqual(origin, msg.Origination);
        }

        [TestMethod]
        public void TryParse_With_Info_LongMessage_With_LineBreaks_With_Number_Gets_Message()
        {
            LogMessage msg = null;
            var msgText = GetMessage(2);
            var dt = DateTime.Now;
            dt = dt.AddSeconds(-dt.Second);
            dt = dt.AddMilliseconds(-dt.Millisecond);
            var origin = "SomeRandomApp.exe";
            var result = LogMessage.TryParse("111. " + GetFullMessage("Info", origin, dt, msgText), 110, out msg);

            Assert.IsTrue(result);
            Assert.IsNotNull(msg);
            Assert.AreEqual(msgText, msg.Message);
            Assert.AreEqual(dt.ToLongDateString(), msg.TimeStamp.ToLongDateString());
            Assert.AreEqual(dt.ToLongTimeString(), msg.TimeStamp.ToLongTimeString());
            Assert.AreEqual(111, msg.Number);
            Assert.AreEqual("Info", msg.MessageType);
            Assert.AreEqual(origin, msg.Origination);
        }

        #endregion

        #region OK

        [TestMethod]
        public void TryParse_With_Ok_No_LineBreaks_No_Number_Gets_Message()
        {
            LogMessage msg = null;
            var msgText = GetMessage(0);
            var dt = DateTime.Now;
            dt = dt.AddSeconds(-dt.Second);
            dt = dt.AddMilliseconds(-dt.Millisecond);
            var origin = "SomeRandomApp.exe";
            var result = LogMessage.TryParse(GetFullMessage("Ok", origin, dt, msgText), 110, out msg);

            Assert.IsTrue(result);
            Assert.IsNotNull(msg);
            Assert.AreEqual(msgText, msg.Message);
            Assert.AreEqual(dt.ToLongDateString(), msg.TimeStamp.ToLongDateString());
            Assert.AreEqual(dt.ToLongTimeString(), msg.TimeStamp.ToLongTimeString());
            Assert.AreEqual(110, msg.Number);
            Assert.AreEqual("Ok", msg.MessageType);
            Assert.AreEqual(origin, msg.Origination);
        }

        [TestMethod]
        public void TryParse_With_Ok_LineBreaks_No_Number_Gets_Message()
        {
            LogMessage msg = null;
            var msgText = GetMessage(1);
            var dt = DateTime.Now;
            dt = dt.AddSeconds(-dt.Second);
            dt = dt.AddMilliseconds(-dt.Millisecond);
            var origin = "SomeRandomApp.exe";
            var result = LogMessage.TryParse(GetFullMessage("Ok", origin, dt, msgText), 110, out msg);

            Assert.IsTrue(result);
            Assert.IsNotNull(msg);
            Assert.AreEqual(msgText, msg.Message);
            Assert.AreEqual(dt.ToLongDateString(), msg.TimeStamp.ToLongDateString());
            Assert.AreEqual(dt.ToLongTimeString(), msg.TimeStamp.ToLongTimeString());
            Assert.AreEqual(110, msg.Number);
            Assert.AreEqual("Ok", msg.MessageType);
            Assert.AreEqual(origin, msg.Origination);
        }

        [TestMethod]
        public void TryParse_With_Ok_LongMessage_With_LineBreaks_No_Number_Gets_Message()
        {
            LogMessage msg = null;
            var msgText = GetMessage(2);
            var dt = DateTime.Now;
            dt = dt.AddSeconds(-dt.Second);
            dt = dt.AddMilliseconds(-dt.Millisecond);
            var origin = "SomeRandomApp.exe";
            var result = LogMessage.TryParse(GetFullMessage("Ok", origin, dt, msgText), 110, out msg);

            Assert.IsTrue(result);
            Assert.IsNotNull(msg);
            Assert.AreEqual(msgText, msg.Message);
            Assert.AreEqual(dt.ToLongDateString(), msg.TimeStamp.ToLongDateString());
            Assert.AreEqual(dt.ToLongTimeString(), msg.TimeStamp.ToLongTimeString());
            Assert.AreEqual(110, msg.Number);
            Assert.AreEqual("Ok", msg.MessageType);
            Assert.AreEqual(origin, msg.Origination);
        }

        [TestMethod]
        public void TryParse_With_Ok_No_LineBreaks_With_Number_Gets_Message()
        {
            LogMessage msg = null;
            var msgText = GetMessage(0);
            var dt = DateTime.Now;
            dt = dt.AddSeconds(-dt.Second);
            dt = dt.AddMilliseconds(-dt.Millisecond);
            var origin = "SomeRandomApp.exe";
            var result = LogMessage.TryParse("111. " + GetFullMessage("Ok", origin, dt, msgText), 110, out msg);

            Assert.IsTrue(result);
            Assert.IsNotNull(msg);
            Assert.AreEqual(msgText, msg.Message);
            Assert.AreEqual(dt.ToLongDateString(), msg.TimeStamp.ToLongDateString());
            Assert.AreEqual(dt.ToLongTimeString(), msg.TimeStamp.ToLongTimeString());
            Assert.AreEqual(111, msg.Number);
            Assert.AreEqual("Ok", msg.MessageType);
            Assert.AreEqual(origin, msg.Origination);
        }

        [TestMethod]
        public void TryParse_With_Ok_LineBreaks_With_Number_Gets_Message()
        {
            LogMessage msg = null;
            var msgText = GetMessage(1);
            var dt = DateTime.Now;
            dt = dt.AddSeconds(-dt.Second);
            dt = dt.AddMilliseconds(-dt.Millisecond);
            var origin = "SomeRandomApp.exe";
            var result = LogMessage.TryParse("111. " + GetFullMessage("Ok", origin, dt, msgText), 110, out msg);

            Assert.IsTrue(result);
            Assert.IsNotNull(msg);
            Assert.AreEqual(msgText, msg.Message);
            Assert.AreEqual(dt.ToLongDateString(), msg.TimeStamp.ToLongDateString());
            Assert.AreEqual(dt.ToLongTimeString(), msg.TimeStamp.ToLongTimeString());
            Assert.AreEqual(111, msg.Number);
            Assert.AreEqual("Ok", msg.MessageType);
            Assert.AreEqual(origin, msg.Origination);
        }

        [TestMethod]
        public void TryParse_With_Ok_LongMessage_With_LineBreaks_With_Number_Gets_Message()
        {
            LogMessage msg = null;
            var msgText = GetMessage(2);
            var dt = DateTime.Now;
            dt = dt.AddSeconds(-dt.Second);
            dt = dt.AddMilliseconds(-dt.Millisecond);
            var origin = "SomeRandomApp.exe";
            var result = LogMessage.TryParse("111. " + GetFullMessage("Ok", origin, dt, msgText), 110, out msg);

            Assert.IsTrue(result);
            Assert.IsNotNull(msg);
            Assert.AreEqual(msgText, msg.Message);
            Assert.AreEqual(dt.ToLongDateString(), msg.TimeStamp.ToLongDateString());
            Assert.AreEqual(dt.ToLongTimeString(), msg.TimeStamp.ToLongTimeString());
            Assert.AreEqual(111, msg.Number);
            Assert.AreEqual("Ok", msg.MessageType);
            Assert.AreEqual(origin, msg.Origination);
        }

        #endregion

        [TestMethod]
        public void ToString_NoColor_Pad3_Pad6_Pad12_Returns_Expected()
        {
            var dt = DateTime.Now;
            var msg = new LogMessage(1, "Error", "Origin.exe", dt, "Test Message");
            var expected = string.Format(
                "  1. {0} | Origin.exe   |  Error: Test Message",
                dt.ToString("yy/MM/dd HH:mm:ss"));
            var actual = msg.ToString(false, 3, 6, 12);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_NoColor_Pad0_Pad0_Pad0_Returns_Expected()
        {
            var dt = DateTime.Now;
            var msg = new LogMessage(1, "Notice", "Origin.exe", dt, "Test Message");
            var expected = string.Format(
                "1. {0} | Origin.exe | Notice: Test Message",
                dt.ToString("yy/MM/dd HH:mm:ss"));
            var actual = msg.ToString(false, 0, 0, 0);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_Notice_Color_Pad0_Pad0_Pad0_Returns_Expected()
        {
            var dt = DateTime.Now;
            var msg = new LogMessage(1, "Notice", "Origin.exe", dt, "Test Message");
            var expected = string.Format(
                ConsoleBase.Colors.LogHeaders.FormatText(false, "1. {0} | Origin.exe | {1}Notice: Test Message{2}"),
                dt.ToString("yy/MM/dd HH:mm:ss"),
                ConsoleBase.Colors.Notice.FormatText(false, ""),
                ColorFormat.CloseTextFormat(""));
            var actual = msg.ToString(true, 0, 0, 0);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_Error_Color_Pad0_Pad0_Pad0_Returns_Expected()
        {
            var dt = DateTime.Now;
            var msg = new LogMessage(1, "Error", "Origin.exe", dt, "Test Message");
            var expected = string.Format(
                ConsoleBase.Colors.LogHeaders.FormatText(false, "1. {0} | Origin.exe | {1}Error: Test Message{2}"),
                dt.ToString("yy/MM/dd HH:mm:ss"),
                ConsoleBase.Colors.Error.FormatText(false, ""),
                ColorFormat.CloseTextFormat(""));
            var actual = msg.ToString(true, 0, 0, 0);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_Warning_Color_Pad0_Pad0_Pad0_Returns_Expected()
        {
            var dt = DateTime.Now;
            var msg = new LogMessage(1, "Warning", "Origin.exe", dt, "Test Message");
            var expected = string.Format(
                ConsoleBase.Colors.LogHeaders.FormatText(false, "1. {0} | Origin.exe | {1}Warning: Test Message{2}"),
                dt.ToString("yy/MM/dd HH:mm:ss"),
                ConsoleBase.Colors.Warning.FormatText(false, ""),
                ColorFormat.CloseTextFormat(""));
            var actual = msg.ToString(true, 0, 0, 0);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_Info_Color_Pad0_Pad0_Pad0_Returns_Expected()
        {
            var dt = DateTime.Now;
            var msg = new LogMessage(1, "Info", "Origin.exe", dt, "Test Message");
            var expected = string.Format(
                ConsoleBase.Colors.LogHeaders.FormatText(false, "1. {0} | Origin.exe | {1}Info: Test Message{2}"),
                dt.ToString("yy/MM/dd HH:mm:ss"),
                ConsoleBase.Colors.Notice.FormatText(false, ""),
                ColorFormat.CloseTextFormat(""));
            var actual = msg.ToString(true, 0, 0, 0);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_Ok_Color_Pad0_Pad0_Pad0_Returns_Expected()
        {
            var dt = DateTime.Now;
            var msg = new LogMessage(1, "Ok", "Origin.exe", dt, "Test Message");
            var expected = string.Format(
                ConsoleBase.Colors.LogHeaders.FormatText(false, "1. {0} | Origin.exe | {1}Ok: Test Message{2}"),
                dt.ToString("yy/MM/dd HH:mm:ss"),
                ConsoleBase.Colors.Notice.FormatText(false, ""),
                ColorFormat.CloseTextFormat(""));
            var actual = msg.ToString(true, 0, 0, 0);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_Unknown_Color_Pad0_Pad0_Pad0_Returns_Expected()
        {
            var dt = DateTime.Now;
            var msg = new LogMessage(1, "Argh", "Origin.exe", dt, "Test Message");
            var expected = string.Format(
                ConsoleBase.Colors.LogHeaders.FormatText(false, "1. {0} | Origin.exe | {1}Argh: Test Message{2}"),
                dt.ToString("yy/MM/dd HH:mm:ss"),
                ConsoleBase.Colors.Subtle.FormatText(false, ""),
                ColorFormat.CloseTextFormat(""));
            var actual = msg.ToString(true, 0, 0, 0);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToString_Returns_Expected()
        {
            var dt = DateTime.Now;
            var msg = new LogMessage(1, "Notice", "Origin.exe", dt, "Test Message");
            var expected = string.Format(
                "1. {0} | Origin.exe | Notice: Test Message",
                dt.ToString("yy/MM/dd HH:mm:ss"));
            var actual = msg.ToString();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Equals_Returns_True_When_InstanceDetails_Are_Equal()
        {
            var dt = DateTime.Now;
            var e1 = new LogMessage(1, "Error", "123.exe", dt, "Hello World!");
            var e2 = new LogMessage(1, "Error", "123.exe", dt, "Hello World!");

            Assert.IsTrue(e1.Equals(e2));
        }

        [TestMethod]
        public void Equals_Returns_False_When_Number_Differs()
        {
            var dt = DateTime.Now;
            var e1 = new LogMessage(1, "Error", "123.exe", dt, "Hello World!");
            var e2 = new LogMessage(2, "Error", "123.exe", dt, "Hello World!");

            Assert.IsFalse(e1.Equals(e2));
        }

        [TestMethod]
        public void Equals_Returns_False_When_Type_Differs()
        {
            var dt = DateTime.Now;
            var e1 = new LogMessage(1, "Error", "123.exe", dt, "Hello World!");
            var e2 = new LogMessage(1, "Warning", "123.exe", dt, "Hello World!");

            Assert.IsFalse(e1.Equals(e2));
        }

        [TestMethod]
        public void Equals_Returns_False_When_Origination_Differs()
        {
            var dt = DateTime.Now;
            var e1 = new LogMessage(1, "Error", "123.exe", dt, "Hello World!");
            var e2 = new LogMessage(1, "Error", "1234.exe", dt, "Hello World!");

            Assert.IsFalse(e1.Equals(e2));
        }

        [TestMethod]
        public void Equals_Returns_False_When_DateTime_Differs()
        {
            var dt = DateTime.Now;
            var e1 = new LogMessage(1, "Error", "123.exe", dt, "Hello World!");
            var e2 = new LogMessage(1, "Error", "123.exe", dt.AddSeconds(1), "Hello World!");

            Assert.IsFalse(e1.Equals(e2));
        }

        [TestMethod]
        public void Equals_Returns_False_When_Message_Differs()
        {
            var dt = DateTime.Now;
            var e1 = new LogMessage(1, "Error", "123.exe", dt, "Hello World!");
            var e2 = new LogMessage(2, "Error", "123.exe", dt, "Hello World");

            Assert.IsFalse(e1.Equals(e2));
        }

        [TestMethod]
        public void Equals_Returns_False_When_SecondInstance_IsNull()
        {
            var dt = DateTime.Now;
            var e1 = new LogMessage(1, "Error", "123.exe", dt, "Hello World!");

            Assert.IsFalse(e1.Equals(null));
        }

        private static string GetFullMessage(string prefix, string origin, DateTime dt, string msg)
        {
            return string.Format(
                "{0}: {1} # {2}  # {3}",
                prefix,
                origin,
                dt.ToString("yyyy-MM-dd HH:mm:ss"),
                msg);
        }

        private static string GetMessage(int index)
        {
            switch (index)
            {
                case 0:
                    return "This is a test message.";
                case 1:
                    return "This message has a newline.\r\nAnd has additional lines.";
            }

            return "This message has newlines\r\nand has a whole lot of text\r\nwhich makes it a seriously really long message, far longer than\r\nwhat we should\r\n\tnormally see from most messages, just\r\nbecause it needs to be really long for testing purposes, with more than three hundred characters so we can test long strings.";
        }
    }
}
