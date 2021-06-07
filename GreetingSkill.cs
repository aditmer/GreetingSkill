using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;
using AzureCognitiveSearch.PowerSkills.Common;

namespace Microsoft.GreetingSkill
{
    public static class GreetingSkill
    {
        [FunctionName("GreetingSkill")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Hello World Custom Skill: C# HTTP trigger function processed a request.");

            string skillName = "GreetingSkill"; //System.Threading.ExecutionContext.FunctionName;
            IEnumerable<WebApiRequestRecord> requestRecords = WebApiSkillHelpers.GetRequestRecords(req);
            if (requestRecords == null)
            {
                return new BadRequestObjectResult($"{skillName} - Invalid request record array.");
            }

            WebApiSkillResponse response = WebApiSkillHelpers.ProcessRequestRecords(skillName, requestRecords,
                (inRecord, outRecord) => {
                    var name = inRecord.Data["name"] as string;
                    outRecord.Data["greeting"] = $"Hello, {name}";
                    return outRecord;
                });

            return new OkObjectResult(response);
        }
    }
}
