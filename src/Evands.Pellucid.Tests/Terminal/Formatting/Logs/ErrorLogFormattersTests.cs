using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Crestron.SimplSharp;

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
        }

        public void ParseCrestronErrorLog_With_Server_Returns_NoEntries()
        {
            CrestronEnvironment.DevicePlatform = eDevicePlatform.Appliance;
            var actual = ErrorLogFormatters.ParseCrestronErrorLog(this.GetThreeSeriesLog()).ToList();
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

            var logItems = ErrorLogFormatters.ParseCrestronErrorLog(this.GetThreeSeriesLog()).ToList();

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

            var logItems = ErrorLogFormatters.ParseCrestronErrorLog(this.GetBrokenThreeSeriesLog()).ToList();

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
            
            var logItems = ErrorLogFormatters.ParseCrestronErrorLog(this.GetFourSeriesLog()).ToList();

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

            var logItems = ErrorLogFormatters.ParseCrestronErrorLog(this.GetBrokenFourSeriesLog()).ToList();

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

        #region ThreeSeriesLogs

        private string GetBrokenThreeSeriesLog()
        {
            return @" SYSTEM LOG:
 65. Notice: ConsoleServiceCE.exe # 2022-05-07 09:32:08  # SHELL Connection from: 123.123.123.123
 66. Error: SimplSharpPro.exe [App 1] # 2022-05-07 09:32:46  # [01][ExampleCommands] Exception encountered.
--------Exception 1--------
System.FormatException: FormatException
   at System.Text.StringBuilder.AppendFormat(IFormatProvider provider, String format, Object[] args)
   at System.String.Format(IFormatPro 67. Error: SimplSharpPro.exe [App 1] # 2022-05-07 09:32:46  # vider provider, String format, Object[] args)
   at System.String.Format(String format, Object[] args)
   at Evands.Pellucid.ProDemo.ExampleCommands.WriteEx(Boolean log)
   at System.Reflection.RuntimeMethodInfo.InternalInvoke(RuntimeMethodInfo rtmi, O 68. Error: SimplSharpPro.exe [App 1] # 2022-05-07 09:32:46  # bject obj, BindingFlags invokeAttr, Binder binder, Object parameters, CultureInfo culture, Boolean isBinderDefault, Assembly caller, Boolean verifyAccess, StackCrawlMark& stackMark)
   at System.Reflection.RuntimeMethodInfo.InternalInvoke(Object obj, Bin 69. Error: SimplSharpPro.exe [App 1] # 2022-05-07 09:32:46  # dingFlags invokeAttr, Binder binder, Object[] parameters, CultureInfo culture, Boolean verifyAccess, StackCrawlMark& stackMark)
   at System.Reflection.RuntimeMethodInfo.Invoke(Object obj, BindingFlags invokeAttr, Binder binder, Object[] parameters, Cult 70. Error: SimplSharpPro.exe [App 1] # 2022-05-07 09:32:46  # ureInfo culture)
   at System.Reflection.MethodBase.Invoke(Object obj, Object[] parameters)
   at Crestron.SimplSharp.Reflection.MethodInfoImpl.Invoke(Object obj, Object[] parameters)
   at Evands.Pellucid.Terminal.Commands.GlobalCommand.ProcessCommand 71. Error: SimplSharpPro.exe [App 1] # 2022-05-07 09:32:46  # (String commandName, String verb, String defaultValue, Dictionary`2 operandsAndFlags)
   at Evands.Pellucid.Terminal.Commands.GlobalCommand.ExecuteCommand(String args)
   at Crestron.SimplSharpProInternal.SimplSharpProManager.k()

-------------------- 72. Error: SimplSharpPro.exe [App 1] # 2022-05-07 09:32:46  # ---------


 Total Msgs Logged = 357
 END OF SYSTEM LOG";
        }

        private string GetThreeSeriesLog()
        {
            return @" SYSTEM LOG:
  1. Notice: nk.exe # 2022-05-03 06:57:10  # User Reboot
  2. Notice: nk.exe # 2022-05-03 06:57:10  # System startup: CP3 [v1.8001.0146 (Mar 08 2022) #007DF147]
  3. Notice: nk.exe # 2022-05-03 06:57:11  # HDG: Timeout set from registry 2 minutes
  4. Error: ConsoleServiceCE.exe # 2022-05-03 06:57:40  # Couldn't find beginning or ending tags
  5. Error: ConsoleServiceCE.exe # 2022-05-03 06:57:40  # Couldn't find beginning or ending tags
  6. Notice: CIPCommandProcessor.exe # 2022-05-03 06:58:11  # Secure WebSocket server Sucessfully Created at port 49200
  7. Warning: CPHProcessor.exe # 2022-05-03 06:58:18  # CPHPlugin: getInstance()
  8. Notice: CPHProcessor.exe # 2022-05-03 06:58:18  # CresStore: Connections OK to redis

  9. Notice: HydrogenManager.exe # 2022-05-03 06:58:35  # CresStore: Connections OK to redis

 10. Notice: SimplSharpPro.exe # 2022-05-03 06:58:56  # Starting Programs...!
 11. Notice: SimplSharpPro.exe # 2022-05-03 06:58:57  # Flash Policy Server - Starting Server.
 12. Notice: SimplSharpPro.exe # 2022-05-03 06:58:57  # Flash Policy Server - Server Started.
 13. Notice: ConsoleServiceCE.exe # 2022-05-03 06:59:14  # SHELL Connection from: 110.1.12.131
 14. Notice: SimplSharpPro.exe [App 9] # 2022-05-03 06:59:20  # **Program 9 Started**
 15. Notice: TLDM.exe # 2022-05-03 06:59:22  #  Event rcvd is 8
 16. Notice: SimplSharpPro.exe [App 10] # 2022-05-03 07:02:36  # **Program 10 Started**
 17. Notice: TLDM.exe # 2022-05-03 07:02:37  #  Event rcvd is 8
 18. Notice: TLDM.exe # 2022-05-03 07:03:03  # **Program 10 Stopped**
 19. Notice: SimplSharpPro.exe [App 10] # 2022-05-03 07:03:56  # **Program 10 Started**
 20. Notice: TLDM.exe # 2022-05-03 07:03:58  #  Event rcvd is 8
 21. Notice: TLDM.exe # 2022-05-03 07:08:07  # **Program 10 Stopped**
 22. Notice: SimplSharpPro.exe [App 10] # 2022-05-03 07:08:29  # **Program 10 Started**
 23. Notice: TLDM.exe # 2022-05-03 07:08:31  #  Event rcvd is 8
 24. Notice: TLDM.exe # 2022-05-03 07:24:46  # **Program 10 Stopped**
 25. Notice: SimplSharpPro.exe [App 10] # 2022-05-03 07:25:08  # **Program 10 Started**
 26. Notice: TLDM.exe # 2022-05-03 07:25:10  #  Event rcvd is 8
 27. Notice: TLDM.exe # 2022-05-03 07:25:37  # **Program 10 Stopped**
 28. Notice: SimplSharpPro.exe [App 10] # 2022-05-03 07:25:59  # **Program 10 Started**
 29. Notice: TLDM.exe # 2022-05-03 07:26:04  #  Event rcvd is 8
 30. Notice: TLDM.exe # 2022-05-03 07:26:48  # **Program 10 Stopped**
 31. Notice: SimplSharpPro.exe [App 10] # 2022-05-03 07:27:11  # **Program 10 Started**
 32. Notice: TLDM.exe # 2022-05-03 07:27:15  #  Event rcvd is 8
 33. Warning: ConsoleServiceCE.exe # 2022-05-03 07:31:44  # SHELL connection being closed from address 110.1.12.131.

 34. Notice: ConsoleServiceCE.exe # 2022-05-03 09:08:50  # SHELL Connection from: 110.1.12.131
 35. Notice: TLDM.exe # 2022-05-03 09:08:53  # **Program 10 Stopped**
 36. Notice: SimplSharpPro.exe [App 10] # 2022-05-03 09:09:46  # **Program 10 Started**
 37. Notice: TLDM.exe # 2022-05-03 09:09:47  #  Event rcvd is 8
 38. Notice: TLDM.exe # 2022-05-03 10:40:59  # **Program 10 Stopped**
 39. Notice: SimplSharpPro.exe [App 10] # 2022-05-03 10:41:52  # **Program 10 Started**
 40. Notice: TLDM.exe # 2022-05-03 10:41:53  #  Event rcvd is 8
 41. Notice: TLDM.exe # 2022-05-03 11:06:01  # **Program 10 Stopped**
 42. Notice: ConsoleServiceCE.exe # 2022-05-03 11:37:10  # SHELL Connection from: 110.1.12.131
 43. Warning: ConsoleServiceCE.exe # 2022-05-03 11:37:10  # SHELL connection being closed from address 110.1.12.131.

 44. Notice: SimplSharpPro.exe [App 10] # 2022-05-03 11:37:40  # **Program 10 Started**
 45. Notice: TLDM.exe # 2022-05-03 11:37:42  #  Event rcvd is 8
 46. Notice: TLDM.exe # 2022-05-03 11:37:47  # **Program 10 Stopped**
 47. Notice: ConsoleServiceCE.exe # 2022-05-03 11:38:07  # SHELL Connection from: 110.1.12.131
 48. Notice: SimplSharpPro.exe [App 10] # 2022-05-03 11:38:44  # **Program 10 Started**
 49. Notice: TLDM.exe # 2022-05-03 11:38:45  #  Event rcvd is 8
 50. Notice: TLDM.exe # 2022-05-03 11:53:17  # **Program 10 Stopped**
 51. Notice: SimplSharpPro.exe [App 10] # 2022-05-03 11:53:47  # **Program 10 Started**
 52. Notice: TLDM.exe # 2022-05-03 11:53:48  #  Event rcvd is 8
 53. Warning: ConsoleServiceCE.exe # 2022-05-03 13:33:33  # SHELL connection being closed from address 110.1.12.131.

 54. Notice: TLDM.exe # 2022-05-03 13:33:51  # **Program 10 Stopped**
 55. Warning: ConsoleServiceCE.exe # 2022-05-03 13:34:00  # SHELL connection being closed from address 110.1.12.131.

 56. Notice: ConsoleServiceCE.exe # 2022-05-03 16:44:12  # SHELL Connection from: 110.1.12.131
 57. Warning: ConsoleServiceCE.exe # 2022-05-03 21:25:31  # SHELL connection being closed from address 110.1.12.131.

 58. Notice: ConsoleServiceCE.exe # 2022-05-05 17:34:38  # SHELL Connection from: 110.1.12.131
 Total Msgs Logged = 322
 END OF SYSTEM LOG
";
        }

        #endregion

        #region FourSeriesLogs

        private string GetBrokenFourSeriesLog()
        {
            return @"
  1. Notice: ctpd # 2022-05-09 16:12:15 # SHELL connection 1 being closed from address 10.20.0.89.
  2. Notice: ctpd # 2022-05-09 16:12:21 # SHELL Connection 1 from: 10.20.0.89
  3. Error: SimplSharpPro[App10] # 2022-05-09 16:21:20 # [10][ExampleCommands] Exception encountered.
                                                                                                     --------Exception 1--------
System.FormatException: Index (zero based) must be greater than or equal to zero and less than the size of the argument list.
                                                                                                                               at System.Text.StringBuilder.AppendFormatHelper (Syst
  4. Error: SimplSharpPro[App10] # 2022-05-09 16:21:20 # em.IFormatProvider provider, System.String format, System.ParamsArray args) [0x000f9] in <2ad40006e9c141b299c9c7c533021a74>:0
                                                    at System.String.FormatHelper (System.IFormatProvider provider, System.String format, System.ParamsArray args) [0x00023] in <2
  5. Error: SimplSharpPro[App10] # 2022-05-09 16:21:20 # ad40006e9c141b299c9c7c533021a74>:0
                                                                                              at System.String.Format (System.String format, System.Object[] args) [0x00020] in <2ad40006e9c141b299c9c7c533021a74>:0
                                                                                  at Evands.Pellucid.ProDemo.ExampleCommands.WriteEx (System.Boolean log) [0x00002] in <13a477303
  6. Error: SimplSharpPro[App10] # 2022-05-09 16:21:20 # 7194d369e3948ff53170bec>:0
                                                                                    -----------------------------


Total Errors Logged = 6

";
        }

        private string GetFourSeriesLog()
        {
            return @"1. Notice: SimplSharpPro[App01] # 2022-05-05 18:39:27 # sync detected: 1
  2. Notice: ctpd # 2022-05-09 16:12:21 # SHELL Connection 1 from: 101.10.1.123
  3. Notice: SimplSharpPro[App01] # 2022-05-06 06:33:33 # sync detected: 1
  4. Notice: SimplSharpPro[App01] # 2022-05-06 06:33:41 # sync detected: 2
  5. Warning: ctpd # 2022-05-06 06:35:31 # SHELL connection 1 being closed from address 101.10.1.123.
  6. Error: crestErrorLogServer # 2022-05-06 13:55:57 # SD Card Write COUNTER: 326247120


Total Errors Logged = 6";
        }

        #endregion
    }
}
