using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SquidWords.Data;
using SquidWords.Models;

namespace SquidWords.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordsController : ControllerBase
    {
        private readonly ApplicationDbContext db;

        public WordsController(ApplicationDbContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Word>>> Get()
        {
            return await db.Words.ToListAsync();
        }

        // GET api/words/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Word>> Get(int id)
        {
            Word user = await db.Words.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                return NotFound();
            return new ObjectResult(user);
        }

        //POST api/words
        [HttpPost]
        public async Task<ActionResult<Word>> Post(Word word)
        {
            if (word == null)
            {
                return BadRequest();
            }

            db.Words.Add(word);
            await db.SaveChangesAsync();
            return Ok(word);
        }

        //PUT api/words/
        [HttpPut]
        public async Task<ActionResult<Word>> Put(Word word)
        {
            if (word == null)
            {
                return BadRequest();
            }
            if (!db.Words.Any(x => x.Id == word.Id))
            {
                return NotFound();
            }

            db.Update(word);
            await db.SaveChangesAsync();
            return Ok(word);
        }

        //DELETE api/words/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Word>> Delete(int id)
        {
            Word word = db.Words.FirstOrDefault(x => x.Id == id);
            if (word == null)
            {
                return NotFound();
            }
            db.Words.Remove(word);
            await db.SaveChangesAsync();
            return Ok(word);
        }
    }



}