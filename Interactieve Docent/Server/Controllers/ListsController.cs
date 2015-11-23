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
using System.IO;
using System.Web;
using Server;
using Server.Hubs;

namespace Server.Controllers
{
    public class ListsController : ApiControllerWithHub<EventHub>
    {
        private ServerContext db = new ServerContext();

        // GET: api/Lists
        public IQueryable<ListDTO> GetLists()
        {
            var Lists = from q in db.Lists select new ListDTO() {
                Id = q.Id,
                Name = q.Name,
                Questions = q.Questions.Select(C => new QuestionDTO{ Id = C.Id }).ToList<QuestionDTO>()
            };

            return Lists;
        }

        // GET: api/Lists/5
        [ResponseType(typeof(ListDTO))]
        public ListDTO GetList(int id)
        {
            var Lists = from q in db.Lists
                        where q.Id == id
                        select new ListDTO()
                        {
                            Id = q.Id,
                            Name = q.Name,
                            Questions = q.Questions.Select(C => new QuestionDTO { Id = C.Id, Text = C.Text, PredefinedAnswers = (C.PredefinedAnswers.Select(V => new PredefinedAnswerDTO { Id = V.Id, Text = V.Text, Question_Id = V.Question.Id })).ToList<PredefinedAnswerDTO>() }).ToList<QuestionDTO>()
                        };
            ListDTO lijst = Lists.First();
            return lijst;
        }

        // PUT: api/Lists/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutList(int id, List list)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != list.Id)
            {
                return BadRequest();
            }

            db.Entry(list).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ListExists(id))
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

        // POST: api/Lists
        [ResponseType(typeof(List))]
        public async Task<IHttpActionResult> PostList([FromBody] List list)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Lists.Add(list);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = list.Id }, list);
        }

        // DELETE: api/Lists/5
        [ResponseType(typeof(List))]
        public async Task<IHttpActionResult> DeleteList(int id)
        {
            List list = await db.Lists.FindAsync(id);
            if (list == null)
            {
                return NotFound();
            }

            db.Lists.Remove(list);
            await db.SaveChangesAsync();

            return Ok(list);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ListExists(int id)
        {
            return db.Lists.Count(e => e.Id == id) > 0;
        }
    }
}