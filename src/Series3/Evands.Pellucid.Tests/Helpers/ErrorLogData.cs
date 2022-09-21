using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evands.Pellucid.Helpers
{
    public static class ErrorLogData
    {
        #region ThreeSeriesLogs

        public static string GetBrokenThreeSeriesLog()
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

        public static string GetThreeSeriesLog()
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

        public static string GetThreeSeriesLogForFilterTests()
        {
            return @" SYSTEM LOG:
  1. Notice: nk.exe # 2022-05-03 06:57:10  # User Reboot
  2. Notice: nk.exe # 2022-05-03 06:57:10  # Some other User message
  3. Notice: nk.exe # 2022-05-03 06:57:11  # HDG: Timeout set from registry 2 minutes
  4. Error: nk.exe # 2022-05-03 06:57:11  # Some nk error message
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

        public static string GetBrokenFourSeriesLog()
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

        public static string GetFourSeriesLog()
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
