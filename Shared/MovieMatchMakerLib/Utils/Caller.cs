using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MovieMatchMakerLib.Utils
{
    public static class Caller
    {
        public static string MemberName([CallerMemberName] string callerMemberName = "") => callerMemberName;

        public static string MemberNameDegree2([CallerMemberName] string callerMemberName = "")
        {
            // ?
            return MemberName();
        }

        public static (string Name, string path, int Line) MemberNameLocationValues([CallerMemberName] string callerMemberName = "",
                                                                     [CallerFilePath] string callerFilePath = "",
                                                                     [CallerLineNumber] int callerLineNumber = -1)
        {
            return (callerMemberName, callerFilePath, callerLineNumber);
        }

        public static string MemberNameLocationAll([CallerMemberName] string callerMemberName = "",
                                                   [CallerFilePath] string callerFilePath = "",
                                                   [CallerLineNumber] int callerLineNumber = -1)
        {
            return $"{callerFilePath}: {callerLineNumber} - {callerMemberName}()";
        }

        public static string MemberNameLocatio([CallerMemberName] string callerMemberName = "",
                                               [CallerFilePath] string callerFilePath = "",
                                               [CallerLineNumber] int callerLineNumber = -1,
                                               bool memberName = false,
                                               bool filePath = false,
                                               bool lineNumber = false)
        {
            return $"{(filePath? callerFilePath:"")}: {(lineNumber? callerLineNumber:"")} - {(memberName? callerMemberName+"()":"")}";
        }

        public static string MemberNameExpression<T>(T value,
                                                     [CallerMemberName] string callerMemberName = "",                                                  
                                                     [CallerArgumentExpression(nameof(value))] string valueExpression = "")
        {
            return $"{callerMemberName}({valueExpression}) was called with {value}";
        }
        public static string ExpressionLocation<T>(T value,
                                                   [CallerMemberName] string callerMemberName = "",
                                                   [CallerArgumentExpression(nameof(value))] string valueExpression = "")
        {
            return $"{callerMemberName}(...):  {valueExpression} was {value}";
        }

        public static string FilePath([CallerFilePath] string callerFilePath = "") => callerFilePath;

        public static int LineNumber([CallerLineNumber] int callerLineNumber = -1) => callerLineNumber;

        public static string ArgumentExpression<T>(T value, 
                                                   [CallerArgumentExpression(nameof(value))] string valueExpression = "")
        {
            return $"{valueExpression} was {value}";
        }

        public static string ArgumentExpression<T, U>(T tValue, U uValue,
                                                      [CallerArgumentExpression(nameof(tValue))] string tValueExpression = "",
                                                      [CallerArgumentExpression(nameof(uValue))] string uValueExpression = "",
                                                      string separator = "\n")
        {
            return $"{tValueExpression} was {tValue}{separator}{uValueExpression} was {uValue}";
        }

        public static void ActionExpression(Action value,
                                            [CallerArgumentExpression(nameof(value))] string valueExpression = "",
                                            [CallerMemberName] string callerMemberName = "")
        {
            Console.Out.Write($"{callerMemberName}(...) - calling {valueExpression}... ");
            value();
            Console.Out.WriteLine("returned");
            
        }
    }
}
