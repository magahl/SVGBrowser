using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SVGBrowser.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            return View();
        }

        public ActionResult ShowImage(string path) {
            var rawFile = System.IO.File.ReadAllBytes(path);
            var fileName = Path.GetFileName(path);
            return File(rawFile, MimeMapping.GetMimeMapping(path), fileName);
        }

        public ActionResult Search(string query) {
            var result = string.IsNullOrEmpty(query) ? Repo.GetDefault() : Repo.GetFilesMatching(query);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }

    public class Repo {
        public static ISet<string> Files => new HashSet<string>(Directory.GetFiles(@"C:\Git\client-icons\filled", "*.svg").Select(x => x.ToLower()).ToList());
        
        public static IList<string> GetFrom(int nmb) {
            return Files.Skip(nmb).Take(100).ToList();
        }

        public static IList<string> GetFilesMatching(string name) {
            return Files.Where(x => Path.GetFileName(x).Contains(name)).ToList();
        }

        public static IList<string> GetDefault() {
            return Files.Take(100).ToList();
        }
    }
}