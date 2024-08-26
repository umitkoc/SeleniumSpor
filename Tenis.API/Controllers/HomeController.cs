using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Tenis.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HomeController : ControllerBase
{
   
    [ActionName("GetTenis"),HttpGet]
    public ActionResult<List<Model>> GetTenis(SeleniumSporRequest request)
    {
        var driverService = ChromeDriverService.CreateDefaultService();
        var chromeOptions = new ChromeOptions();
        driverService.HideCommandPromptWindow = true;
        chromeOptions.AddArguments("--disable-infobars");
        chromeOptions.AddArguments("--headless=new");
        chromeOptions.AddArguments("--disable-gpu");
        chromeOptions.AddArguments("--no-sandbox");
        chromeOptions.AddArguments("--disable-dev-shm-usage");
        chromeOptions.PageLoadStrategy = PageLoadStrategy.Normal;
        var driver = new ChromeDriver(driverService, chromeOptions);
        driver.Url = "https://online.spor.istanbul/uyeseanssecim";
        var tcnum= driver.FindElement(By.Name("txtTCPasaport"));
        tcnum.SendKeys(request.tcnumber);
        var password= driver.FindElement(By.Name("txtSifre"));
        password.SendKeys(request.password);
        driver.FindElement(By.Name("btnGirisYap")).Click();
        Thread.Sleep(5000);
        driver.FindElement(By.Id("checkBox")).Click();
        Thread.Sleep(5000);
        driver.FindElement(By.Id("closeModal")).Click();
        driver.Navigate().GoToUrl("https://online.spor.istanbul/uyeseanssecim");
        Thread.Sleep(5000);
        driver.FindElement(By.Id("pageContent_rptListe_lbtnSeansSecim_0")).Click();
        Thread.Sleep(5000);
        var allDays = driver.FindElement(By.Id("dvScheduler"));
        IReadOnlyCollection<IWebElement> colMd1Elements = allDays.FindElements(By.ClassName("col-md-1"));
        var row = 0;
        var models = new List<Model>();
        foreach (var item in colMd1Elements)
        {
            var model = new Model
            {
                Day = item.FindElement(By.TagName("h3")).Text.Replace("\n"," ")
            };
            if (item.FindElement(By.ClassName("panel-body")).Text != "")
            {
                IReadOnlyCollection<IWebElement> groups = item.FindElements(By.ClassName("form-group"));
                var col = 0;
                foreach (var group in groups)
                {
                    var form = new Form
                    {
                        State = group.FindElement(By.ClassName("label-default")).Text,
                        Time = group.FindElement(By.Id($"pageContent_rptList_ChildRepeater_{row}_lblSeansSaat_{col}")).Text
                    };
                    col++;
                    
                 
                    if(request.all)
                    {
                        model.Forms.Add(form);
                    }
                    else if (form.State=="Yer Var")
                    {
                        model.Forms.Add(form);
                    }
                }
            }
            row++;
            if (model.Forms.Count > 0)
            {
                models.Add(model);
            }
        }
        driver.Close();
        return models;
    }
}