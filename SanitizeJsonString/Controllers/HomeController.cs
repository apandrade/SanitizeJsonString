using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace SanitizeJsonString.Controllers
{
    public class HomeController : Controller
    {
        private static readonly string inputString = "{ \"name\":{ \"first\":\"Robert\", \"middle\":\"\", \"last\":\"Smith\" }, \"age\":25, \"DOB\":\"-\", \"hobbies\":[\"running\",\"coding\",\"-\"],\"education\":{\"highschool\":\"N/A\",\"college\":\"Yale\"}}";
        public ActionResult Index()
        {
            ViewBag.InputString = inputString;
            ViewBag.SanitizedString = SanitizeJsonString(inputString);
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private static string SanitizeJsonString(string jsonString)
        {
            string result = jsonString;

            string keyPattern = "\"\\w+\":\"N/A\",|\"\\w+\":\"-\",|\"\\w+\":\"\",";
            Regex removeKeyRegex = new Regex(keyPattern);
            result = removeKeyRegex.Replace(result, "");

            string lastKeyPattern = "\"\\w+\":\"N/A\"|\"\\w+\":\"-\"|\"\\w+\":\"\"";
            Regex removeLastKeyRegex = new Regex(lastKeyPattern);
            result = removeLastKeyRegex.Replace(result, "");

            string emptyArrayValuePattern = "\"-\"|\"\"|\"N/A\"";
            Regex removeEmptyArrayValueRegex = new Regex(emptyArrayValuePattern);
            result = removeEmptyArrayValueRegex.Replace(result, "");

            result = RemoveWrongComma(result);

            string blankSpacePattern = " +";
            Regex regexBlankSpace = new Regex(blankSpacePattern);
            result = regexBlankSpace.Replace(result, " ");

            return result;
        }

        private static string RemoveWrongComma(string jsonString)
        {
            string result = jsonString;

            string doubleCommaPattern = ",,";
            Regex regexDoubleComma = new Regex(doubleCommaPattern);
            result = regexDoubleComma.Replace(result, "");

            string startBracketPattern = "{,";
            Regex regexStartBracket = new Regex(startBracketPattern);
            result = regexStartBracket.Replace(result, "{");

            string endBracketPattern = ",}";
            Regex regexEndBracket = new Regex(endBracketPattern);
            result = regexEndBracket.Replace(result, "}");

            string startArrayPattern = @"\[,";
            Regex regexStartArray = new Regex(startArrayPattern);
            result = regexStartArray.Replace(result, "[");

            string endArrayPattern = @",\]";
            Regex regexEndArray = new Regex(endArrayPattern);
            result = regexEndArray.Replace(result, "]");

            return result;
        }
    }
}