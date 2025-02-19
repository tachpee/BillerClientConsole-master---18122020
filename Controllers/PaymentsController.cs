﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BillerClientConsole.Globals;
using BillerClientConsole.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Webdev.Payments;

namespace BillerClientConsole.Controllers
{
    [Route("Payments")]
    public class PaymentsController : Controller
    {
        private dbContext db = new dbContext();

        [HttpGet("PaymentHistory")]
        public async Task<IActionResult> PaymentHistory(bool paymentSuccessful)
        {
            var client = new HttpClient();
            var user = db.AspNetUsers.Where(i => i.Email == User.Identity.Name).FirstOrDefault();
            var paymentsHistory = await client.GetAsync($"{Globals.Globals.end_point_payment}{user.UserName}").Result.Content.ReadAsStringAsync();
            var creditsHistory = await client.GetAsync($"{Globals.Globals.end_point_payments_credits}{user.UserName}").Result.Content.ReadAsStringAsync();

            dynamic json_data = JsonConvert.DeserializeObject(paymentsHistory);
            PaymentsResponse response  = JsonConvert.DeserializeObject<PaymentsResponse>(json_data.ToString());

            dynamic json_data_credits = JsonConvert.DeserializeObject(creditsHistory);
            CreditCounts credits = JsonConvert.DeserializeObject<CreditCounts>(json_data_credits.ToString());

            ViewBag.Balance = response.AccountBalance;
            ViewBag.Payments = response.Payments;
            ViewBag.credits = credits;

            return View();
        }

        [HttpPost("MakePayment/{redirect}")]
        public IActionResult MakePayment(string redirect, string Email, int Amount)
        {
            //var paynow = new Paynow("9848", "9cb58118-b8e3-45bc-b5b5-66a29ce71309");
            if(!string.IsNullOrEmpty(Email) && Amount != 0)
            {
                var paynow = new Paynow("10057", "5220513d-580d-402e-8eb4-abceff3efae5");

          
                //paynow.ResultUrl = "https://deedsapp.ttcsglobal.com/Paynow/Result";
                //paynow.ReturnUrl = $"https://deedsapp.ttcsglobal.com/Payments/Response/{redirect}";
                paynow.ResultUrl = "http://localhost:2015/Paynow/Result";
                paynow.ReturnUrl = $"http://localhost:2015/Payments/Response/{redirect}";

                var payment = paynow.CreatePayment(Guid.NewGuid().ToString(), Email);
                payment.Add("Account Topup", Amount);
                var response = paynow.Send(payment);
                if (response.Success())
                {
                    Globals.Globals.payment = paynow;
                    Globals.Globals.response = response;
                    return new RedirectResult(response.RedirectLink());
                }
            }
            return BadRequest();
            
        }

        [HttpGet("Response/{redirect}")]
        public async Task<IActionResult> PaynowRwsponse(string redirect)
        {
            var payment = Globals.Globals.payment;
            var response = Globals.Globals.response;

            var status = payment.PollTransaction(response.PollUrl());
            if (status.Paid())
            {
                var user = db.AspNetUsers.Where(i => i.Email == User.Identity.Name).FirstOrDefault();
                var client = new HttpClient();
                PaymentDto paymnt = new PaymentDto();
                paymnt.UserId = user.UserName;                

                var data = status.GetData();
                if (data.ContainsKey("reference"))
                    paymnt.PaymentId = data["reference"];

                if (data.ContainsKey("paynowreference"))
                    paymnt.PaynowReference = data["paynowreference"];

                paymnt.Date = DateTime.Now.ToString();
                paymnt.Description = "Account topup";
                paymnt.AmountCr = (double) status.Amount;
                var responsey = await client.PostAsJsonAsync<PaymentDto>($"http://localhost:800/Payments/RecordPayment", paymnt).Result.Content.ReadAsStringAsync();
                if (redirect.Equals("PaymentHistory"))
                    return RedirectToAction("PaymentHistory");
                else
                    return Redirect("/Home/Dashboard");
                //PostSearchResponse psr = JsonConvert.DeserializeObject<PostSearchResponse>(responsey);
                
            }

            return BadRequest("Your payment was not successful");
        }

        [HttpPost("PurchaseCredits/{redirect}")]
        public async Task<IActionResult> PurchaseCreditsAsync(string redirect, CreditPurchaseDto puchase)
        {
            var client = new HttpClient();
            if(puchase != null && puchase.NumberOfCredits > 0)
            {
                var user = db.AspNetUsers.Where(i => i.Email == User.Identity.Name).FirstOrDefault();
                puchase.UserID = user.UserName;
                var responsey = await client.PostAsJsonAsync<CreditPurchaseDto>($"http://localhost:800/Payments/PurchaseCredits", puchase).Result.Content.ReadAsStringAsync();
                if (redirect.Equals("PaymentHistory"))
                    return RedirectToAction("PaymentHistory");
                else
                    return Redirect("/Home/Dashboard");
            }

            return BadRequest();
        }

        [HttpGet("CreditStatus/{service}")]
        public async Task<IActionResult> CreditStatus(string service)
        {
            if (!string.IsNullOrEmpty(service))
            {
                var client = new HttpClient();
                var user = db.AspNetUsers.Where(i => i.Email == User.Identity.Name).FirstOrDefault();
                var credits = await client.GetAsync($"http://localhost:800/Payments/Credits/{user.UserName}").Result.Content.ReadAsStringAsync();
                dynamic json_data_credits = JsonConvert.DeserializeObject(credits);
                CreditCounts creditsFromDb = JsonConvert.DeserializeObject<CreditCounts>(json_data_credits.ToString());

                if(service.Equals("Name search"))
                {
                    if (creditsFromDb.NameSearch > 0)
                    {
                        return Redirect("/Products/AddNewProduct");
                    }
                }
                
                if(service.Equals("Private Limited Entity"))
                {
                    if (creditsFromDb.PvtLimitedCompany > 0)
                    {
                        return Redirect("/Company/CompanyApplication");
                    }
                }
                
            }

            return View();
        }
    }
}
