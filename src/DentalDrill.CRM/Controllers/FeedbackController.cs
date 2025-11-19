using System;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.Validation;
using DentalDrill.CRM.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalDrill.CRM.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IServiceProvider serviceProvider;

        public FeedbackController(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task<IActionResult> Form(Guid id)
        {
            var feedbackForm = await FeedbackFormDomainModel.GetByIdAsync(this.serviceProvider, id);
            if (feedbackForm == null)
            {
                return this.NotFound();
            }

            if (feedbackForm.Status != FeedbackFormStatus.New)
            {
                return this.View("NotAvailable");
            }

            await feedbackForm.OpenAsync();
            var answers = feedbackForm.GetQuestionsAndAnswers();
            var model = new FeedbackFormFillModel
            {
                Form = feedbackForm,
                Answers = answers,
            };

            model.Answer = model.Answers.ToDictionary(x => x.Question.Id, x => x.Question.Type switch
            {
                FeedbackFormQuestionType.Rating => new FeedbackFormFillModelRatingAnswer { Value = x.IntegerAnswer } as FeedbackFormFillModelAnswer,
                FeedbackFormQuestionType.MultilineText => new FeedbackFormFillModelMultilineTextAnswer { Value = x.StringAnswer } as FeedbackFormFillModelAnswer,
                _ => throw new InvalidOperationException(),
            });

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Form(Guid id, FeedbackFormFillModel model)
        {
            var feedbackForm = await FeedbackFormDomainModel.GetByIdAsync(this.serviceProvider, id);
            if (feedbackForm == null)
            {
                return this.NotFound();
            }

            if (feedbackForm.Status != FeedbackFormStatus.New)
            {
                return this.View("NotAvailable");
            }

            await feedbackForm.OpenAsync();
            var answers = feedbackForm.GetQuestionsAndAnswers();
            model.Form = feedbackForm;
            model.Answers = answers;

            var validation = feedbackForm.ValidateAnswers(model.Answer);
            validation.MergeToModelState(this.ModelState, "Answer");

            if (this.ModelState.IsValid)
            {
                await feedbackForm.CompleteFormAsync(model.Answer);
                return this.RedirectToAction("Complete", new { id = id });
            }

            return this.View(model);
        }

        public async Task<IActionResult> Complete(Guid id)
        {
            var feedbackForm = await FeedbackFormDomainModel.GetByIdAsync(this.serviceProvider, id);
            if (feedbackForm == null)
            {
                return this.NotFound();
            }

            if (feedbackForm.Status != FeedbackFormStatus.Completed || feedbackForm.CompletedOn == null || feedbackForm.CompletedOn.Value < DateTime.UtcNow.AddDays(-1))
            {
                return this.NotFound();
            }

            return this.View();
        }

        [Authorize(Roles = ApplicationRoles.Administrator)]
        public async Task<IActionResult> EmailDebug(Guid id)
        {
            var feedbackForm = await FeedbackFormDomainModel.GetByIdAsync(this.serviceProvider, id);
            if (feedbackForm == null)
            {
                return this.NotFound();
            }

            if (feedbackForm.Status != FeedbackFormStatus.New)
            {
                return this.NotFound();
            }

            await feedbackForm.OpenAsync();
            var answers = feedbackForm.GetQuestionsAndAnswers();
            var model = new FeedbackFormFillModel
            {
                Form = feedbackForm,
                Answers = answers,
            };

            return this.View(model);
        }
    }
}
