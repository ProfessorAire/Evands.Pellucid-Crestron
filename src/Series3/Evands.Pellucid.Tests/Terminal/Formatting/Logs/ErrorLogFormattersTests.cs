using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Crestron.SimplSharp;
using Evands.Pellucid.Helpers;

namespace Evands.Pellucid.Terminal.Formatting.Logs
{
    [TestClass]
    public class ErrorLogFormattersTests
    {
        [TestInitialize]
        public void Setup()
        {
        }

        [TestCleanup]
        public void Cleanup()
        {
            CrestronEnvironment.DevicePlatform = eDevicePlatform.Appliance;
        }

        [TestMethod]
        public void ParseCrestronErrorLog_With_Server_Returns_NoEntries()
        {
            CrestronEnvironment.DevicePlatform = eDevicePlatform.Server;
            var actual = ErrorLogFormatters.ParseCrestronErrorLog(ErrorLogData.GetThreeSeriesLog()).ToList();
            Assert.AreEqual(actual.Count, 0);
        }

        #region ParseCrestronErrorLogThreeSeries

        [TestMethod]
        public void ParseCrestronErrorLog_With_ThreeSeriesLog_Returns_Expected()
        {
            CrestronEnvironment.DevicePlatform = eDevicePlatform.Appliance;
            CrestronEnvironment.ProgramCompatibility = eCrestronSeries.Series3;
            var expectedItem0 = new LogMessage(1, "Notice", "nk.exe", DateTime.Parse("2022-05-03 06:57:10"), "User Reboot");
            var expectedItem3 = new LogMessage(4, "Error", "ConsoleServiceCE.exe", DateTime.Parse("2022-05-03 06:57:40"), "Couldn't find beginning or ending tags");
            var expectedItem6 = new LogMessage(7, "Warning", "CPHProcessor.exe", DateTime.Parse("2022-05-03 06:58:18"), "CPHPlugin: getInstance()");
            var expectedItem56 = new LogMessage(57, "Warning", "ConsoleServiceCE.exe", DateTime.Parse("2022-05-03 21:25:31"), "SHELL connection being closed from address 110.1.12.131.");

            var logItems = ErrorLogFormatters.ParseCrestronErrorLog(ErrorLogData.GetThreeSeriesLog()).ToList();

            Assert.IsTrue(expectedItem0.Equals(logItems[0]), "Item0 Didn't Match: Expected <{0}> was <{1}>", expectedItem0, logItems[0]);
            Assert.IsTrue(expectedItem3.Equals(logItems[3]), "Item3 Didn't Match: Expected <{0}> was <{1}>", expectedItem3, logItems[3]);
            Assert.IsTrue(expectedItem6.Equals(logItems[6]), "Item6 Didn't Match: Expected <{0}> was <{1}>", expectedItem6, logItems[6]);
            Assert.IsTrue(expectedItem56.Equals(logItems[56]), "Item56 Didn't Match: Expected <{0}> was <{1}>", expectedItem56, logItems[56]);
        }

        [TestMethod]
        public void ParseCrestronErrorLog_With_ThreeSeriesLog_And_BrokenErrors_Returns_Expected()
        {
            CrestronEnvironment.DevicePlatform = eDevicePlatform.Appliance;
            CrestronEnvironment.ProgramCompatibility = eCrestronSeries.Series3;
            var expectedMessage = @"[01][ExampleCommands] Exception encountered.
--------Exception 1--------
System.FormatException: FormatException
  at System.Text.StringBuilder.AppendFormat(IFormatProvider provider, String format, Object[] args)
  at System.String.Format(IFormatProvider provider, String format, Object[] args)
  at System.String.Format(String format, Object[] args)
  at Evands.Pellucid.ProDemo.ExampleCommands.WriteEx(Boolean log)
  at System.Reflection.RuntimeMethodInfo.InternalInvoke(RuntimeMethodInfo rtmi, Object obj, BindingFlags invokeAttr, Binder binder, Object parameters, CultureInfo culture, Boolean isBinderDefault, Assembly caller, Boolean verifyAccess, StackCrawlMark& stackMark)
  at System.Reflection.RuntimeMethodInfo.InternalInvoke(Object obj, BindingFlags invokeAttr, Binder binder, Object[] parameters, CultureInfo culture, Boolean verifyAccess, StackCrawlMark& stackMark)
  at System.Reflection.RuntimeMethodInfo.Invoke(Object obj, BindingFlags invokeAttr, Binder binder, Object[] parameters, CultureInfo culture)
  at System.Reflection.MethodBase.Invoke(Object obj, Object[] parameters)
  at Crestron.SimplSharp.Reflection.MethodInfoImpl.Invoke(Object obj, Object[] parameters)
  at Evands.Pellucid.Terminal.Commands.GlobalCommand.ProcessCommand(String commandName, String verb, String defaultValue, Dictionary`2 operandsAndFlags)
  at Evands.Pellucid.Terminal.Commands.GlobalCommand.ExecuteCommand(String args)
  at Crestron.SimplSharpProInternal.SimplSharpProManager.k()

-----------------------------";
            var expectedCount = 2;
            var expectedItem1 = new LogMessage(66, "Error", "SimplSharpPro.exe [App 1]", DateTime.Parse("2022-05-07 09:32:46"), expectedMessage);

            var logItems = ErrorLogFormatters.ParseCrestronErrorLog(ErrorLogData.GetBrokenThreeSeriesLog()).ToList();

            Assert.IsTrue(logItems.Count == expectedCount, "Expected <{0}> items and there were <{1}>", expectedCount, logItems.Count);
            Assert.IsTrue(expectedItem1.Equals(logItems[1]), "Item1 Didn't Match. Expected\r\n<{0}>\r\nActual was\r\n<{1}>", expectedItem1, logItems[1]);
        }

        #endregion

        #region ParseCrestronErrorLogFourSeries

        [TestMethod]
        public void ParseCrestronErrorLog_With_FourSeriesLog_Returns_Expected()
        {
            CrestronEnvironment.DevicePlatform = eDevicePlatform.Appliance;
            CrestronEnvironment.ProgramCompatibility = eCrestronSeries.Series3 | eCrestronSeries.Series4;
            var expectedItem0 = new LogMessage(1, "Notice", "SimplSharpPro[App01]", DateTime.Parse("2022-05-05 18:39:27"), "sync detected: 1");
            var expectedItem1 = new LogMessage(2, "Notice", "ctpd", DateTime.Parse("2022-05-09 16:12:21"), "SHELL Connection 1 from: 101.10.1.123");
            var expectedItem4 = new LogMessage(5, "Warning", "ctpd", DateTime.Parse("2022-05-06 06:35:31"), "SHELL connection 1 being closed from address 101.10.1.123.");
            var expectedItem5 = new LogMessage(6, "Error", "crestErrorLogServer", DateTime.Parse("2022-05-06 13:55:57"), "SD Card Write COUNTER: 326247120");

            var logItems = ErrorLogFormatters.ParseCrestronErrorLog(ErrorLogData.GetFourSeriesLog()).ToList();

            Assert.IsTrue(expectedItem0.Equals(logItems[0]), "Item0 Didn't Match: Expected <{0}> was <{1}>", expectedItem0, logItems[0]);
            Assert.IsTrue(expectedItem1.Equals(logItems[1]), "Item1 Didn't Match: Expected <{0}> was <{1}>", expectedItem1, logItems[1]);
            Assert.IsTrue(expectedItem4.Equals(logItems[4]), "Item4 Didn't Match: Expected <{0}> was <{1}>", expectedItem4, logItems[4]);
            Assert.IsTrue(expectedItem5.Equals(logItems[5]), "Item5 Didn't Match: Expected <{0}> was <{1}>", expectedItem5, logItems[5]);
        }

        [TestMethod]
        public void ParseCrestronErrorLog_With_FourSeriesLog_And_BrokenErrors_Returns_Expected()
        {
            CrestronEnvironment.DevicePlatform = eDevicePlatform.Appliance;
            CrestronEnvironment.ProgramCompatibility = eCrestronSeries.Series3 | eCrestronSeries.Series4;
            var expectedMessage = @"[10][ExampleCommands] Exception encountered.
--------Exception 1--------
System.FormatException: Index (zero based) must be greater than or equal to zero and less than the size of the argument list.
  at System.Text.StringBuilder.AppendFormatHelper (System.IFormatProvider provider, System.String format, System.ParamsArray args) [0x000f9] in <2ad40006e9c141b299c9c7c533021a74>:0
  at System.String.FormatHelper (System.IFormatProvider provider, System.String format, System.ParamsArray args) [0x00023] in <2ad40006e9c141b299c9c7c533021a74>:0
  at System.String.Format (System.String format, System.Object[] args) [0x00020] in <2ad40006e9c141b299c9c7c533021a74>:0
  at Evands.Pellucid.ProDemo.ExampleCommands.WriteEx (System.Boolean log) [0x00002] in <13a4773037194d369e3948ff53170bec>:0
-----------------------------";

            var expectedCount = 3;
            var expectedItem2 = new LogMessage(3, "Error", "SimplSharpPro[App10]", DateTime.Parse("2022-05-09 16:21:20"), expectedMessage);

            var logItems = ErrorLogFormatters.ParseCrestronErrorLog(ErrorLogData.GetBrokenFourSeriesLog()).ToList();

            Assert.IsTrue(logItems.Count == expectedCount, "Expected <{0}> items and there were <{1}>", expectedCount, logItems.Count);
            Assert.IsTrue(expectedItem2.Equals(logItems[2]), "Item2 Didn't Match. Expected\r\n<{0}>\r\nActual was\r\n<{1}>", expectedItem2, logItems[2]);
        }

        #endregion

        #region PrettyPrintErrorLog

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PrettyPrintErrorLog_Throws_ArgumentNullException_With_Null_Enumerable()
        {
            ErrorLogFormatters.PrintPrettyErrorLog(null, false);
        }

        [TestMethod]
        public void PrettyPrintErrorLog_NoColor_Returns_ExpectedMessage_When_Items_Empty()
        {
            var expected = "No Messages to Display";
            var actual = ErrorLogFormatters.PrintPrettyErrorLog(new List<LogMessage>() as IEnumerable<LogMessage>, false);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PrettyPrintErrorLog_Color_Returns_ExpectedMessage_When_Items_Empty()
        {
            var expected = ConsoleBase.Colors.Warning.FormatText("No Messages to Display");
            var actual = ErrorLogFormatters.PrintPrettyErrorLog(new List<LogMessage>() as IEnumerable<LogMessage>, true);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PrettyPrintErrorLog_ThreeSeries_NoColor_Returns_Expected_String()
        {
            var items = new List<LogMessage>
            {
                new LogMessage(1, "Notice", "nk.exe", new DateTime(2022, 5, 4, 11, 11, 11), "User Reboot"),
                new LogMessage(2, "Error", "ConsoleServiceCE.exe", new DateTime(2022, 5, 4, 11, 12, 11), "Couldn't find beginning or ending tags"),
                new LogMessage(10, "Warning", "CPHProcessor.exe", new DateTime(2022, 5, 5, 11, 11, 11), "CPHPlugin: getInstance()"),
                new LogMessage(250, "Notice", "nk.exe", new DateTime(2022, 5, 6, 11, 11, 11), "User Reboot"),
                new LogMessage(300, "Notice", "nk.exe", new DateTime(2022, 5, 7, 11, 11, 11), "User Reboot"),
                new LogMessage(1220, "Notice", "nk.exe", new DateTime(2022, 5, 8, 11, 11, 11), "User Reboot"),
                new LogMessage(1221, "Notice", "nk.exe", new DateTime(2022, 5, 9, 11, 11, 11), "User Reboot"),
                new LogMessage(1223, "Notice", "nk.exe", new DateTime(2022, 5, 10, 11, 11, 11), "User Reboot"),
                new LogMessage(1224, "Notice", "nk.exe", new DateTime(2022, 5, 11, 11, 11, 11), "User Reboot"),
                new LogMessage(2222, "Notice", "nk.exe", new DateTime(2022, 5, 12, 11, 11, 11), "User Reboot"),
                new LogMessage(2323, "Notice", "nk.exe", new DateTime(2022, 5, 13, 11, 11, 11), "User Reboot"),
                new LogMessage(2442, "Notice", "nk.exe", new DateTime(2022, 5, 14, 11, 11, 11), "User Reboot"),
            };

            var expected = @"   1. 22/05/04 11:11:11 | nk.exe               |  Notice: User Reboot
   2. 22/05/04 11:12:11 | ConsoleServiceCE.exe |   Error: Couldn't find beginning or ending tags
  10. 22/05/05 11:11:11 | CPHProcessor.exe     | Warning: CPHPlugin: getInstance()
 250. 22/05/06 11:11:11 | nk.exe               |  Notice: User Reboot
 300. 22/05/07 11:11:11 | nk.exe               |  Notice: User Reboot
1220. 22/05/08 11:11:11 | nk.exe               |  Notice: User Reboot
1221. 22/05/09 11:11:11 | nk.exe               |  Notice: User Reboot
1223. 22/05/10 11:11:11 | nk.exe               |  Notice: User Reboot
1224. 22/05/11 11:11:11 | nk.exe               |  Notice: User Reboot
2222. 22/05/12 11:11:11 | nk.exe               |  Notice: User Reboot
2323. 22/05/13 11:11:11 | nk.exe               |  Notice: User Reboot
2442. 22/05/14 11:11:11 | nk.exe               |  Notice: User Reboot
";
            Options.Instance.DefaultLogTimestampFormat = "yy/MM/dd HH:mm:ss";
            var actual = ErrorLogFormatters.PrintPrettyErrorLog(items.AsEnumerable(), false);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PrettyPrintErrorLog_ThreeSeries_Color_Returns_Expected_String()
        {
            var items = new List<LogMessage>
            {
                new LogMessage(1, "Notice", "nk.exe", new DateTime(2022, 5, 4, 11, 11, 11), "User Reboot"),
                new LogMessage(2, "Error", "ConsoleServiceCE.exe", new DateTime(2022, 5, 4, 11, 12, 11), "Couldn't find beginning or ending tags"),
                new LogMessage(10, "Warning", "CPHProcessor.exe", new DateTime(2022, 5, 5, 11, 11, 11), "CPHPlugin: getInstance()"),
                new LogMessage(250, "Notice", "nk.exe", new DateTime(2022, 5, 6, 11, 11, 11), "User Reboot"),
                new LogMessage(300, "Notice", "nk.exe", new DateTime(2022, 5, 7, 11, 11, 11), "User Reboot"),
                new LogMessage(1220, "Notice", "nk.exe", new DateTime(2022, 5, 8, 11, 11, 11), "User Reboot"),
                new LogMessage(1221, "Notice", "nk.exe", new DateTime(2022, 5, 9, 11, 11, 11), "User Reboot"),
                new LogMessage(1223, "Notice", "nk.exe", new DateTime(2022, 5, 10, 11, 11, 11), "User Reboot"),
                new LogMessage(1224, "Notice", "nk.exe", new DateTime(2022, 5, 11, 11, 11, 11), "User Reboot"),
                new LogMessage(2222, "Notice", "nk.exe", new DateTime(2022, 5, 12, 11, 11, 11), "User Reboot"),
                new LogMessage(2323, "Notice", "nk.exe", new DateTime(2022, 5, 13, 11, 11, 11), "User Reboot"),
                new LogMessage(2442, "Notice", "nk.exe", new DateTime(2022, 5, 14, 11, 11, 11), "User Reboot"),
            };

            var expected = string.Format(@"{1}   1. 22/05/04 11:11:11 | nk.exe               | {2} Notice: User Reboot{0}
{1}   2. 22/05/04 11:12:11 | ConsoleServiceCE.exe | {4}  Error: Couldn't find beginning or ending tags{0}
{1}  10. 22/05/05 11:11:11 | CPHProcessor.exe     | {3}Warning: CPHPlugin: getInstance(){0}
{1} 250. 22/05/06 11:11:11 | nk.exe               | {2} Notice: User Reboot{0}
{1} 300. 22/05/07 11:11:11 | nk.exe               | {2} Notice: User Reboot{0}
{1}1220. 22/05/08 11:11:11 | nk.exe               | {2} Notice: User Reboot{0}
{1}1221. 22/05/09 11:11:11 | nk.exe               | {2} Notice: User Reboot{0}
{1}1223. 22/05/10 11:11:11 | nk.exe               | {2} Notice: User Reboot{0}
{1}1224. 22/05/11 11:11:11 | nk.exe               | {2} Notice: User Reboot{0}
{1}2222. 22/05/12 11:11:11 | nk.exe               | {2} Notice: User Reboot{0}
{1}2323. 22/05/13 11:11:11 | nk.exe               | {2} Notice: User Reboot{0}
{1}2442. 22/05/14 11:11:11 | nk.exe               | {2} Notice: User Reboot{0}
",
    ColorFormat.CloseTextFormat(string.Empty),
    ConsoleBase.Colors.LogHeaders.FormatText(false, string.Empty),
    ConsoleBase.Colors.Notice.FormatText(false, string.Empty),
    ConsoleBase.Colors.Warning.FormatText(false, string.Empty),
    ConsoleBase.Colors.Error.FormatText(false, string.Empty));

            var actual = ErrorLogFormatters.PrintPrettyErrorLog(items.AsEnumerable(), true);

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
