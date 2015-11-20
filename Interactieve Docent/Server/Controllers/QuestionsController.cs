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

namespace Server.Controllers
{
    public class QuestionsController : ApiControllerWithHub<EventHub>
    {
        private ServerContext db = new ServerContext();

        // GET: api/Question
        public IQueryable<QuestionDTO> GetQuestions()
        {
            var questions = from q in db.Questions select new QuestionDTO() {
                Id = q.Id,
                Text = q.Text,
                List_Id = q.List.Id,
                PredefinedAnswers = q.PredefinedAnswers.Select(V => new PredefinedAnswerDTO { Id = V.Id, Text = V.Text, Question_Id = V.Question.Id }).ToList<PredefinedAnswerDTO>()

            };

            return questions;
        }

        // GET: api/Question/5
        [ResponseType(typeof(QuestionDTO))]
        public QuestionDTO GetQuestionById(int id)
        {
            var vragen = from q in db.Questions
                        where q.Id == id
                        select new QuestionDTO()
                        {
                            Id = q.Id,
                            Text = q.Text,
                            List_Id = q.List.Id,
                            PredefinedAnswers = q.PredefinedAnswers.Select(V => new PredefinedAnswerDTO { Id = V.Id, Text = V.Text, Question_Id = V.Question.Id }).ToList<PredefinedAnswerDTO>()
                        };

            QuestionDTO vraag = vragen.First();
            return vraag;
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

            db.Entry(question).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
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

            var subscribed = Hub.Clients.Group(question.List_Id.ToString());
            subscribed.QuestionAdded(new QuestionDTO(question));

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