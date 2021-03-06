﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Server.Models;
using Server.Models.DTO;
using Server.Hubs;
using Server.Models.Context;

namespace Server.Controllers
{
    public class QuestionsController : ApiControllerWithHub<EventHub>
    {
        public QuestionsController() { }
        public QuestionsController(IDocentAppContext context)
        {
            this.db = context;
        }

        // GET: api/Question
        public IQueryable<QuestionDTO> GetQuestions()
        {
            var questions = from q in db.Questions
                            select new QuestionDTO()
                            {
                                Id = q.Id,
                                Text = q.Text,
                                List_Id = q.List.Id,
                                Time = q.Time,
                                Points = q.Points,
                                PredefinedAnswerCount = q.PredefinedAnswerCount,
                                PredefinedAnswers = q.PredefinedAnswers.Select(V => new PredefinedAnswerDTO { Id = V.Id, Text = V.Text,Right_Answer = V.Right_Answer, Question_Id = V.Question.Id }).ToList<PredefinedAnswerDTO>(),
                                UserAnswers = q.UserAnswers.Select(UA => new UserAnswerDTO { Id = UA.Id, Question_Id = UA.Question_Id, PredefinedAnswer_Id = UA.PredefinedAnswer_Id }).ToList<UserAnswerDTO>()
                            };

            return questions;
        }

        // GET: api/Question/5
        [ResponseType(typeof(QuestionDTO))]
        public QuestionDTO GetQuestionById(int id)
        {
            var questions = from q in db.Questions
                            where q.Id == id
                            select new QuestionDTO()
                            {
                                Id = q.Id,
                                Text = q.Text,
                                List_Id = q.List.Id,
                                Time = q.Time,
                                Points = q.Points,
                                PredefinedAnswerCount = q.PredefinedAnswerCount,
                                PredefinedAnswers = q.PredefinedAnswers.Select(V => new PredefinedAnswerDTO { Id = V.Id, Text = V.Text, Question_Id = V.Question.Id }).ToList<PredefinedAnswerDTO>(),
                                UserAnswers = q.UserAnswers.Select(UA => new UserAnswerDTO { Id = UA.Id, Question_Id = UA.Question_Id, PredefinedAnswer_Id = UA.PredefinedAnswer_Id }).ToList<UserAnswerDTO>()
                            };

            return questions.FirstOrDefault(x => x.Id == x.Id);
        }

        // PUT: api/Questions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutQuestion(int id, Question question)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != question.Id)
            {
                return BadRequest();
            }

            db.MarkAsModified(question);

            try
            {
                await db.SaveChangesAsync();

                this.getSubscribed("List-" + question.List_Id).QuestionUpdated(new QuestionDTO(question));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Questions
        [ResponseType(typeof(Question))]
        public async Task<IHttpActionResult> PostQuestion(Question question)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Questions.Add(question);
            await db.SaveChangesAsync();

            this.getSubscribed("List-" + question.List_Id).QuestionAdded(new QuestionDTO(question));

            return CreatedAtRoute("DefaultApi", new { id = question.Id }, question);
        }

        // DELETE: api/Questions/5
        [ResponseType(typeof(Question))]
        public async Task<IHttpActionResult> DeleteQuestion(int id)
        {
            Question question = await db.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            db.Questions.Remove(question);
            await db.SaveChangesAsync();

            this.getSubscribed("List-" + question.List_Id).QuestionRemoved(new QuestionDTO(question));

            return Ok(question);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool QuestionExists(int id)
        {
            return db.Questions.Count(e => e.Id == id) > 0;
        }
    }
}