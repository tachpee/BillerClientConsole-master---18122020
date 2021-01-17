using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BillerClientConsole.Globals;
using BillerClientConsole.Data;
using BillerClientConsole.Models;
using BillerClientConsole.Models.QueryModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Webdev.Http;

namespace BillerClientConsole.Controllers
{
    [Route("Queries")]
    public class QueriesController : Controller
    {
        private readonly QueryDbContext context;

        public QueriesController(QueryDbContext context)
        {
            this.context = context;
        }
        [HttpPost("RegisteredOfficeHasQuery")]
        public async Task<IActionResult> RegisteredOfficeHasQuery(Queries query)
        {
            //Code to check if there exist a Query with same ref number
            var QueryExits = context.Queries.Where(q => q.applicationRef == query.applicationRef && q.tableName == "Step2");
            if (QueryExits.Count() > 0)
            {
                if (query.HasQuery == true)
                {
                    //If it exists and has another query Update the current 
                    Queries queryExists = QueryExits.FirstOrDefault();
                    queryExists.QueryCount += 1;
                    queryExists.comment = query.comment;
                    queryExists.status = "Pending";


                    context.Queries.Update(queryExists);
                    await context.SaveChangesAsync();

                    var forqueryhistory = new QueryHistory()
                    {
                        applicationRef = query.applicationRef,
                        comment = query.comment,
                        dateCreated = DateTime.Now.ToString(),
                        status = "Pending",
                        tableName = "Step2",
                    };
                    context.QueryHistory.Add(forqueryhistory);
                    await context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    var queryExists = QueryExits.FirstOrDefault();
                    queryExists.status = "Resolved";
                    queryExists.HasQuery = false;
                    context.Queries.Update(queryExists);
                    await context.SaveChangesAsync();
                    //Updating the Query History Table
                    var forqueryhistory = new QueryHistory()
                    {
                        applicationRef = query.applicationRef,
                        comment = query.comment,
                        dateCreated = DateTime.Now.ToString(),
                        status = "Resolved",
                        tableName = "Step2",

                    };
                    context.QueryHistory.Add(forqueryhistory);
                    await context.SaveChangesAsync();
                    return Ok();
                }
            }
            else
            {
                if (query.HasQuery == true)
                {
                    query.status = "Pending";
                    query.dateCreated = DateTime.Now.ToString();
                    query.QueryCount += 1;
                    query.tableName = "Step2";
                    context.Queries.Add(query);
                    await context.SaveChangesAsync();

                    var forqueryhistory = new QueryHistory()
                    {
                        applicationRef = query.applicationRef,
                        comment = query.comment,
                        dateCreated = DateTime.Now.ToString(),
                        status = "Pending",
                        tableName = "Step2",
                    };
                    context.QueryHistory.Add(forqueryhistory);
                    await context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
        }
        //ShareClause Query Handling
        [HttpPost("LiabilityAndShareClauseHasQuery")]
        public async Task<IActionResult> ShareClauseHasQuery(Queries query)
        {
            var QueryExits = context.Queries.Where(q => q.applicationRef == query.applicationRef && q.tableName == "Step4");
            if (QueryExits.Count() > 0)
            {
                if (query.HasQuery == true)
                {
                    //If it exists and has another query Update the current 
                    Queries queryExists = QueryExits.FirstOrDefault();
                    queryExists.QueryCount += 1;
                    queryExists.comment = query.comment; ;
                    queryExists.status = "Pending";

                    context.Queries.Update(queryExists);
                    await context.SaveChangesAsync();

                    var forqueryhistory = new QueryHistory()
                    {
                        applicationRef = query.applicationRef,
                        comment = query.comment,
                        dateCreated = DateTime.Now.ToString(),
                        status = "Pending",
                        tableName = "Step4",
                    };
                    context.QueryHistory.Add(forqueryhistory);
                    await context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    var queryExists = QueryExits.FirstOrDefault();
                    queryExists.status = "Resolved";
                    queryExists.HasQuery = false;
                    context.Queries.Update(queryExists);
                    await context.SaveChangesAsync();
                    //Updating the Query History Table
                    var forqueryhistory = new QueryHistory()
                    {
                        applicationRef = query.applicationRef,
                        comment = query.comment,
                        dateCreated = DateTime.Now.ToString(),
                        status = "Resolved",
                        tableName = "Step4",

                    };
                    context.QueryHistory.Add(forqueryhistory);
                    await context.SaveChangesAsync();
                    return Ok();
                }
            }
            else
            {
                if (query.HasQuery == true)
                {
                    query.status = "Pending";
                    query.dateCreated = DateTime.Now.ToString();
                    query.QueryCount += 1;
                    query.tableName = "Step4";
                    context.Queries.Add(query);
                    await context.SaveChangesAsync();

                    var forqueryhistory = new QueryHistory()
                    {
                        applicationRef = query.applicationRef,
                        comment = query.comment,
                        dateCreated = DateTime.Now.ToString(),
                        status = "Pending",
                        tableName = "Step4",
                    };
                    context.QueryHistory.Add(forqueryhistory);
                    await context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        [HttpPost("MembersHasQuery")]
        public async Task<IActionResult> MembersHasQuery(Queries query)
        {
            var QueryExits = context.Queries.Where(q => q.applicationRef == query.applicationRef && q.tableName == "Step3");
            if (QueryExits.Count() > 0)
            {
                if (query.HasQuery == true)
                {
                    //If it exists and has another query Update the current 
                    Queries queryExists = QueryExits.FirstOrDefault();
                    queryExists.QueryCount += 1;
                    queryExists.comment = query.comment; ;
                    queryExists.status = "Pending";

                    context.Queries.Update(queryExists);
                    await context.SaveChangesAsync();

                    var forqueryhistory = new QueryHistory()
                    {
                        applicationRef = query.applicationRef,
                        comment = query.comment,
                        dateCreated = DateTime.Now.ToString(),
                        status = "Pending",
                        tableName = "Step3",
                    };
                    context.QueryHistory.Add(forqueryhistory);
                    await context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    var queryExists = QueryExits.FirstOrDefault();
                    queryExists.status = "Resolved";
                    queryExists.HasQuery = false;
                    context.Queries.Update(queryExists);
                    await context.SaveChangesAsync();
                    //Updating the Query History Table
                    var forqueryhistory = new QueryHistory()
                    {
                        applicationRef = query.applicationRef,
                        comment = query.comment,
                        dateCreated = DateTime.Now.ToString(),
                        status = "Resolved",
                        tableName = "Step3",

                    };
                    context.QueryHistory.Add(forqueryhistory);
                    await context.SaveChangesAsync();
                    return Ok();
                }
            }
            else
            {
                if (query.HasQuery == true)
                {
                    query.status = "Pending";
                    query.dateCreated = DateTime.Now.ToString();
                    query.QueryCount += 1;
                    query.tableName = "Step3";
                    context.Queries.Add(query);
                    await context.SaveChangesAsync();

                    var forqueryhistory = new QueryHistory()
                    {
                        applicationRef = query.applicationRef,
                        comment = query.comment,
                        dateCreated = DateTime.Now.ToString(),
                        status = "Pending",
                        tableName = "Step3",
                    };
                    context.QueryHistory.Add(forqueryhistory);
                    await context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
        }
        [HttpGet("ResolveMembersQuery")]
        public ActionResult ResolveMembersQuery(string applicationID)
        {
            var client = new HttpClient();
            var rhisponzi = await client.GetAsync($"{Globals.Globals.end_point_get_company_application_by_search_ref}?SearchRef={applicationID}").Result.Content.ReadAsStringAsync();

            dynamic json_dataa = JsonConvert.DeserializeObject(rhisponzi);
            dynamic dataa = json_dataa;
            try
            {
                dataa = json_dataa.data.value;
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            mCompanyResponse companyApplication = JsonConvert.DeserializeObject<mCompanyResponse>(dataa.ToString());
            ViewBag.companyApplication = companyApplication;
            ///ViewBag.CompanyApplication = companyApplication;
            return View(companyApplication);
        }
        [HttpGet("ResolveQuery/{id}")]
        [HttpGet("ResolveQuery/{applicationRef}")]
        public async Task<IActionResult> ResolveQuery( string step, string applicationRef, string id=null, string applicationID=null)
        {
           
            //Code to get Registered Office Details
            if (step == "Step2")
            {
                var registeredOfficeExists = await client.GetAsync($"{Globals.Globals.service_end_point}/RegisteredOffice/{id}").Result.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<RegisteredOffice>(registeredOfficeExists);
                return View(model);
            }
            else if (step == "Step3")
            {

                //ViewBag.CompanyApplication = companyApplication;
                // Redirecting to an another Action with the model data from database........
                return RedirectToAction("ResolveMembersQuery", new{model=companyApplication});
                ///return ResolveMembersQuery(companyApplication);
            }
            else if (step == "Step4")
            {
                return RedirectToAction("",companyApplication);
            }

                   
            return NotFound();
        }

        [HttpPost("ResolveQueryPostRegisteredOffice")]

        public async Task<IActionResult> ResolveQueryPostRegisteredOffice(RegisteredOffice model)
        {
            var client = new HttpClient();
            if (ModelState.IsValid)
            {
               var result= await client.PostAsJsonAsync($"{Globals.Globals.service_end_point}/UpdateRegisteredOffice", model);
                if (result.IsSuccessStatusCode)
                {
                    return Ok();
                }   
            }
            return Ok();
        }
    }
}
