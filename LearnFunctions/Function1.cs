using Azure.Storage.Queues.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using MortgageCalculator;

namespace LearnFunctions
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;
        private readonly Loan _defaultLoan;

        public Function1(ILogger<Function1> logger, Loan defaultLoan)
        {
            _logger = logger;
            _defaultLoan = defaultLoan;
        }

        [Function("Loan")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult($"AJ your loan's monly payment is: ${_defaultLoan.MonthlyPayment}! The balance left is " +
                $"${_defaultLoan.BalanceLeftOnLoan}");
        }

        [Function("QueueTriggerFunction")]
        [QueueOutput("output-queue")]
        public string[] Run2([QueueTrigger("input-queue")] QueueMessage myQueueItem, FunctionContext context)
        {
            // Use a string array to return more than one message.
            string[] messages = {
                $"message inserted on = {myQueueItem.InsertedOn}",
                $"message bidt = {myQueueItem.Body}"
            };

            _logger.LogInformation("{msg1},{msg2}", messages[0], messages[1]);

            // Queue Output messages
            return messages;
        }
    }
}
